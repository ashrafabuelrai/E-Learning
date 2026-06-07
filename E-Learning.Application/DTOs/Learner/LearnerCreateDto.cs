using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_Learning.Application.DTOs.Learner
{
    public class LearnerCreateDto
    {
        [Required(ErrorMessage = "FullName is required.")]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be valid.")]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "NationalId is required.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "NationalId must be exactly 14 characters.")]
        public string NationalId { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Department { get; set; }
    }
}
