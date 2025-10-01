using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ViajeHonesto.Destinations;

public class Destination : AuditedAggregateRoot<Guid>
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string Region { get; set; }
    public required long Population { get; set; }
}