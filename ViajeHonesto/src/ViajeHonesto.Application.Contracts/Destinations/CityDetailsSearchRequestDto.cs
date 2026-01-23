using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class CityDetailsSearchRequestDto
    {
        private string? _wikiDataId;

        [Required]
        [RegularExpression(@"^[Qq][1-9]\d*$")]
        public string? WikiDataId
        {
            get => _wikiDataId;

            set => _wikiDataId = value?.ToUpper();
        }
    }
}
