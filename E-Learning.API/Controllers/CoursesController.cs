using E_Learning.Application.DTOs.Course;
using E_Learning.Application.Services.Interface;
using E_Learning.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService) => _courseService = courseService;

        private UserContext GetUserContext() =>
            HttpContext.Items["UserContext"] as UserContext ?? new UserContext();

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 0, int page = 1)
        {
            try
            {
                var result = await _courseService.GetAllAsync(pageSize, page);
                if(!result.IsSuccess)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving courses.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if(!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
        {
            try
            {
                var result = await _courseService.CreateAsync(dto, GetUserContext());
                if (!result.IsSuccess)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the course.", Details = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseUpdateDto dto)
        {
            try
            {
                var result = await _courseService.UpdateAsync(id, dto, GetUserContext());
                if (!result.IsSuccess)
                     return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the course.", Details = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _courseService.DeleteAsync(id, GetUserContext());
                if(!result.IsSuccess)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the course.", Details = ex.Message });
            }
        }
    }
}
