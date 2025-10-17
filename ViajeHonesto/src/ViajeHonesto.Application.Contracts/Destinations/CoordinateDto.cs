using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations;

public class CoordinateDto
{
    [Required]
    [Range(-90f, 90f)]
    public float Latitude { get; set; } = 0f;

    [Required]
    [Range(-180f, 180f)]
    public float Longitude { get; set; } = 0f;
}