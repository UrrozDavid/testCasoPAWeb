using APW.Architecture;
using Microsoft.EntityFrameworkCore;
using PAWCP2.Core.Manager;
using PAWCP2.Core.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Mvc.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRestProvider, RestProvider>();

// ****************************************************************************//
// User
builder.Services.AddScoped<IRepositoryUser, RepositoryUser>();
builder.Services.AddScoped<IManagerUser, ManagerUser>();
//builder.Services.AddScoped<IUserService, UserService>();

//Roles
builder.Services.AddScoped<IRepositoryRole, RepositoryRole>();
builder.Services.AddScoped<IManagerRole, ManagerRole>();
builder.Services.AddScoped<IRoleService, RoleService>();
//builder.Services.AddScoped<RoleService>();

//userRoles
builder.Services.AddScoped<IRepositoryUserRole, RepositoryUserRole>();
builder.Services.AddScoped<IManagerUserRole, ManagerUserRole>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();
builder.Services.AddScoped<UserRolesService>();

builder.Services.AddDbContext<FoodbankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FoodbankContext")));
// ****************************************************************************//

 var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
