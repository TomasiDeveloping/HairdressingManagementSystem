using System.Transactions;
using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Core.Helpers.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AuthenticationService(IMapper mapper, UserManager<User> userManager, ILogger<AuthenticationService> logger,
        ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<IdentityResult> RegisterCustomer(CustomerForRegistration customerForRegistration)
    {
        var result = new IdentityResult();
        var user = _mapper.Map<User>(customerForRegistration);
        var customer = _mapper.Map<CustomerDto>(customerForRegistration);
        customer.UserId = user.Id;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            result = await _userManager.CreateAsync(user, customerForRegistration.Password);
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

    public async Task<IdentityResult> RegisterEmployee(EmployeeForRegistration employeeForRegistration)
    {
        var result = new IdentityResult();
        var user = _mapper.Map<User>(employeeForRegistration);
        var employee = _mapper.Map<EmployeeDto>(employeeForRegistration);
        employee.UserId = user.Id;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            result = await _userManager.CreateAsync(user, employeeForRegistration.Password);
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
}