using System.Net;
using BookShop.Cart.IntegrationTests.Common;

namespace BookShop.Cart.IntegrationTests;

internal sealed class CartApiTests : TestsBase
{
    [Test]
    public async Task GetTodos_ReturnsOk()
    {
        HttpClient client = Factory.CreateClient();

        HttpResponseMessage response = await client.PostAsync("/cart", null);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }
}
