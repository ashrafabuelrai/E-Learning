using E_Learning.Application.Common;
using E_Learning.Application.DTOs.Course;
using E_Learning.Application.DTOs.Learner;
using E_Learning.Application.Interfaces;
using E_Learning.Application.Services.Interface;
using E_Learning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Services.Implementation
{
    public class LearnerService : ILearnerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LearnerService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ServiceResult<PagedResult<LearnerResponseDto>>> GetAllAsync(int PageSize = 0, int Page = 1)
        {
            IQueryable<Learner> query = await _unitOfWork.Learners.GetAll();
            var totalCount = query.Count();

            var pageSize = Math.Max(1, Math.Min(PageSize, 100));
            var page = Math.Max(1, Page);
            var items = query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => MapToDto(e))
                .ToList();

            return ServiceResult<PagedResult<LearnerResponseDto>>.Success(new PagedResult<LearnerResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }

        public async Task<ServiceResult<LearnerResponseDto>> GetByIdAsync(Guid id)
        {
            var learner = await _unitOfWork.Learners.Get(l => l.Id == id);
            if (learner is null)
                return ServiceResult<LearnerResponseDto>.NotFound($"Learner with ID {id} not found.");

            return ServiceResult<LearnerResponseDto>.Success(MapToDto(learner));
        }

        public async Task<ServiceResult<LearnerResponseDto>> CreateAsync(LearnerCreateDto dto)
        {
            var nationalIdExists = await _unitOfWork.Learners.Get(l => l.NationalId == dto.NationalId);
            if (nationalIdExists != null)
                return ServiceResult<LearnerResponseDto>.Failure("A learner with this NationalId already exists.");

            var emailExists = await _unitOfWork.Learners.Get(l => l.Email == dto.Email);
            if (emailExists != null)
                return ServiceResult<LearnerResponseDto>.Failure("A learner with this email already exists.");

            var learner = new Learner
            {
                FullName = dto.FullName,
                Email = dto.Email,
                NationalId = dto.NationalId,
                Department = dto.Department,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Learners.Add(learner);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<LearnerResponseDto>.Success(MapToDto(learner), 201);
        }

        private static LearnerResponseDto MapToDto(Learner l) => new()
        {
            Id = l.Id,
            FullName = l.FullName,
            Email = l.Email,
            NationalId = l.NationalId,
            Department = l.Department,
            CreatedAt = (DateTime)l.CreatedAt
        };
    }
}