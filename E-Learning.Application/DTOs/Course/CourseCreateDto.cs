using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Learning.Application.DTOs.Course
{
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "DurationHours must be greater than 0.")]
        public int DurationHours { get; set; }

        public bool RequiresApproval { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
