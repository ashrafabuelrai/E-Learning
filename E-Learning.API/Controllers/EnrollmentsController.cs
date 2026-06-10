using E_Learning.Application.DTOs.Enrollment;
using E_Learning.Application.Services.Interface;
using E_Learning.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
            => _enrollmentService = enrollmentService;

        private UserContext GetUserContext() =>
            HttpContext.Items["UserContext"] as UserContext ?? new UserContext();


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EnrollmentFilterDto filter)
        {
           
                var result = await _enrollmentService.GetAllAsync(filter);
                if(!result.IsSuccess)
                    return NotFound(result);
                return Ok(result);
            
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            
                var result = await _enrollmentService.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return NotFound(result);
                return Ok(result);
           
        }

        
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentCreateDto dto)
        {
            
                var result = await _enrollmentService.EnrollAsync(dto, GetUserContext());
                if (!result.IsSuccess)
                    return BadRequest(result);
                return Ok(result);
            
        }

      
        [HttpPost("{id}/decision")]
        public async Task<IActionResult> MakeDecision(Guid id, [FromBody] EnrollmentDecisionDto dto)
        {
           
                var result = await _enrollmentService.MakeDecisionAsync(id, dto, GetUserContext());
                if (!result.IsSuccess)
                    return BadRequest(result);
                return Ok(result);
            
        }
    }
}
