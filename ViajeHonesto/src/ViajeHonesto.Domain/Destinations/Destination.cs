using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ViajeHonesto.Destinations;

public class Destination : AuditedAggregateRoot<Guid>
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string Region { get; set; }
    public required long Population { get; set; }
    public Coordinate Coordinate { get; set; }
    public List<DestinationPhoto> Photos { get; set; } = new List<DestinationPhoto>();
}