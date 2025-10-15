using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ViajeHonesto.Destinations
{
    public class GeoDbApiClient : IGeoDbApiClient, ITransientDependency
    {
        private static readonly string apiKey = "0d591376bamsh69ea0c8ddcb541ep152145jsn345c066e6f52";
        private static readonly string baseUrl = "https://wft-geo-db.p.rapidapi.com/v1/geo";

        public GeoDbApiClient()
        {

        }

        public async Task<string> SearchCitiesRawAsync(string partialCityName, int limit)
        {
            using (HttpClient client = new HttpClient())
            {
                // Configuración de Headers
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

                // Endpoint con query params
                string url = $"{baseUrl}/cities?namePrefix={Uri.EscapeDataString(partialCityName)}&limit={limit}";

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
