using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Models.DTOs
{
    public class UserRoleCreateDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string? Description { get; set; }
    }
}
