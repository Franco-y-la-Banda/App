using System.Collections.Generic;
using System.Threading.Tasks;

namespace ViajeHonesto.Destinations
{
    public interface ICitySearchService
    { 
        Task<CitySearchResultDto> SearchCitiesByNameAsync(CitySearchRequestDto request);
        Task<CityDetailsDto> SearchCityDetailsAsync(CityDetailsSearchRequestDto request);
    }
}