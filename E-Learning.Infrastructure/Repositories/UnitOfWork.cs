using E_Learning.Application.Interfaces;
using E_Learning.Domain.Entities;
using E_Learning.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public ICourseRepository Courses {  get; private set; }

        public ILearnerRepository Learners { get; private set; }

        public IEnrollmentRepository Enrollments { get; private set; }
        public IAuditLogRepository AuditLogs { get; private set; }
        public UnitOfWork(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            Courses = new CourseRepository(_db);
            Learners = new LearnerRepository(_db);
            Enrollments = new EnrollmentRepository(_db);
            AuditLogs = new AuditLogRepository(_db);
       
        }
        public async Task<int> SaveChangesAsync()
        {
           return  await _db.SaveChangesAsync();
        }
    }
}
