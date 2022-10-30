﻿using Core.Entities.Models;
using Core.Entities.Responses;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly UserManager<User> _userManager;

    public AuthenticationController(IAuthenticationService authenticationService,
        ILogger<AuthenticationController> logger, UserManager<User> userManager)
    {
        _authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [AllowAnonymous]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [Authorize(Roles = "Administrator")]
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
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(AuthenticationDto authenticationDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(authenticationDto.Email);
            if (user == null) return Unauthorized(new AuthResponseDto {ErrorMessage = "Invalid Authentication"});

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new AuthResponseDto {ErrorMessage = "Email is not confirmed"});

            if (await _userManager.IsLockedOutAsync(user))
                return Unauthorized(new AuthResponseDto {ErrorMessage = "The Account is locked out"});

            if (!await _userManager.CheckPasswordAsync(user, authenticationDto.Password))
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                    // TODO send mail
                    return Unauthorized(new AuthResponseDto {ErrorMessage = "The Account is locked out"});

                return Unauthorized(new AuthResponseDto {ErrorMessage = "Invalid Authentication"});
            }

            var tokenDto = await _authenticationService.CreateToken(true, user);

            await _userManager.ResetAccessFailedCountAsync(user);

            var authResponse = new AuthResponseDto
            {
                RefreshToken = tokenDto.RefreshToken,
                AccessToken = tokenDto.AccessToken,
                IsAuthSuccessful = true
            };
            return Ok(authResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }
}