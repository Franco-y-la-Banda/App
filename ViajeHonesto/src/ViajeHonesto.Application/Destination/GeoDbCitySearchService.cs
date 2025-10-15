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
            var jsonRaw = await _geoDbApiClient.SearchCitiesRawAsync(request.PartialCityName, resultLimit);
            var jsonDocument = JsonDocument.Parse(jsonRaw);
            var jsonCities = jsonDocument.RootElement.GetProperty("data");

            var citySearchResultDto = new CitySearchResultDto();
            foreach (var city in jsonCities.EnumerateArray())
            {
                citySearchResultDto.CityNames.Add(
                    new CityDto
                    {
                        Name = city.GetProperty("name").GetString(),
                        Country = city.GetProperty("country").GetString(),
                    });
            }

            return citySearchResultDto;
        }
    }
}
