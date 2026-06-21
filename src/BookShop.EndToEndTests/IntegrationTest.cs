using System.Net;

namespace BookShop.EndToEndTests;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
internal sealed class ApiTests(AppFixture fixture)
{
    [Test]
    public async Task GetWeatherForecast_ReturnsOk()
    {
        HttpClient client = fixture.CreateHttpClient("bookshop-webapi");

        HttpResponseMessage response = await client.PostAsync("/cart", null);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }
}
