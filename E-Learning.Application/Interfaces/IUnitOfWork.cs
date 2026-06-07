using E_Learning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICourseRepository Courses { get; }

        ILearnerRepository Learners { get; }

        IEnrollmentRepository Enrollments { get; }

        IAuditLogRepository AuditLogs { get; }

        Task<int> SaveChangesAsync();
    }
}
