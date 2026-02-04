using System.Text;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Application.UseCases.Users;
using PortfolioApi.Infrastructure.Data;
using PortfolioApi.Infrastructure.Repository.Implementations;
using PortfolioApi.Infrastructure.Repository.Interfaces;
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
using Microsoft.OpenApi.Models;


using Microsoft.AspNetCore.Identity;

using PortfolioApi.Domain.Entities; // sua entidade User



var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
    
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
                origin == "https://stpimenta.com" ||
                origin == "https://www.stpimenta.com" ||
                origin == "https://portadmin.stpimenta.com" ||
                origin == "https://portawsapi.stpimenta.com" ||
                origin == "https://gurdiano.com" ||
                origin == "https://www.gurdiano.com" ||
                origin == "https://portadmin.gurdiano.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//database
builder.Services.AddDbContext<AppDbContext>(
    options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("DevDb"));

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
builder.Services.AddScoped<GetProjectByIdUserUseCase>();
builder.Services.AddScoped<AddImageUseCase>();
builder.Services.AddScoped<DeleteImageUseCase>();

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
builder.Services.AddScoped<AmazonS3Service>();
builder.Services.AddHttpClient<GetAllProjectsUseCase>();

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

//aws
builder.Services.AddSingleton<Amazon.S3.IAmazonS3>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var accessKey = config["AWS:AccessKey"];
    var secretKey = config["AWS:SecretKey"];
    var regionName = config["AWS:Region"];

    var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
    var region = Amazon.RegionEndpoint.GetBySystemName(regionName);

    return new AmazonS3Client(credentials, region);
} );



builder.Services.AddControllers();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Campo para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Segurança global
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // Apenas a referência
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();
app.UseCors("AllowFrontend");



//migrations
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     db.Database.Migrate();
// }

//middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API v1");

  
});

app.MapControllers();

app.Run();

