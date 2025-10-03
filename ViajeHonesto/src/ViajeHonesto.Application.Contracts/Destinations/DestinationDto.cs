using System;
using Volo.Abp.Application.Dtos;

namespace ViajeHonesto.Destinations;

public class DestinationDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public string Country { get; set; }

    public string Region { get; set; }

    public long Population { get; set; }

    public CoordinateDto Coordinate { get; set; }
}
