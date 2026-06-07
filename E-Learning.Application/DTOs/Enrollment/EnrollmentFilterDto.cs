using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.DTOs.Enrollment
{
    public class EnrollmentFilterDto
    {
        public Guid? LearnerId { get; set; }
        public Guid? CourseId { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
