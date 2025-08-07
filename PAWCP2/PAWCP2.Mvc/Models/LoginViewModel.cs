using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PAWCP2.Mvc.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
