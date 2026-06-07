using E_Learning.Application.Common;
using E_Learning.Application.DTOs.Course;
using E_Learning.Application.DTOs.Enrollment;
using E_Learning.Application.DTOs.Learner;
using E_Learning.Application.Interfaces;
using E_Learning.Application.Services.Interface;
using E_Learning.Domain.Entities;
using E_Learning.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_Learning.Application.Services.Implementation
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork; 

        public EnrollmentService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ServiceResult<PagedResult<EnrollmentResponseDto>>> GetAllAsync(EnrollmentFilterDto filter)
        {
            IQueryable<Enrollment> query = await _unitOfWork.Enrollments.GetAll(null, "Course,Learner");

            if (filter.LearnerId.HasValue)
                query = query.Where(e => e.LearnerId == filter.LearnerId.Value);

            if (filter.CourseId.HasValue)
                query = query.Where(e => e.CourseId == filter.CourseId.Value);

            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                Enum.TryParse<EnrollmentStatus>(filter.Status, true, out var status))
                query = query.Where(e => e.Status == status);

            if (filter.FromDate.HasValue)
                query = query.Where(e => e.CreatedAt >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(e => e.CreatedAt <= filter.ToDate.Value);

            var totalCount = query.Count();

            var pageSize = Math.Max(1, Math.Min(filter.PageSize, 100));
            var page = Math.Max(1, filter.Page);

            var items = query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => MapToDto(e))
                .ToList();

            return ServiceResult<PagedResult<EnrollmentResponseDto>>.Success(new PagedResult<EnrollmentResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        public async Task<ServiceResult<EnrollmentResponseDto>> GetByIdAsync(Guid id)
        {
            var enrollment = await _unitOfWork.Enrollments.Get(e => e.Id == id,"Course,Learner");
                

            if (enrollment is null)
                return ServiceResult<EnrollmentResponseDto>.NotFound($"Enrollment with ID {id} not found.");

            return ServiceResult<EnrollmentResponseDto>.Success(MapToDto(enrollment));
        }

        public async Task<ServiceResult<EnrollmentResponseDto>> EnrollAsync(EnrollmentCreateDto dto, UserContext user)
        {
            if (user.Role != UserRole.Learner)
                return ServiceResult<EnrollmentResponseDto>.Forbidden("Only Learners can request enrollment.");

            var learner = await _unitOfWork.Learners.Get(l=>l.Id==dto.LearnerId);
            if (learner is null)
                return ServiceResult<EnrollmentResponseDto>.NotFound($"Learner with ID {dto.LearnerId} not found.");

            var course = await _unitOfWork.Courses.Get(c=>c.Id==dto.CourseId);
            if (course is null)
                return ServiceResult<EnrollmentResponseDto>.NotFound($"Course with ID {dto.CourseId} not found.");

            if (!course.IsActive)
                return ServiceResult<EnrollmentResponseDto>.Failure("Cannot enroll in an inactive course.");

            var alreadyEnrolled = await _unitOfWork.Enrollments.Get(e => e.LearnerId == dto.LearnerId && e.CourseId == dto.CourseId);

            if (alreadyEnrolled!=null)
                return ServiceResult<EnrollmentResponseDto>.Failure("Learner is already enrolled in this course.");

            var enrollment = new Enrollment
            {
                LearnerId = dto.LearnerId,
                CourseId = dto.CourseId,
                Status = course.RequiresApproval ? EnrollmentStatus.PendingApproval : EnrollmentStatus.Approved,
                CreatedAt = DateTime.UtcNow,
                
            };

            await _unitOfWork.Enrollments.Add(enrollment);
            await _unitOfWork.SaveChangesAsync();
            var e=new EnrollmentResponseDto
            {

                Id = enrollment.Id,
                Status = enrollment.Status,
                CreatedAt = (DateTime)enrollment.CreatedAt,
                DecisionDate = enrollment.DecisionDate,
                DecisionReason = enrollment.DecisionReason,
                Learner = new LearnerResponseDto
                {
                    Id = learner.Id,
                    FullName = learner.FullName,
                    Email = learner.Email,
                    Department = learner.Department
                },
                Course = new CourseResponseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    DurationHours = course.DurationHours,
                    RequiresApproval = course.RequiresApproval
                }
            };
            return ServiceResult<EnrollmentResponseDto>.Success(e, 201);
        }

        public async Task<ServiceResult<EnrollmentResponseDto>> MakeDecisionAsync(
            Guid enrollmentId, EnrollmentDecisionDto dto, UserContext user)
        {
            if (user.Role!=UserRole.Manager)
                return ServiceResult<EnrollmentResponseDto>.Forbidden("Only Managers can approve or reject enrollments.");

            if (!Enum.TryParse<EnrollmentStatus>(dto.Decision, true, out var decision))
                return ServiceResult<EnrollmentResponseDto>.Failure("Decision must be 'Approved' or 'Rejected'.");

            if (decision != EnrollmentStatus.Approved && decision != EnrollmentStatus.Rejected)
                return ServiceResult<EnrollmentResponseDto>.Failure("Decision must be 'Approved' or 'Rejected'.");

            if (decision == EnrollmentStatus.Rejected && string.IsNullOrWhiteSpace(dto.Reason))
                return ServiceResult<EnrollmentResponseDto>.Failure("A reason is required when rejecting an enrollment.");

            var enrollment = await _unitOfWork.Enrollments.Get(e=>e.Id== enrollmentId,"Course,Learner");

            if (enrollment is null)
                return ServiceResult<EnrollmentResponseDto>.NotFound($"Enrollment with ID {enrollmentId} not found.");

            if (enrollment.Status != EnrollmentStatus.PendingApproval)
                return ServiceResult<EnrollmentResponseDto>.Failure(
                    $"Only PendingApproval enrollments can be decided. Current status: {enrollment.Status}.");

            var oldValue = JsonSerializer.Serialize(new
            {
                enrollment.Status,
                enrollment.DecisionReason,
                enrollment.DecisionDate
            });

            enrollment.Status = decision;
            enrollment.DecisionDate = DateTime.UtcNow;
            enrollment.DecisionReason = dto.Reason;

            var newValue = JsonSerializer.Serialize(new
            {
                enrollment.Status,
                enrollment.DecisionReason,
                enrollment.DecisionDate
            });

            var auditLog = new AuditLog
            {
                EntityName = "Enrollment",
                EntityId = enrollment.Id,
                Action = decision.ToString(),
                OldValue = oldValue,
                NewValue = newValue,
                PerformedBy = user.UserId,
                PerformedAt = DateTime.UtcNow,
                EnrollmentId = enrollment.Id
            };

            await _unitOfWork .AuditLogs.Add(auditLog);
            await _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<EnrollmentResponseDto>.Success(MapToDto(enrollment));
        }

        private static EnrollmentResponseDto MapToDto(Enrollment e) => new()
        {
            Id = e.Id,
            Status = e.Status,
            CreatedAt = (DateTime)e.CreatedAt,
            DecisionDate = e.DecisionDate,
            DecisionReason = e.DecisionReason,
            Learner = new LearnerResponseDto
            {
                Id = e.Learner.Id,
                FullName = e.Learner.FullName,
                Email = e.Learner.Email,
                Department = e.Learner.Department
            },
            Course = new CourseResponseDto
            {
                Id = e.Course.Id,
                Title = e.Course.Title,
                DurationHours = e.Course.DurationHours,
                RequiresApproval = e.Course.RequiresApproval
            }
        };
    }
}
