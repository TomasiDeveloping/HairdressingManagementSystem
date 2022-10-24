using Core.Entities.Models;
using Core.Helpers.Services;
using Core.Interfaces;
using DataBase;
using DataBase.Profiles;
using DataBase.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Application Starting Up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "Hairdressing Management", Version = "v1"});

        var securitySchema = new OpenApiSecurityScheme
        {
            Description = "JWT Auth Bearer Scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
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

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });


    builder.Services.AddDbContext<RepositoryContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("HairdressingManagement"));
    });

    builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 7;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RepositoryContext>();

    builder.Services.AddAutoMapper(options =>
    {
        options.AddProfile<EmployeeProfile>();
        options.AddProfile<AddressProfile>();
        options.AddProfile<CustomerProfile>();
        options.AddProfile<AppointmentProfile>();
        options.AddProfile<CustomerForRegistrationProfile>();
        options.AddProfile<EmployeeForRegistrationProfile>();
    });

    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IAddressRepository, AddressRepository>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


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
}
catch (Exception e)
{
    logger.Error(e, "Stopped program because of exception");
    throw;
}