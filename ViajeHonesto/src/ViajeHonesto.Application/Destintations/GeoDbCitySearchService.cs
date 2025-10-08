using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ViajeHonesto.Destinations;
using Volo.Abp.Application.Services;

namespace ViajeHonesto.Destintations
{
    public class GeoDbCitySearchServices : ApplicationService, ICitySearchService
    {
        private readonly ICitySearchService _citySearchService;

        public GeoDbCitySearchServices(ICitySearchService citySearchService)
        {
            _citySearchService = citySearchService;
        }

        private static readonly string apiKey = "0d591376bamsh69ea0c8ddcb541ep152145jsn345c066e6f52";
        private static readonly int resultLimit = 5;
        private static readonly string baseUrl = "https://wft-geo-db.p.rapidapi.com/v1/geo";

        public async Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request)
        {
            // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview
            var jsonResult = await SearchCities(request);
            CitySearchResultDto? citySearchResultDto = JsonSerializer.Deserialize<CitySearchResultDto>(jsonResult);
            return citySearchResultDto ?? new CitySearchResultDto { CityNames = new List<CityDto>() };
        }

        static async Task<string> SearchCities(CitySearchRequestDto request)
        {
            using (HttpClient client = new HttpClient())
            {
                // Configuración de Headers
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

                // Endpoint con query params
                string url = $"{baseUrl}/cities?namePrefix={Uri.EscapeDataString(request.PartialCityName)}&limit={resultLimit}";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    return jsonResult;
                }
                else
                {
                    throw new Exception("Error fetching city data");
                }
            }
        }
    }
}
