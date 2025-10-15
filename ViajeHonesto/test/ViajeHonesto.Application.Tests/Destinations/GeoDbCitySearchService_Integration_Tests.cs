using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace ViajeHonesto.Destinations;
public abstract class GeoDbCitySearchService_Integration_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IDestinationAppService _destinationAppService;

    protected GeoDbCitySearchService_Integration_Tests()
    {
        _destinationAppService = GetRequiredService<IDestinationAppService>();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Return_Real_Results()
    {
        // ACT
        var request = new CitySearchRequestDto { PartialCityName = "Concepción" };
        var result = await _destinationAppService.SearchCitiesByNameAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.CityNames.Count.ShouldBeGreaterThan(0);
        result.CityNames.ShouldContain(c => c.Name.Contains("Concepción", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Map_Fields_Correctly()
    {
        // ACT
        var request = new CitySearchRequestDto { PartialCityName = "Concepción" };
        var result = await _destinationAppService.SearchCitiesByNameAsync(request);

        // ASSERT
        result.CityNames.ShouldNotBeEmpty();

        var first = result.CityNames[0];
        first.Name.ShouldNotBeNullOrWhiteSpace();
        first.Country.ShouldNotBeNullOrWhiteSpace();
    }
    /*
    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Handle_Network_Error_Gracefully()
    {
        // Simulamos fallo cambiando el host (si lo permite tu implementación)
        var brokenHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.invalid-geo-db.example.com/")
        };

        // Si tu servicio real acepta HttpClient por inyección:
        var service = new GeoDbCitySearchService(
            new GeoDbApiClient(brokenHttpClient)
        );

        await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            await service.SearchCitiesByNameAsync(
                new CitySearchRequestDto { PartialCityName = "London" }
            );
        });
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Handle_Unexpected_Response()
    {
        // Si querés probar cómo responde ante un JSON inválido, hacelo con un fake client
        var fakeClient = new HttpClient(new BrokenHandler()); // handler que devuelve 200 pero body inválido
        var service = new GeoDbCitySearchService(new GeoDbApiClient(fakeClient));

        await Should.ThrowAsync<Exception>(async () =>
        {
            await service.SearchCitiesByNameAsync(
                new CitySearchRequestDto { PartialCityName = "Rome" }
            );
        });
    }

    private class BrokenHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var msg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("{ invalid json here }")
            };
            return Task.FromResult(msg);
        }
    }
    */
}
