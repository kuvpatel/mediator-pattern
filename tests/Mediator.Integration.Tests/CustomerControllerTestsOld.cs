//using FluentAssertions;
//using System.Net;
//using System.Net.Http.Json;


//namespace Mediator.IntegrationTests
//{
//    public class CustomerControllerTests :
//    IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;

//        public CustomerControllerTests(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateClient();
//        }

//        [Fact]
//        public async Task GetWeather_ShouldReturnSuccess()
//        {
//            // Arrange
//            var id = 1;

//            // Act
//            var response = await _client.GetAsync($"/api/customer/{id}");

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//        } 

//        [Fact]
//        public async Task CreateCustomer_ShouldReturnCreated()
//        {
//            var customer = new
//            {
//                firstName = "John",
//                lastName = "Smith",
//                email = "john.smith@test.com"
//            };

//            var response = await _client.PostAsJsonAsync(
//                "/api/customer",
//                customer);

//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//        }

//        [Fact]
//        public async Task UpdateCustomer_ShouldReturnNoContent()
//        {
//            var customer = new
//            {
//                id = 1,
//                firstName = "Updated",
//                lastName = "User",
//                email = "updated@test.com"
//            };

//            var response =
//                await _client.PutAsJsonAsync("/api/customer/1", customer);

//            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }

//        [Fact]
//        public async Task DeleteCustomer_ShouldReturnNoContent()
//        {
//            var response =
//                await _client.DeleteAsync("/api/customer/1");

//            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
//        }
//    }
//}
