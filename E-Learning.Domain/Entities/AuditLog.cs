using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; } 
        public Guid EntityId { get; set; }
        public string Action { get; set; } 
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Guid PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public Guid EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; }
    }
}
