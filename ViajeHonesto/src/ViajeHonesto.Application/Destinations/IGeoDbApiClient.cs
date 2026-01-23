using System.Threading.Tasks;

namespace ViajeHonesto.Destinations
{
    public interface IGeoDbApiClient
    {
        Task<string> SearchCitiesRawAsync(string partialCityName, int limit, int skipCount);
        Task<string> SearchCityDetailsRawAsync(string wikiDataId);
    }
}
