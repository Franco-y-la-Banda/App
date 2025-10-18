using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using static System.Net.WebRequestMethods;

namespace ViajeHonesto.Destinations
{
    public class GeoDbApiClient : IGeoDbApiClient, ITransientDependency
    {
        private readonly HttpClient _client;

        public GeoDbApiClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<string> SearchCitiesRawAsync(string partialCityName, int limit)
        {
            // Endpoint con query 
            string url = $"{_client.BaseAddress}cities?namePrefix={Uri.EscapeDataString(partialCityName)}&limit={limit}";

            HttpResponseMessage response = await _client.GetAsync(url);

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
