using Shouldly;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Elders.Iso3166;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ViajeHonesto.Destinations;

[Collection("NoParallelization")]
public abstract class GeoDbCitySearchService_Integration_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ICitySearchService _citySearchService;

    protected GeoDbCitySearchService_Integration_Tests()
    {
        _citySearchService = GetRequiredService<ICitySearchService>();

       Task.Delay(1500).GetAwaiter().GetResult();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Return_Real_Results()
    {
        // ACT
        var request = new CitySearchRequestDto { PartialCityName = "Concepción" };
        var result = await _citySearchService.SearchCitiesByNameAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBeGreaterThan(0);
        result.CityNames.ShouldContain(c => c.Name.Contains("Concepción", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByNameAsync_Should_Map_Fields_Correctly()
    {
        // ACT
        var request = new CitySearchRequestDto { PartialCityName = "Paraná" };
        var result = await _citySearchService.SearchCitiesByNameAsync(request);

        // ASSERT
        result.CityNames.ShouldNotBeEmpty();

        var first = result.CityNames[0];
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
        var result = await _citySearchService.SearchCityDetailsAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.Name.ShouldBe("New York City");
        result.IsSaved.ShouldBeFalse();
        result.LocalId.ShouldBeNull();
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
    public async Task SearchCityDetailsAsync_Should_Fail_If_It_Doesnt_Find_City()
    {
        // Arrange
        var request = new CityDetailsSearchRequestDto { WikiDataId = "Q1" };

        // Act & Assert
        await Should.ThrowAsync<UserFriendlyException>(async () =>
        {
            await _citySearchService.SearchCityDetailsAsync(request);
        });
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByRegionAsync_Should_Fail_If_RegionCode_Is_Missing()
    {
        // Arrange
        var request = new CitySearchRequestDto { CountryCode = "AR" };

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _citySearchService.SearchCitiesByRegionAsync(request);
        });

        exception.Message.ShouldContain("RegionCode");
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByRegionAsync_Should_Fail_If_CountryCode_Is_Missing()
    {
        // Arrange
        var request = new CitySearchRequestDto { RegionCode = "E" };

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _citySearchService.SearchCitiesByRegionAsync(request);
        });

        // Assert
        exception.Message.ShouldContain("CountryCode");
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByRegionAsync_Should_Return_Real_Results()
    {
        // Arrange
        var request = new CitySearchRequestDto
        {
            CountryCode = "US",
            RegionCode = "NY"
        };

        // Act
        var result = await _citySearchService.SearchCitiesByRegionAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBeGreaterThan(0);
        result.CityNames.ShouldNotBeEmpty();
        result.CityNames.ShouldAllBe(c => c.Country.Contains("United States") && c.Region.Contains("New York"));
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByRegionAsync_Should_Handle_Network_Error_Gracefully()
    {
        // Arrange
        var brokenHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.invalid-geo-db.example.com/")
        };

        var service = new GeoDbCitySearchService(
            new GeoDbApiClient(brokenHttpClient)
        );

        // Act & Assert
        await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            await service.SearchCitiesByRegionAsync(
                new CitySearchRequestDto { CountryCode = "US", RegionCode = "NY" }
            );
        });
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task SearchCitiesByRegionAsync_Should_Handle_Unexpected_Response()
    {
        // Arrange
        var fakeClient = new HttpClient(new BrokenHandler());
        var service = new GeoDbCitySearchService(new GeoDbApiClient(fakeClient));

        // Act & Assert
        await Should.ThrowAsync<Exception>(async () =>
        {
            await service.SearchCitiesByRegionAsync(
                new CitySearchRequestDto { CountryCode = "US", RegionCode = "NY" }
            );
        });
    }
}
