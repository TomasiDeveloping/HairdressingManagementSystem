using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterCustomer(CustomerForRegistration customerForRegistration);

    Task<IdentityResult> RegisterEmployee(EmployeeForRegistration employeeForRegistration);

    Task<bool> ValidateUser(AuthenticationDto authenticationDto);

    Task<string> CreateToken();
}