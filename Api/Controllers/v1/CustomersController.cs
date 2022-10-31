using Core.Entities.DataTransferObjects;
using Core.Entities.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
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
    public async Task<ActionResult<ApiOkResponse<List<CustomerDto>>>> Get()
    {
        try
        {
            var customers = await _customerRepository.GetCustomersAsync();
            return Ok(new ApiOkResponse<List<CustomerDto>>(customers));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<ApiOkResponse<CustomerDto>>> Get(string customerId)
    {
        try
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound(new ApiNotFoundResponse($"No customer found with id: {customerId}"));
            return Ok(new ApiOkResponse<CustomerDto>(customer));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiOkResponse<CustomerDto>>> Post(CustomerDto customerDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var customer = await _customerRepository.CreateCustomerAsync(customerDto);
            return Ok(new ApiOkResponse<CustomerDto>(customer));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut("{customerId}")]
    public async Task<ActionResult<ApiOkResponse<CustomerDto>>> Put(string customerId, CustomerDto customerDto)
    {
        try
        {
            if (!customerId.Equals(customerDto.Id))
                BadRequest(new ApiBadRequestResponse($"Id: {customerId} is not the same as in Object"));
            var customer = await _customerRepository.UpdateCustomerAsync(customerDto);
            return Ok(new ApiOkResponse<CustomerDto>(customer));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult<ApiOkResponse<bool>>> Delete(string customerId)
    {
        try
        {
            var checkDelete = await _customerRepository.DeleteCustomerByCustomerId(customerId);
            if (!checkDelete) return BadRequest(new ApiBadRequestResponse("Could not delete customer"));
            return Ok(new ApiOkResponse<bool>(checkDelete));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }
}