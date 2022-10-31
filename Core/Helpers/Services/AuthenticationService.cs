using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
    private readonly IConfiguration _configuration;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;


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

    public async Task<IdentityResult> RegisterCustomer(CustomerForRegistrationDto customerForRegistrationDto)
    {
        var result = new IdentityResult();
        var user = _mapper.Map<User>(customerForRegistrationDto);
        var customer = _mapper.Map<CustomerDto>(customerForRegistrationDto);
        customer.UserId = user.Id;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            result = await _userManager.CreateAsync(user, customerForRegistrationDto.Password);
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

    public async Task<IdentityResult> RegisterEmployee(EmployeeForRegistrationDto employeeForRegistrationDto)
    {
        var result = new IdentityResult();
        var user = _mapper.Map<User>(employeeForRegistrationDto);
        user.EmailConfirmed = true;
        var employee = _mapper.Map<EmployeeDto>(employeeForRegistrationDto);
        employee.UserId = user.Id;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            result = await _userManager.CreateAsync(user, employeeForRegistrationDto.Password);
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

    public async Task<TokenDto> CreateToken(bool populateExp, User user)
    {
        var signInCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signInCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        if (populateExp) user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto
        {
            RefreshToken = refreshToken,
            AccessToken = accessToken
        };
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken!);

        var user = await _userManager.FindByNameAsync(principal.Identity!.Name);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new Exception("Invalid client request. The tokenDto has some invalid values.");

        return await CreateToken(false, user);
    }

    #region Private Methods

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName)
        };
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            _configuration["JwtSettings:validIssuer"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["secretKey"])),
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["validIssuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Invalid token");
        return principal;
    }

    #endregion
}