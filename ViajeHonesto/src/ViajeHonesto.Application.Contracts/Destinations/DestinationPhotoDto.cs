using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations;

public class DestinationPhotoDto
{
    [Required]
    public string Path { get; set; }

}