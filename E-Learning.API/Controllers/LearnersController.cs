using E_Learning.Application.DTOs.Learner;
using E_Learning.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnersController : ControllerBase
    {
        private readonly ILearnerService _learnerService;

        public LearnersController(ILearnerService learnerService) => _learnerService = learnerService;

        
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 0, int page = 1)
        {
            try
            {
                var result = await _learnerService.GetAllAsync(pageSize, page);
                if (!result.IsSuccess)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, ErrorMessage = "An error occurred while processing your request.", Details = ex.Message });
            }
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _learnerService.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            { 
                 return StatusCode(500, new { IsSuccess = false, ErrorMessage = "An error occurred while processing your request.", Details = ex.Message });
            }

        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LearnerCreateDto dto)
        {
            try
            {
                var result = await _learnerService.CreateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                  return StatusCode(500, new { IsSuccess = false, ErrorMessage = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
    }
}
