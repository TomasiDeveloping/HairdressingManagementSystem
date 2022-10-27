using Core.Helpers.Services;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<TokenController> _logger;

    public TokenController(IAuthenticationService authenticationService, ILogger<TokenController> logger)
    {
        _authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Refresh(TokenDto tokenDto)
    {
        try
        {
            var tokenDtoToReturn = await _authenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not refresh token", StatusCodes.Status400BadRequest,
                e.Message);
            return BadRequest(errorResponse);
        }
    }
}