using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Domain.Entities
{
    public class Learner
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string? Department { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
