using Core.Entities.DataTransferObjects;
using Core.Entities.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}[controller]")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly ILogger<AddressController> _logger;

    public AddressController(IAddressRepository addressRepository, ILogger<AddressController> logger)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{addressId}")]
    public async Task<ActionResult<ApiOkResponse<AddressDto>>> Get(string addressId)
    {
        try
        {
            var address = await _addressRepository.GetAddressByIdAsync(addressId);
            if (address == null)
                return NotFound(new ApiNotFoundResponse($"Address with id: {addressId} is not found."));
            return Ok(new ApiOkResponse<AddressDto>(address));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiOkResponse<AddressDto>>> Post(AddressDto addressDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var address = await _addressRepository.CreateAddressAsync(addressDto);
            return Ok(new ApiOkResponse<AddressDto>(address));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult<ApiOkResponse<AddressDto>>> Put(string addressId, AddressDto addressDto)
    {
        try
        {
            if (!addressId.Equals(addressDto.Id))
                return BadRequest(new ApiBadRequestResponse($"Id: {addressId} is not the same as in Object"));
            var address = await _addressRepository.UpdateAddressAsync(addressDto);
            return Ok(new ApiOkResponse<AddressDto>(address));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult<ApiOkResponse<bool>>> Delete(string addressId)
    {
        try
        {
            var checkDelete = await _addressRepository.DeleteAddressByAddressIdAsync(addressId);
            if (!checkDelete) throw new Exception();
            return Ok(new ApiOkResponse<bool>(checkDelete));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }
}