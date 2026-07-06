using Mediator.Application.DTOs;
using Mediator.Application.Features.Customers.Commands.CreateCustomer;
using Mediator.Application.Features.Customers.Commands.UpdateCustomer;
using Mediator.Application.Features.Customers.Queries.GetCustomerById;
using Mediator.Application.Features.Customers.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace MediateRApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _mediator.Send(new GetCustomersQuery() { });

            if (customers == null)
                return NotFound();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customerDetails = await _mediator.Send(new GetCustomerQuery() { CustomerId = id });

            if (customerDetails == null)
                return NotFound();

            return Ok(customerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerRequest customer)
        {
            var customerResponse = await _mediator.Send(new CreateCustomerCommand() { Customer = customer });

            if (customerResponse == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create customer.");
            }

            return CreatedAtAction(nameof(Get), new { id = customerResponse.Id }, customerResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CustomerRequest customer)
        {
            if (id != customer.Id)
                return BadRequest();

            var customerResponse = await _mediator.Send(new UpdateCustomerCommand() { CustomerId = id, Customer = customer });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCustomerCommand() { CustomerId = id });

            return NoContent();
        }
    }
}
