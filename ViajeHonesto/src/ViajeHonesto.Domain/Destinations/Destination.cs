using System;
using System.Collections.Generic;
using System.Linq;
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

    public void AddPhoto(Guid photoId, string path)
    {
        if (photoId == Guid.Empty)
        {
            throw new ArgumentException("Photo ID cannot be empty.", nameof(photoId));
        }
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Photo path cannot be null or empty.", nameof(path));
        }
        if (Photos.Any(p => p.PhotoId == photoId))
        {
            throw new InvalidOperationException($"A photo with ID {photoId} already exists.");
        }

        Photos.Add(new DestinationPhoto(photoId, this.Id, this, path));
    }

    public void RemovePhoto(Guid photoId)
    {
        var photo = Photos.FirstOrDefault(p => p.PhotoId == photoId);
        if (photo == null)
        {
            throw new InvalidOperationException($"Photo with ID {photoId} not found.");
        }

        Photos.Remove(photo);
    }
}