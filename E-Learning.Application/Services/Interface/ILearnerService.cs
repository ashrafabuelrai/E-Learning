using E_Learning.Application.Common;
using E_Learning.Application.DTOs.Learner;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Services.Interface
{
    public interface ILearnerService
    {
        Task<ServiceResult<PagedResult<LearnerResponseDto>>> GetAllAsync(int PageSize = 0, int Page = 1);
        Task<ServiceResult<LearnerResponseDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<LearnerResponseDto>> CreateAsync(LearnerCreateDto dto);
    }
}
