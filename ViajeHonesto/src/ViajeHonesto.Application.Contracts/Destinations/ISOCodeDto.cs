using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Destinations
{
    public class ISOCodeDto
    {
        [StringLength(3, MinimumLength = 1)]
        public string ISOCode { get; set; } = "";

        [StringLength(100)]
        public string Name { get; set; } = "";
    }
}
