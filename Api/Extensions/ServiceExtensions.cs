﻿using System.Text;
using Core.Entities.Models;
using DataBase;
using DataBase.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Hairdressing Management v1",
                Version = "v1",
                Description = "Hairdressing Management API",
                Contact = new OpenApiContact
                {
                    Name = "Tomasi-Developing",
                    Email = "info@tomasi-developing.ch",
                    Url = new Uri("https://wwww.tomasi-developing.ch")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri(
                        "https://github.com/TomasiDeveloping/HairdressingManagementSystem/blob/master/LICENSE.txt")
                }
            });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    securitySchema, new[]
                        {"Bearer"}
                }
            };
            c.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfigurationSection jwtSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["validIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"]))
            };
        });
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(options =>
        {
            options.AddProfile<EmployeeProfile>();
            options.AddProfile<AddressProfile>();
            options.AddProfile<CustomerProfile>();
            options.AddProfile<AppointmentProfile>();
            options.AddProfile<CustomerForRegistrationProfile>();
            options.AddProfile<EmployeeForRegistrationProfile>();
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 7;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddEntityFrameworkStores<RepositoryContext>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("HairdressingManagement"));
        });
    }

    public static void ConfigureApiVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
    }
}