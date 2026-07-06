

namespace Mediator.Application.DTOs
{
    public class CustomerRequest
    {
        public int Id { get; set; }

        
        public string FirstName { get; set; } = string.Empty;

        
        public string LastName { get; set; } = string.Empty;

        
        public string Email { get; set; } = string.Empty;
    }
}
