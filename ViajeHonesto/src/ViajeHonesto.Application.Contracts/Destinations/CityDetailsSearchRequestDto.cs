using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class CityDetailsSearchRequestDto
    {
        private string? _wikiDataId;

        [Required]
        [RegularExpression(DestinationConsts.WikiDataIdRegex)]
        public string? WikiDataId
        {
            get => _wikiDataId;

            set => _wikiDataId = value?.ToUpper();
        }
    }
}
