
using Mediator.Application.DTOs;
using Mediator.Domain.Entities;

namespace Mediator.Application.Mappers
{
    public static class EntityMapper
    {
        public static IEnumerable<CustomerResponse> Map(IEnumerable<Customer> customer)
        {
            List<CustomerResponse> customers = new List<CustomerResponse>();

            foreach (var c in customer)
            {
                var customerResponse = new CustomerResponse
                {
                    Email = c.Email,
                    FirstName = c.FirstName,
                    Id = c.Id,
                    LastName = c.LastName,
                    CreatedDate = c.CreatedDate,
                    IsActive = c.IsActive,
                };
                customers.Add(customerResponse);
            }

            return customers;
        }


        public static CustomerResponse Map(Customer customer)
        {
            var customerResponse = new CustomerResponse
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                Id = customer.Id,
                LastName = customer.LastName,
                CreatedDate = customer.CreatedDate,
                IsActive = customer.IsActive,
            };

            return customerResponse;
        }

        public static Customer Map(CustomerRequest customerRequest)
        {
            var customer = new Customer
            {
                Email = customerRequest.Email,
                FirstName = customerRequest.FirstName,
                Id = customerRequest.Id,
                LastName = customerRequest.LastName
            };

            return customer;
        }


        public static Customer Map(CustomerRequest customerRequest, Customer customer, int customerId)
        {
            customer.Email = customerRequest.Email;
            customer.FirstName = customerRequest.FirstName;
            customer.Id = customerId;
            customer.LastName = customerRequest.LastName;
            return customer;
        }
    }
}
