using E_Learning.Application.Common;
using E_Learning.Application.DTOs.Course;
using E_Learning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Services.Interface
{
    public interface ICourseService
    {
        Task<ServiceResult<PagedResult<CourseResponseDto>>> GetAllAsync(int PageSize = 0, int Page = 1);
        Task<ServiceResult<CourseResponseDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<CourseResponseDto>> CreateAsync(CourseCreateDto dto, UserContext user);
        Task<ServiceResult<CourseResponseDto>> UpdateAsync(Guid id, CourseUpdateDto dto, UserContext user);
        Task<ServiceResult<bool>> DeleteAsync(Guid id, UserContext user);
    }
}
