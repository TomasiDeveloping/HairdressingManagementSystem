using Core.Entities.DataTransferObjects;
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
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> Post(AddressDto addressDto)
    {
        try
        {
            var address = await _addressRepository.CreateAddressAsync(addressDto);
            return Ok(address);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult<AddressDto>> Put(string addressId, AddressDto addressDto)
    {
        try
        {
            if (!addressId.Equals(addressDto.Id)) return BadRequest("Error in addressId");
            var address = await _addressRepository.UpdateAddressAsync(addressDto);
            return Ok(address);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult<bool>> Delete(string addressId)
    {
        try
        {
            var checkDelete = await _addressRepository.DeleteAddressByAddressIdAsync(addressId);
            if (!checkDelete) return BadRequest($"Could not delete address with id: {addressId}");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}