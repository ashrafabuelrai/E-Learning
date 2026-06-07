using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.DTOs.Course
{
    public class CourseResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationHours { get; set; }
        public bool RequiresApproval { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
