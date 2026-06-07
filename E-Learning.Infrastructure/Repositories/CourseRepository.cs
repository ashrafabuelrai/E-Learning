using E_Learning.Application.Interfaces;
using E_Learning.Domain.Entities;
using E_Learning.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Learning.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseRepository(ApplicationDbContext db): base(db) 
        {
            _db = db;
        }
       
        public async Task<Course> Update(Course obj)
        {
            _db.Courses.Update(obj);
            return obj;
        }
    }
}
