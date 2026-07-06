
namespace Mediator.IntegrationTests.Infrastructure
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class IntegrationTestCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
