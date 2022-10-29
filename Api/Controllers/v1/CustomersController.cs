using Core.Entities.DataTransferObjects;
using Core.Helpers.Services;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerRepository customerRepository, ILogger<CustomersController> logger)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> Get()
    {
        try
        {
            var customers = await _customerRepository.GetCustomersAsync();
            if (!customers.Any()) return NoContent();
            return Ok(customers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Error in get customers", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDto>> Get(string customerId)
    {
        try
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound($"No customer found with id: {customerId}");
            return Ok(customer);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Error in get customer by id", StatusCodes.Status400BadRequest,
                e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Post(CustomerDto customerDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var customer = await _customerRepository.CreateCustomerAsync(customerDto);
            return Ok(customer);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not create new customer",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPut("{customerId}")]
    public async Task<ActionResult<CustomerDto>> Put(string customerId, CustomerDto customerDto)
    {
        try
        {
            if (!customerId.Equals(customerDto.Id)) ErrorService.IdError(customerId);
            var customer = await _customerRepository.UpdateCustomerAsync(customerDto);
            return Ok(customer);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Could not update customer", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult<bool>> Delete(string customerId)
    {
        try
        {
            var checkDelete = await _customerRepository.DeleteCustomerByCustomerId(customerId);
            if (!checkDelete) throw new Exception();
            return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not delete customer", StatusCodes.Status400BadRequest,
                e.Message);
            return BadRequest(errorResponse);
        }
    }
}