using Elders.Iso3166;
using System;
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
            var jsonRaw = await _geoDbApiClient.SearchCitiesRawAsync(request);
            var jsonDocument = JsonDocument.Parse(jsonRaw);
            var jsonCities = jsonDocument.RootElement.GetProperty("data");
            var jsonMetadata = jsonDocument.RootElement.GetProperty("metadata");

            var citySearchResultDto = new CitySearchResultDto();
            foreach (var city in jsonCities.EnumerateArray())
            {
                citySearchResultDto.CityNames.Add(
                    new CityDto
                    {
                        WikiDataId = city.GetProperty("wikiDataId").ToString(),
                        Name = city.GetProperty("name").GetString(),
                        Country = city.GetProperty("country").GetString(),
                        Region = city.GetProperty("region").GetString(),
                        Population = city.GetProperty("population").GetInt64(),
                    });
            }

            citySearchResultDto.TotalCount = jsonMetadata.GetProperty("totalCount").GetInt32();

            return citySearchResultDto;
        }


        public async Task<CityDetailsDto> SearchCityDetailsAsync(CityDetailsSearchRequestDto request)
        {
            if (request.WikiDataId.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("El WikiDataId de la ciudad no puede estar vacío.");
            };

            var jsonRaw = await _geoDbApiClient.SearchCityDetailsRawAsync(request.WikiDataId!);
            var jsonDocument = JsonDocument.Parse(jsonRaw);
            var jsonCityDetails = jsonDocument.RootElement.GetProperty("data");

            var cityDetails =  new CityDetailsDto
            {
                WikiDataId = jsonCityDetails.GetProperty("wikiDataId").ToString(),
                Name = jsonCityDetails.GetProperty("name").GetString(),
                Country = jsonCityDetails.GetProperty("country").GetString(),
                Region = jsonCityDetails.GetProperty("region").GetString(),
                Population = jsonCityDetails.GetProperty("population").GetInt64(),
                Coordinate =
                {
                    Latitude = jsonCityDetails.GetProperty("latitude").GetSingle(),
                    Longitude = jsonCityDetails.GetProperty("longitude").GetSingle()
                }
            };

            return cityDetails;
        }

        public async Task<CitySearchResultDto> SearchCitiesByRegion(CityRegionSearchRequestDto request)
        {
            if (request.RegionCode.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("The RegionCode must be specified");
            }
            if (request.CountryCode.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("The CountryCode must be specified");
            }

            var jsonRaw = await _geoDbApiClient.SearchCitiesRegionRawAsync(request);
            var jsonDocument = JsonDocument.Parse(jsonRaw);
            var jsonCities = jsonDocument.RootElement.GetProperty("data");
            var jsonMetadata = jsonDocument.RootElement.GetProperty("metadata");

            var citySearchResultDto = new CitySearchResultDto();

            var country = new Country(request.CountryCode).Name;
            var region = new Subdivision($"{request.CountryCode}-{request.RegionCode}").Name;

            foreach (var city in jsonCities.EnumerateArray())
            {
                citySearchResultDto.CityNames.Add(
                    new CityDto
                    {
                        WikiDataId = city.GetProperty("wikiDataId").ToString(),
                        Name = city.GetProperty("name").GetString(),
                        Region = region,
                        Country = country,
                        Population = city.GetProperty("population").GetInt64(),
                    });
            }

            citySearchResultDto.TotalCount = jsonMetadata.GetProperty("totalCount").GetInt32();

            return citySearchResultDto;
        }
    }
}
