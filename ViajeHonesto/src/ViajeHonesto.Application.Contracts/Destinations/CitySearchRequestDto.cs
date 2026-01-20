namespace ViajeHonesto.Destinations
{
    public class CitySearchRequestDto
    {
        public string PartialCityName { get; set; }
        public int ResultLimit { get; set; } = 5;

        public int SkipCount { get; set; } = 0;
    }
}