using Api.Extensions;
using Core.Helpers.Services;
using Core.Interfaces;
using DataBase.Repositories;
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

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.ConfigureSwagger();

    builder.Services.ConfigureAutoMapper();

    builder.Services.ConfigureIdentity();

    builder.Services.ConfigureDbContext(builder.Configuration);

    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    builder.Services.ConfigureAuthentication(jwtSettings);

    builder.Services.ConfigureApiVersion();

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
        app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hairdressing Management v1"); });
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Stopped program because of exception");
    throw;
}