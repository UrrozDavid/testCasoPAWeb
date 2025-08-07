using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PAWCP2.Models.DTOs
{
    public class UserRoleDto
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }
        [JsonPropertyName("roleName")]
        public string? RoleName { get; set; }
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
    }
}
