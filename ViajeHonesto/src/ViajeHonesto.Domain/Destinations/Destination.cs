using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ViajeHonesto.Destinations;

public class Destination : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public long Population { get; set; }
}