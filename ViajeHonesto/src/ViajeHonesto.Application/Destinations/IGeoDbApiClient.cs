using System.Threading.Tasks;

namespace ViajeHonesto.Destinations
{
    public interface IGeoDbApiClient
    {
        Task<string> SearchCitiesRawAsync(CitySearchRequestDto input);
        Task<string> SearchCityDetailsRawAsync(string wikiDataId);
        Task<string> SearchCitiesRegionRawAsync(CityRegionSearchRequestDto input);
    }
}
