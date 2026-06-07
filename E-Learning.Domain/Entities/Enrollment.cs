using E_Learning.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Domain.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Guid LearnerId { get; set; }

        public Guid CourseId { get; set; }

        public EnrollmentStatus Status { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string? DecisionReason { get; set; }

        public Learner Learner { get; set; }

        public Course Course { get; set; }
    }
}
