using APW.Architecture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PAWCP2.API.Auth;
using PAWCP2.Core.Manager;
using PAWCP2.Core.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Mvc.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ----- Authentication : Basic -----
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", _ => { });
builder.Services.AddAuthorization();
//-----------------------------------

// ----- Swagger using Basic Auth -----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PAWCP2.API", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic auth: username & password"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme {Reference = new OpenApiReference
            { Type = ReferenceType.SecurityScheme, Id = "basic" }}, Array.Empty<string>() }
    });
});
//-----------------------------------


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
//fooditem
builder.Services.AddScoped<IRepositoryFoodItem, RepositoryFoodItem>();
builder.Services.AddScoped<IManagerFoodItem, ManagerFoodItem>();

// ****************************************************************************//


var allowedOrigin = "https://localhost:7063"; 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
    {
        policy.WithOrigins("https://localhost:7063", "https://localhost:7099") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors("AllowLocal");
app.MapControllers();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
