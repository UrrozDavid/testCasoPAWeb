using APW.Architecture;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Manager;
using PAWCP2.Core.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Models.Entities;
using PAWCP2.Mvc.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("api");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddSession();


builder.Services.AddScoped<IRestProvider, PAWCP2.Mvc.Service.AuthRestProvider>();

// ****************************************************************************//
// User
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IManagerUser, ManagerUser>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserRolesService>();

//Roles
builder.Services.AddScoped<IRepositoryRole, RepositoryRole>();
builder.Services.AddScoped<IManagerRole, ManagerRole>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<RoleService>();

//userRoles
builder.Services.AddScoped<IRepositoryUserRole, RepositoryUserRole>();
builder.Services.AddScoped<IManagerUserRole, ManagerUserRole>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();

builder.Services.AddDbContext<FoodbankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ****************************************************************************//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
