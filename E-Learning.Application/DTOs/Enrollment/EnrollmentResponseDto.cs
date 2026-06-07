using E_Learning.Application.DTOs.Course;
using E_Learning.Application.DTOs.Learner;
using E_Learning.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.DTOs.Enrollment
{
    public class EnrollmentResponseDto
    {
        public Guid Id { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? DecisionReason { get; set; }

        public LearnerResponseDto Learner { get; set; } 
        public CourseResponseDto Course { get; set; }
    }
}
