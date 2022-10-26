using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Core.Helpers.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    private User? _user;

    public AuthenticationService(IMapper mapper, UserManager<User> userManager, ILogger<AuthenticationService> logger,
        ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IConfiguration configuration)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<IdentityResult> RegisterCustomer(CustomerForRegistration customerForRegistration)
    {
        var result = new IdentityResult();
        var user = _mapper.Map<User>(customerForRegistration);
        var customer = _mapper.Map<CustomerDto>(customerForRegistration);
        customer.UserId = user.Id;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            result = await _userManager.CreateAsync(user, customerForRegistration.Password);
            if (!result.Succeeded) throw new Exception();
            result = await _userManager.AddToRolesAsync(user, new[] {"Customer"});
            if (!result.Succeeded) throw new Exception();
            await _customerRepository.CreateCustomerAsync(customer);
            scope.Complete();
        }
        catch (Exception e)
        {
            scope.Dispose();
            _logger.LogError(e, e.Message);
            if (!result.Errors.Any())
                result = IdentityResult.Failed(new IdentityError
                {
                    Code = "Customer",
                    Description = "Could not create customer"
                });
        }

        return result;
    }

    public async Task<IdentityResult> RegisterEmployee(EmployeeForRegistration employeeForRegistration)
    {
        var result = new IdentityResult();
        var user = _mapper.Map<User>(employeeForRegistration);
        var employee = _mapper.Map<EmployeeDto>(employeeForRegistration);
        employee.UserId = user.Id;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            result = await _userManager.CreateAsync(user, employeeForRegistration.Password);
            if (!result.Succeeded) throw new Exception();
            result = await _userManager.AddToRolesAsync(user, new[] {"Employee"});
            if (!result.Succeeded) throw new Exception();
            await _employeeRepository.CreateEmployeeAsync(employee);
            scope.Complete();
        }
        catch (Exception e)
        {
            scope.Dispose();
            _logger.LogError(e, e.Message);
            if (!result.Errors.Any())
                result = IdentityResult.Failed(new IdentityError
                {
                    Code = "Employee",
                    Description = "Could not create employee"
                });
        }

        return result;
    }

    public async Task<bool> ValidateUser(AuthenticationDto authenticationDto)
    {
        _user = await _userManager.FindByEmailAsync(authenticationDto.Email);

        var result = _user != null && await _userManager.CheckPasswordAsync(_user, authenticationDto.Password);

        if (!result) _logger.LogWarning($"{nameof(ValidateUser)}: Authentication failed. Wrong email or password");
        return result;
    }

    public async Task<string> CreateToken()
    {
        var signInCredentials = GetSigningCredentials();
        var claims =  await GetClaims();
        var tokenOptions = GenerateTokenOptions(signInCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    #region Private Methods

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, _user!.Email)
        };
        var roles = await _userManager.GetRolesAsync(_user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:validIssuer"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
            );
        return tokenOptions;
    }

    #endregion
}