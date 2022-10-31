using Core.Entities.Models;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterCustomer(CustomerForRegistrationDto customerForRegistrationDto);

    Task<IdentityResult> RegisterEmployee(EmployeeForRegistrationDto employeeForRegistrationDto);

    Task<TokenDto> CreateToken(bool populateExp, User user);

    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}