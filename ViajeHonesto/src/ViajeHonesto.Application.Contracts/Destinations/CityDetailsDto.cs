using System;

namespace ViajeHonesto.Destinations
{
    public class CityDetailsDto
    {
        public string? WikiDataId { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public long Population { get; set; }
        public CoordinateDto Coordinate { get; set; } = new CoordinateDto { };
        public DestinationPhotoDto[] Photos { get; set; } = [];

        public bool IsSaved { get; set; }
        public Guid? LocalId { get; set; }
    }
}
