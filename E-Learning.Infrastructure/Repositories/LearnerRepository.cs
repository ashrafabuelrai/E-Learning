using E_Learning.Application.Interfaces;
using E_Learning.Domain.Entities;
using E_Learning.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Learning.Infrastructure.Repositories
{
    public class LearnerRepository : Repository<Learner>, ILearnerRepository
    {
        private readonly ApplicationDbContext _db;
        public LearnerRepository(ApplicationDbContext db): base(db) 
        {
            _db = db;
        }
        public async Task<Learner> Update(Learner obj)
        {
            _db.Learners.Update(obj);
            return obj;
        }
    }
}
