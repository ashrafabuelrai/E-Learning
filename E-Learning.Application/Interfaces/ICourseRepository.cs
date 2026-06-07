using E_Learning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Application.Interfaces
{
    public interface ICourseRepository:IRepository<Course>
    {
        Task<Course> Update(Course obj);
    }
}
