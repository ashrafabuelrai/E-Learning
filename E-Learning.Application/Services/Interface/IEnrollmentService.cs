using E_Learning.Application.Common;
using E_Learning.Application.DTOs.Enrollment;
using E_Learning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Services.Interface
{
    public interface IEnrollmentService
    {
        Task<ServiceResult<PagedResult<EnrollmentResponseDto>>> GetAllAsync(EnrollmentFilterDto filter);
        Task<ServiceResult<EnrollmentResponseDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<EnrollmentResponseDto>> EnrollAsync(EnrollmentCreateDto dto, UserContext user);
        Task<ServiceResult<EnrollmentResponseDto>> MakeDecisionAsync(Guid enrollmentId, EnrollmentDecisionDto dto, UserContext user);
    }
}
