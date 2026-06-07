using E_Learning.Application.Common;
using E_Learning.Application.DTOs.Course;
using E_Learning.Application.DTOs.Enrollment;
using E_Learning.Application.Interfaces;
using E_Learning.Application.Services.Interface;
using E_Learning.Domain.Entities;
using E_Learning.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace E_Learning.Application.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork) => _unitOfWork=unitOfWork;

        public async Task<ServiceResult<PagedResult<CourseResponseDto>>> GetAllAsync(int PageSize = 0, int Page = 1)
        {
            IQueryable<Course> query = await _unitOfWork.Courses.GetAll();
            var totalCount = query.Count();

            var pageSize = Math.Max(1, Math.Min(PageSize, 100));
            var page = Math.Max(1, Page);
            var items = query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => MapToDto(e))
                .ToList();

            return ServiceResult<PagedResult<CourseResponseDto>>.Success(new PagedResult<CourseResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
            });
        }

        public async Task<ServiceResult<CourseResponseDto>> GetByIdAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.Get(c=>c.Id==id);
            if (course is null)
                return ServiceResult<CourseResponseDto>.NotFound($"Course with ID {id} not found.");

            return ServiceResult<CourseResponseDto>.Success(MapToDto(course));
        }

        public async Task<ServiceResult<CourseResponseDto>> CreateAsync(CourseCreateDto dto, UserContext user)
        {
            if (user.Role!=UserRole.Admin)
                return ServiceResult<CourseResponseDto>.Forbidden("Only Admins can create courses.");

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                DurationHours = dto.DurationHours,
                RequiresApproval = dto.RequiresApproval,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Courses.Add(course);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<CourseResponseDto>.Success(MapToDto(course), 201);
        }

        public async Task<ServiceResult<CourseResponseDto>> UpdateAsync(Guid id, CourseUpdateDto dto, UserContext user)
        {
            if (user.Role!=UserRole.Admin)
                return ServiceResult<CourseResponseDto>.Forbidden("Only Admins can update courses.");

            var course = await _unitOfWork.Courses.Get(c=>c.Id==id);
            if (course is null)
                return ServiceResult<CourseResponseDto>.NotFound($"Course with ID {id} not found.");

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.DurationHours = dto.DurationHours;
            course.RequiresApproval = dto.RequiresApproval;
            course.IsActive = dto.IsActive;
            await _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<CourseResponseDto>.Success(MapToDto(course));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id, UserContext user)
        {
            if (user.Role != UserRole.Admin)
                return ServiceResult<bool>.Forbidden("Only Admins can delete courses.");

            var course = await _unitOfWork.Courses.Get(c=>c.Id==id);
            if (course is null)
                return ServiceResult<bool>.NotFound($"Course with ID {id} not found.");

            var hasEnrollments = await _unitOfWork.Enrollments.Get(e => e.CourseId==id);
            if (hasEnrollments!=null)
                return ServiceResult<bool>.Failure("Cannot delete a course with existing enrollments.", 409);

            await _unitOfWork.Courses.Remove(course);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        private static CourseResponseDto MapToDto(Course c) => new()
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            DurationHours = c.DurationHours,
            RequiresApproval = c.RequiresApproval,
            IsActive = c.IsActive,
            CreatedAt = (DateTime)c.CreatedAt
        };
    }
}
 