using System.Text;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Application.UseCases.Users;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PortfolioApi.Application.Services;
using PortfolioApi.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PortfolioApi.Application.UseCases.Icons;
using PortfolioApi.Application.UseCases.Projects;
using PortfolioApi.Application.UseCases.Roles;
using PortfolioApi.Application.UseCases.Technologies;
using PortfolioApi.Application.UseCases.UserRoleProgress;
using PortfolioApi.Application.UseCases.UserTechnologyProgress;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    // Opcional: HTTPS
    // options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps());
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
builder.Services.AddControllers();

//injetando o mappgind dto
builder.Services.AddAutoMapper(typeof(MappingProfile));

//user cases and Repositories
// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<AddUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();

// Icons
builder.Services.AddScoped<IIconRepository, IconRepository>();
builder.Services.AddScoped<GetAllIconsUseCase>();
builder.Services.AddScoped<GetIconByIdUseCase>();
builder.Services.AddScoped<AddIconUseCase>();
builder.Services.AddScoped<UpdateIconUseCase>();
builder.Services.AddScoped<DeleteIconUseCase>();

// Technology
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();
builder.Services.AddScoped<GetAllTechnologiesUseCase>();
builder.Services.AddScoped<GetTechnologyByIdUseCase>();
builder.Services.AddScoped<AddTechnologyUseCase>();
builder.Services.AddScoped<UpdateTechnologyUseCase>();
builder.Services.AddScoped<DeleteTechnologyUseCase>();

// Project
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<GetAllProjectsUseCase>();
builder.Services.AddScoped<GetProjectByIdUseCase>();
builder.Services.AddScoped<AddProjectUseCase>();
builder.Services.AddScoped<UpdateProjectUseCase>();
builder.Services.AddScoped<DeleteProjectUseCase>();

// Roles
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<GetAllRolesUseCase>();
builder.Services.AddScoped<GetRoleByIdUseCase>();
builder.Services.AddScoped<CreateRoleUseCase>();
builder.Services.AddScoped<UpdateRoleUseCase>();
builder.Services.AddScoped<DeleteRoleUseCase>();

// UserRoleProgress
builder.Services.AddScoped<IUserRoleProgressRepository, UserRoleProgressRepository>();
builder.Services.AddScoped<AddUserRoleProgressUseCase>();
builder.Services.AddScoped<UpdateUserRoleProgressUseCase>();
builder.Services.AddScoped<DeleteUserRoleProgressUseCase>();
builder.Services.AddScoped<GetUserRoleProgressByUserUseCase>();

// UserTechnologyProgress
builder.Services.AddScoped<IUserTechnologyRepository, UserTechnologyRepository>();
builder.Services.AddScoped<AddUserTechnologyUseCase>();
builder.Services.AddScoped<UpdateUserTechnologyUseCase>();
builder.Services.AddScoped<DeleteUserTechnologyUseCase>();
builder.Services.AddScoped<GetUserTechnologyUseCase>();



//services
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<JwtService>();

//jwt
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            )
        };
    });

var app = builder.Build();

//middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

