using Core.Entities.DataTransferObjects;
using Core.Helpers.Services;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}[controller]")]
[ApiVersion("1.0")]
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
    public async Task<ActionResult<AddressDto>> Get(string addressId)
    {
        try
        {
            var address = await _addressRepository.GetAddressByIdAsync(addressId);
            if (address == null) return NotFound($"No address found with id: {addressId}");
            return Ok(address);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Error in get address by id", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> Post(AddressDto addressDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var address = await _addressRepository.CreateAddressAsync(addressDto);
            return Ok(address);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not create new address",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult<AddressDto>> Put(string addressId, AddressDto addressDto)
    {
        try
        {
            if (!addressId.Equals(addressDto.Id)) ErrorService.IdError(addressId);
                var address = await _addressRepository.UpdateAddressAsync(addressDto);
            return Ok(address);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Could not update address", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult<bool>> Delete(string addressId)
    {
        try
        {
            var checkDelete = await _addressRepository.DeleteAddressByAddressIdAsync(addressId);
            if (!checkDelete) throw new Exception();
            return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Could not delete address", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }
}