using E_Learning.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Learning.Domain.Entities
{
    public class UserContext
    {
        public Guid UserId { get; set; }
        public UserRole Role { get; set; } 
    }
}
