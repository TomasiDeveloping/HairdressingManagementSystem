using Core.Entities.Models;
using Core.Interfaces;
using DataBase;
using DataBase.Profiles;
using DataBase.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
});

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

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
