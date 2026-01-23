using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ViajeHonesto.Destinations;
using Volo.Abp;
using Xunit;

namespace ViajeHonesto.Destinations;
public class GeoDbCitySearchService_Tests
{
    private readonly IGeoDbApiClient _mockGeoDbApiClient;
    private readonly GeoDbCitySearchService _citySearchService;

    public GeoDbCitySearchService_Tests()
    {
        _mockGeoDbApiClient = Substitute.For<IGeoDbApiClient>();
        _citySearchService = new GeoDbCitySearchService(_mockGeoDbApiClient);
    }

    [Fact]
    public async Task SearchCitiesByNameAsync_Should_Handle_Deserialization_Correctly()
    {
        // ARRANGE
        var request = new CitySearchRequestDto
        {
            PartialCityName = "Concepción",
            ResultLimit = 5,
            SkipCount = 0
        };

        // Respuesta JSON simulada para que la lógica de deserialización funcione
        string mockJsonResponse = @"{
    ""links"": [{
            ""rel"": ""first"",
            ""href"": ""/v1/geo/cities?offset=0&limit=5&namePrefix=Concepción""
        }, {
            ""rel"": ""next"",
            ""href"": ""/v1/geo/cities?offset=5&limit=5&namePrefix=Concepción""
        }, {
            ""rel"": ""last"",
            ""href"": ""/v1/geo/cities?offset=130&limit=5&namePrefix=Concepción""
        }
    ],
    ""data"": [{
            ""id"": 1609,
            ""wikiDataId"": ""Q1016869"",
            ""type"": ""ADM2"",
            ""city"": ""Concepción"",
            ""name"": ""Concepción"",
            ""country"": ""Argentina"",
            ""countryCode"": ""AR"",
            ""region"": ""Corrientes Province"",
            ""regionCode"": ""W"",
            ""regionWdId"": ""Q44758"",
            ""latitude"": -28.3930294,
            ""longitude"": -57.8868493,
            ""population"": 4022
        }, {
            ""id"": 3389324,
            ""wikiDataId"": ""Q5158358"",
            ""type"": ""CITY"",
            ""city"": ""Concepción"",
            ""name"": ""Concepción"",
            ""country"": ""Argentina"",
            ""countryCode"": ""AR"",
            ""region"": ""Catamarca Province"",
            ""regionCode"": ""K"",
            ""regionWdId"": ""Q44756"",
            ""latitude"": -28.68333333,
            ""longitude"": -66.06666667,
            ""population"": 0
        }, {
            ""id"": 3213321,
            ""wikiDataId"": ""Q1123806"",
            ""type"": ""ADM2"",
            ""city"": ""Concepción"",
            ""name"": ""Concepción"",
            ""country"": ""Argentina"",
            ""countryCode"": ""AR"",
            ""region"": ""Tucumán Province"",
            ""regionCode"": ""T"",
            ""regionWdId"": ""Q44829"",
            ""latitude"": -27.333333333,
            ""longitude"": -65.583333333,
            ""population"": 0
        }, {
            ""id"": 132302,
            ""wikiDataId"": ""Q728171"",
            ""type"": ""ADM2"",
            ""city"": ""Concepción Department"",
            ""name"": ""Concepción Department"",
            ""country"": ""Argentina"",
            ""countryCode"": ""AR"",
            ""region"": ""Misiones Province"",
            ""regionCode"": ""N"",
            ""regionWdId"": ""Q44798"",
            ""latitude"": -27.9863288,
            ""longitude"": -55.52531,
            ""population"": 10348
        }, {
            ""id"": 132307,
            ""wikiDataId"": ""Q868674"",
            ""type"": ""ADM2"",
            ""city"": ""Concepción Department"",
            ""name"": ""Concepción Department"",
            ""country"": ""Argentina"",
            ""countryCode"": ""AR"",
            ""region"": ""Corrientes Province"",
            ""regionCode"": ""W"",
            ""regionWdId"": ""Q44758"",
            ""latitude"": -28.33333,
            ""longitude"": -58.0,
            ""population"": 26282
        }
    ],
    ""metadata"": {
        ""currentOffset"": 0,
        ""totalCount"": 131
    }
}
";

        // Configurar el Mock para que devuelva el JSON.
        _mockGeoDbApiClient.SearchCitiesRawAsync(
            Arg.Any<string>(), // Usamos Arg.Any para ser flexibles
            Arg.Any<int>(),
            Arg.Any<int>())
            .Returns(mockJsonResponse);


        // ACT
        var result = await _citySearchService.SearchCitiesByNameAsync(request);


        // ASSERT

        // Verificar que la lógica interna de la clase funcionó correctamente con el JSON.
        result.ShouldNotBeNull();
        result.CityNames.Count.ShouldBe(5);
        result.CityNames[0].Name.ShouldBe("Concepción");
        result.CityNames[0].Country.ShouldBe("Argentina");

