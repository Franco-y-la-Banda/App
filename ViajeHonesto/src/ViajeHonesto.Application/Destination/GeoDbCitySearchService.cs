using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ViajeHonesto.Destinations
{
    public class GeoDbCitySearchService : ICitySearchService, ITransientDependency
    {
        private readonly IGeoDbApiClient _geoDbApiClient;
        public GeoDbCitySearchService(IGeoDbApiClient geoDbApiClient)
        {
            _geoDbApiClient = geoDbApiClient;
        }

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            if (request.PartialCityName.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("El nombre parcial de la ciudad no puede estar vacío.");
            };

            var jsonRaw = await _geoDbApiClient.SearchCitiesRawAsync(request.PartialCityName, request.ResultLimit, request.SkipCount);
            var jsonDocument = JsonDocument.Parse(jsonRaw);
            var jsonCities = jsonDocument.RootElement.GetProperty("data");
            var jsonMetadata = jsonDocument.RootElement.GetProperty("metadata");

            var citySearchResultDto = new CitySearchResultDto();
            foreach (var city in jsonCities.EnumerateArray())
            {
                string? cityName = city.GetProperty("name").GetString();
                string? cityCountry = city.GetProperty("country").GetString();
                citySearchResultDto.CityNames.Add(
                    new CityDto
                    {
                        Name = cityName,
                        Country = cityCountry,
                    });
            }

            citySearchResultDto.TotalCount = jsonMetadata.GetProperty("totalCount").GetInt32();

            return citySearchResultDto;
        }
    }
}
