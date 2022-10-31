using Core.Entities.Responses;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v:{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
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

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ActionResult<ApiOkResponse<TokenDto>>> Refresh(TokenDto tokenDto)
    {
        try
        {
            var tokenDtoToReturn = await _authenticationService.RefreshToken(tokenDto);
            return Ok(new ApiOkResponse<TokenDto>(tokenDtoToReturn));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }
}