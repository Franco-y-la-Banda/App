using System.Collections.Generic;

namespace ViajeHonesto.Destinations
{
    public class CitySearchResultDto
    {
        public List<CityDto> CityNames { get; set; } = new();
        public int TotalCount { get; set; }
    }
}