using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class CreateUpdateDestinationDto
    {
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
    }
}