using Core.Helpers.Services;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authenticationService,
        ILogger<AuthenticationController> logger)
    {
        _authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterCustomer(CustomerForRegistration customerForRegistration)
    {
        try
        {
            var result = await _authenticationService.RegisterCustomer(customerForRegistration);
            if (result.Succeeded) return StatusCode(StatusCodes.Status201Created);
            foreach (var error in result.Errors) ModelState.TryAddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not register new customer",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterEmployee(EmployeeForRegistration employeeForRegistration)
    {
        try
        {
            var result = await _authenticationService.RegisterEmployee(employeeForRegistration);
            if (result.Succeeded) return StatusCode(StatusCodes.Status201Created);
            foreach (var error in result.Errors) ModelState.TryAddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not register new employee",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(AuthenticationDto authenticationDto)
    {
        try
        {
            if (!await _authenticationService.ValidateUser(authenticationDto)) return Unauthorized();
            var tokenDto = await _authenticationService.CreateToken(populateExp: true);
            return Ok(tokenDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Login fail", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }
}