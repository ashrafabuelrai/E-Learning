using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationHours { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool RequiresApproval { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Enrollment> Enrollments { get; set; } 
    }
}
