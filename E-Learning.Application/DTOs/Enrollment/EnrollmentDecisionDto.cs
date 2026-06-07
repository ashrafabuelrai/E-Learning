using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Learning.Application.DTOs.Enrollment
{
    public class EnrollmentDecisionDto
    {
        [Required(ErrorMessage = "Decision is required.")]
        public string Decision { get; set; } = string.Empty;

        public string? Reason { get; set; }
    }
}
