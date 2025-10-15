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
        private static readonly int resultLimit = 5;

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

            var jsonRaw = await _geoDbApiClient.SearchCitiesRawAsync(request.PartialCityName, resultLimit);
            var jsonDocument = JsonDocument.Parse(jsonRaw);
            var jsonCities = jsonDocument.RootElement.GetProperty("data");

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

            return citySearchResultDto;
        }
    }
}
