using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class CreateUpdateDestinationDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [StringLength(512)]
        public string Country { get; set; } = string.Empty;

        [Required]
        public long Population { get; set; } = long.MinValue;
     }
}