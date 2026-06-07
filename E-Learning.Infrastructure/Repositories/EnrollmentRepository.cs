using E_Learning.Application.Interfaces;
using E_Learning.Domain.Entities;
using E_Learning.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Learning.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _db;
        public EnrollmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Enrollment> Update(Enrollment obj)
        {
            _db.Enrollments.Update(obj);
            return obj;
        }
    }
}
