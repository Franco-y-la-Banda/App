using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class CitySearchRequestDto
    {
        public string PartialCityName { get; set; } = string.Empty;
        public int ResultLimit { get; set; } = 5;
        public int SkipCount { get; set; } = 0;
        public string? CountryCode { get; set; } = null;
        public long? MinPopulation { get; set; } = null;
        public long? MaxPopulation { get; set; } = null;
        public string? Sort { get; set; } = null;
    }
}