        result.CityNames[4].Name.ShouldBe("Concepción Department");
        result.CityNames[4].Country.ShouldBe("Argentina");
        // Verificar que la dependencia externa fue llamada.
        await _mockGeoDbApiClient.Received(1).SearchCitiesRawAsync(
            request.PartialCityName, request.ResultLimit, request.SkipCount
        );
    }
    [Fact]
    public async Task SearchCitiesByNameAsync_Should_Return_Empty_When_No_Results()
    {
        // ARRANGE
        var request = new CitySearchRequestDto { PartialCityName = "Xyz" };
        string mockJsonResponse = @"{ ""data"": [], ""metadata"": {""totalCount"": 0} }"; // sin resultados

        _mockGeoDbApiClient
            .SearchCitiesRawAsync(Arg.Is("Xyz"), Arg.Is(5), Arg.Is(0))
            .Returns(mockJsonResponse);

        // ACT
        var result = await _citySearchService.SearchCitiesByNameAsync(request);

        // ASSERT
        result.ShouldNotBeNull();
        result.CityNames.ShouldNotBeNull();
        result.CityNames.Count.ShouldBe(0);

        await _mockGeoDbApiClient.Received(1).SearchCitiesRawAsync("Xyz", 5, 0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SearchCitiesByNameAsync_Should_Throw_On_Invalid_Input(string? partial)
    {
        // ARRANGE
        var request = new CitySearchRequestDto { PartialCityName = partial };

        // ACT + ASSERT
        await Should.ThrowAsync<ArgumentException>(() => _citySearchService.SearchCitiesByNameAsync(request));

        await _mockGeoDbApiClient.DidNotReceiveWithAnyArgs().SearchCitiesRawAsync(default!, default, default);
    }

    [Fact]
    public async Task SearchCitiesByNameAsync_Should_Propagate_When_Api_Fails()
    {
        // ARRANGE
        var request = new CitySearchRequestDto { PartialCityName = "Roma" };

        _mockGeoDbApiClient
        .SearchCitiesRawAsync(Arg.Is("Roma"), Arg.Is(5), Arg.Is(0))
        .Returns(Task.FromException<string>(new HttpRequestException("Error fetching city data")));

        // ACT + ASSERT
        var ex = await Should.ThrowAsync<HttpRequestException>(() => _citySearchService.SearchCitiesByNameAsync(request));
        ex.Message.ShouldContain("Error");

        await _mockGeoDbApiClient.Received(1).SearchCitiesRawAsync("Roma", 5, 0);
    }

    [Fact]
    public async Task SearchCitiesByNameAsync_Should_Handle_Malformed_Json()
    {
        // ARRANGE
        var request = new CitySearchRequestDto { PartialCityName = "Tok" };
        string malformed = @"{ ""data"": [ { ""name"": ""Tokyo"", ""country"": ""Japan"" } "; // JSON roto

        _mockGeoDbApiClient
            .SearchCitiesRawAsync(Arg.Is("Tok"), Arg.Is(5), Arg.Is(0))
            .Returns(malformed);

        // ACT + ASSERT
        await Should.ThrowAsync<Exception>(() => _citySearchService.SearchCitiesByNameAsync(request));
    }

    [Fact]
    public async Task SearchCityDetailsAsync_Should_Handle_Deserialization_Correctly()
    {
        // ARRANGE
        var request = new CityDetailsSearchRequestDto
        {
            WikiDataId = "Q60"
        };

        // Respuesta JSON simulada para que la lógica de deserialización funcione
        string mockJsonResponse = @"{
  ""data"": {
    ""id"": 123214,
    ""wikiDataId"": ""Q60"",
    ""type"": ""CITY"",
    ""city"": ""New York City"",
    ""name"": ""New York City"",
    ""country"": ""United States of America"",
    ""countryCode"": ""US"",
    ""region"": ""New York"",
    ""regionCode"": ""NY"",
    ""elevationMeters"": 10,
    ""latitude"": 40.7,
    ""longitude"": -74,
    ""population"": 8804190,
    ""timezone"": ""America__New_York"",
    ""distance"": null,
    ""deleted"": false,
    ""placeType"": ""CITY""
  }
}
";

        // Configurar el Mock para que devuelva el JSON.
        _mockGeoDbApiClient.SearchCityDetailsRawAsync(
            Arg.Any<string>())
            .Returns(mockJsonResponse);

        // ACT
        var result = await _citySearchService.SearchCityDetailsAsync(request);


        // ASSERT

        // Verificar que la lógica interna de la clase funcionó correctamente con el JSON.
        result.ShouldNotBeNull();
        result.WikiDataId.ShouldBe("Q60");
        result.Name.ShouldBe("New York City");
        result.Country.ShouldBe("United States of America");
        result.Region.ShouldBe("New York");
        result.Population.ShouldBeGreaterThan(0);
        result.Coordinate.ShouldNotBeNull();
        result.Coordinate.Latitude.ShouldBe((float)40.7);
        result.Coordinate.Longitude.ShouldBe((float)-74);

        // Verificar que la dependencia externa fue llamada.
        await _mockGeoDbApiClient.Received(1).SearchCityDetailsRawAsync(
            request.WikiDataId
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SearchCityDetailsAsync_Should_Throw_When_WikiDataId_Is_NullOrEmpty(string? wikiDataId)
    {
        // ARRANGE
        var request = new CityDetailsSearchRequestDto { WikiDataId= wikiDataId };

        // ACT + ASSERT
        await Should.ThrowAsync<ArgumentException>(() => _citySearchService.SearchCityDetailsAsync(request));

        await _mockGeoDbApiClient.DidNotReceiveWithAnyArgs().SearchCityDetailsRawAsync(default!);
    }

    [Fact]
    public async Task SearchCityDetailsAsync_Should_Propagate_When_Api_Fails()
    {
        // ARRANGE
        var request = new CityDetailsSearchRequestDto { WikiDataId = "Q60" };

        _mockGeoDbApiClient
        .SearchCityDetailsRawAsync(Arg.Is(request.WikiDataId))
        .Returns(Task.FromException<string>(new HttpRequestException("Error fetching city data")));

        // ACT + ASSERT
        var ex = await Should.ThrowAsync<HttpRequestException>(() => _citySearchService.SearchCityDetailsAsync(request));
        ex.Message.ShouldContain("Error");

        await _mockGeoDbApiClient.Received(1).SearchCityDetailsRawAsync(request.WikiDataId);
    }
}
