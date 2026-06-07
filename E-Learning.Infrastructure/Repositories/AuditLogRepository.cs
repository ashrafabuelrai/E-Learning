using E_Learning.Application.Interfaces;
using E_Learning.Domain.Entities;
using E_Learning.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Infrastructure.Repositories
{
    public class AuditLogRepository :Repository<AuditLog> ,IAuditLogRepository
    {
        private readonly ApplicationDbContext _db;
        public AuditLogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
