using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace ViajeHonesto.Destinations
{
    public class GeoDbApiClient : IGeoDbApiClient, ITransientDependency
    {
        private readonly HttpClient _client;

        public GeoDbApiClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<string> SearchCitiesRawAsync(CitySearchRequestDto input)
        {
            var queryParams = new List<string>
            {
                $"namePrefix={Uri.EscapeDataString(input.PartialCityName)}"
            };

            if (input.MinPopulation.HasValue)
            {
                queryParams.Add($"minPopulation={input.MinPopulation.Value}");
            }

            if (input.MaxPopulation.HasValue)
            {
                queryParams.Add($"maxPopulation={input.MaxPopulation.Value}");
            }

            if (!string.IsNullOrEmpty(input.Sort))
            {
                queryParams.Add($"sort={input.Sort}");
            }

            if (!string.IsNullOrEmpty(input.CountryCode))
            { 
                queryParams.Add($"countryIds={input.CountryCode}");
            }

            queryParams.Add($"limit={input.ResultLimit}");
            queryParams.Add($"offset={input.SkipCount}");

            string queryString = string.Join("&", queryParams);

            string fullUrl = $"cities?{queryString}";

            HttpResponseMessage response = await _client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                return jsonResult;
            }
            else
            {
                throw new HttpRequestException("Error fetching city data");
            }
        }

        public async Task<string> SearchCityDetailsRawAsync(string wikiDataId)
        {
            string url = $"{_client.BaseAddress}cities/{Uri.EscapeDataString(wikiDataId)}";

            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                return jsonResult;
            }
            else
            {
                throw new UserFriendlyException("Error fetching city details", response.StatusCode.ToString(), response.ToString());
            }
        }
        public async Task<string> SearchCitiesRegionRawAsync(CitySearchRequestDto input)
        {
            var queryParams = new List<string>
            {
                $"namePrefix={Uri.EscapeDataString(input.PartialCityName)}"
            };

            if (input.MinPopulation.HasValue)
            {
                queryParams.Add($"minPopulation={input.MinPopulation.Value}");
            }

            if (input.MaxPopulation.HasValue)
            {
                queryParams.Add($"maxPopulation={input.MaxPopulation.Value}");
            }

            if (!string.IsNullOrEmpty(input.Sort))
            {
                queryParams.Add($"sort={input.Sort}");
            }

            queryParams.Add($"limit={input.ResultLimit}");
            queryParams.Add($"offset={input.SkipCount}");

            string queryString = string.Join("&", queryParams);

            string fullUrl = $"countries/{input.CountryCode}/regions/{input.RegionCode}/cities?{queryString}";

            HttpResponseMessage response = await _client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                return jsonResult;
            }
            else
            {
                throw new HttpRequestException("Error fetching city data");
            }
        }
    }
}
