using Shouldly;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace ViajeHonesto.Destinations;

[Collection("NoParallelization")]
public abstract class GeoDbCitySearchService_Integration_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IDestinationAppService _destinationAppService;

    protected GeoDbCitySearchService_Integration_Tests()
    {
        _destinationAppService = GetRequiredService<IDestinationAppService>();

        Task.Delay(1500).GetAwaiter().GetResult();
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
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(c => c.Name.Contains("Concepción", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Map_Fields_Correctly()
    {
        // ACT
        var request = new CitySearchRequestDto { PartialCityName = "Paraná" };
        var result = await _destinationAppService.SearchCitiesByNameAsync(request);

        // ASSERT
        result.Items.ShouldNotBeEmpty();

        var first = result.Items[0];
        first.Name.ShouldNotBeNullOrWhiteSpace();
        first.Country.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Handle_Network_Error_Gracefully()
    {
        // Simulamos fallo cambiando el host
        var brokenHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.invalid-geo-db.example.com/")
        };

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

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCityDetailsAsync_Should_Return_Real_Results_From_API()
    {
        // ACT
        var request = new CityDetailsSearchRequestDto { WikiDataId = "Q60" };
        var result = await _destinationAppService.SearchCityDetailsAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.Name.ShouldBe("New York City");
        result.IsSaved.ShouldBeFalse();
        result.LocalId.ShouldBeNull();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCityDetailsAsync_Should_Return_Results_From_Local_DB_First()
    {
        // ACT
        var request = new CityDetailsSearchRequestDto { WikiDataId = "Q727464" };
        var result = await _destinationAppService.SearchCityDetailsAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Concepción del Uruguay");
        result.IsSaved.ShouldBeTrue();
        result.LocalId.ShouldNotBeNull();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCityDetailsAsync_Should_Handle_Network_Error_Gracefully()
    {
        // Simulamos fallo cambiando el host
        var brokenHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.invalid-geo-db.example.com/")
        };

        var service = new GeoDbCitySearchService(
            new GeoDbApiClient(brokenHttpClient)
        );

        await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            await service.SearchCityDetailsAsync(
                new CityDetailsSearchRequestDto { WikiDataId = "Q60" }
            );
        });
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCityDetailsAsync_Should_Return_Results_From_Local_DB_If_API_Is_Down()
    {
        // Arrange
        // Simulamos fallo cambiando el host
        var brokenHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.invalid-geo-db.example.com/")
        };

        var service = new GeoDbCitySearchService(
            new GeoDbApiClient(brokenHttpClient)
        );

        // ACT
        var request = new CityDetailsSearchRequestDto { WikiDataId = "Q727464" };
        var result = await _destinationAppService.SearchCityDetailsAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Concepción del Uruguay");
        result.IsSaved.ShouldBeTrue();
        result.LocalId.ShouldNotBeNull();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCityDetailsAsync_Should_Fail_If_It_Doesnt_Find_City()
    {
        // Arrange
        // Simulamos fallo cambiando el host
        var brokenHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.invalid-geo-db.example.com/")
        };

        var service = new GeoDbCitySearchService(
            new GeoDbApiClient(brokenHttpClient)
        );

        // ACT
        var request = new CityDetailsSearchRequestDto { WikiDataId = "Q1" };

        // Assert
        await Should.ThrowAsync<UserFriendlyException>(async () =>
        {
            await _destinationAppService.SearchCityDetailsAsync(request);
        });
    }
}
