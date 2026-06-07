using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Learning.Application.DTOs.Enrollment
{
    public class EnrollmentCreateDto
    {
        [Required]
        public Guid LearnerId { get; set; }

        [Required]
        public Guid CourseId { get; set; }
    }
}
