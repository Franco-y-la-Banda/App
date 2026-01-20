using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class CreateUpdateDestinationDto
    {
        private string? _wikiDataId = null;

        [RegularExpression(@"^[Qq][1-9]\d*$")]
        public string? WikiDataId
        {
            get => _wikiDataId;

            set => _wikiDataId = string.IsNullOrWhiteSpace(value) ? null : value.ToUpper();
        }

        [Required]
        [StringLength(DestinationConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(DestinationConsts.MaxRegionLength)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [StringLength(DestinationConsts.MaxCountryLength)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [Range(0, DestinationConsts.MaxPopulation)]
        public long Population { get; set; } = long.MinValue;

        [Required]
        public CoordinateDto Coordinate { get; set; }

        public DestinationPhotoDto[] Photos { get; set; }
    }
}