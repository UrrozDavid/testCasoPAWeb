using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PAWCP2.Models.Entities;
using PAWCP2.Mvc.Models;
using PAWCP2.Mvc.Service;
using PAWCP2.Models.DTOs;

namespace PAWCP2.Mvc.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly UserRolesService _userRolesService;
        private readonly IUserService _userService;
        private readonly IUserRolesService _iuserRolesService;
        private readonly RoleService _roleService;

        public UserRoleController(
            UserRolesService userRolesService,
            IUserRolesService iuserRolesService,
            IUserService userService,
            RoleService role)
        {
            _userRolesService = userRolesService;
            _iuserRolesService = iuserRolesService;
            _userService = userService;
            _roleService = role;
        }

        #region Index
        // GET: UserRole
        public async Task<IActionResult> Index()
        {
            var currentUser = HttpContext.Session.GetString("User")
                              ?? User.Identity?.Name;

            var userRoles = await _userRolesService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(currentUser))
                userRoles = userRoles.Where(ur => !string.Equals(ur.UserName, currentUser, StringComparison.OrdinalIgnoreCase))
                            .ToList();

            var viewModel = new UserRoleViewModel
            {
                UserRoles = userRoles,
                Roles = await _roleService.GetAllAsync()
            };

            return View(viewModel);
        }
        #endregion

        #region Create
        // GET: Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User model)
        {
            if (!ModelState.IsValid) return View(model);

            model.IsActive = true;
            model.CreatedAt = DateTime.Now;
            model.LastLogin = null;

            var userId = await _userService.CreateAsync(model);

            if (userId is null || userId <= 0 )
            {
                ModelState.AddModelError("", "Unable to create the user");
                return View(model);
            }

            // Crear Usuario con Role 3 (Viewer)
            var result = await _userRolesService.SaveAsync(new []
            {
                new UserRole
                {
                    UserId = userId ?? 0,
                    RoleId = 3,
                    Description = null
                }
            });

            return RedirectToAction(nameof(Index));

        }

        // POST: Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save (int userId, int roleId)
        {
            var result = await _userRolesService.SaveAsync(new[]
            {
                new UserRole { UserId = userId, RoleId = roleId, Description = null }
            });

            if (result) return RedirectToAction(nameof(Index));

            return View(result);
        }
        
        #endregion
    }
}
