using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ViajeHonesto.Destinations;

public class Destination : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public long Population { get; set; }
    public Coordinate Coordinate { get; set; }
    public List<DestinationPhoto> Photos { get; private set; }

    private Destination() 
    { 

    }

    public Destination(Guid id) : base(id)
    {
        Photos = new List<DestinationPhoto>();
    }

    public void addPhoto(Guid photoId, string path)
    {
        Photos.Add(new DestinationPhoto(photoId, this.Id, this, path));
    }
}