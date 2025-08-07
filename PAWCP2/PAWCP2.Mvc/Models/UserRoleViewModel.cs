using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.DTOs;
using PAWCP2.Models.Entities;

namespace PAWCP2.Mvc.Models
{
    public class UserRoleViewModel
    {
        public List<UserRoleDto> UserRoles { get; set; } = new();
        public List<Role> Roles { get; set; }
    }
}
