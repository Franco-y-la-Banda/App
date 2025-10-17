using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace ViajeHonesto.Destinations;
public class DestinationPhoto : Entity
{
    public Destination Destination { get; protected set; }
    public Guid PhotoId { get; protected set; }
    public Guid DestinationId { get; protected set; }
    public string Path { get; protected set; }

    private DestinationPhoto()
    {

    }

    internal DestinationPhoto(Guid photoId, Guid destinationId, Destination destination, string path)
    {
        PhotoId = photoId;
        DestinationId = destinationId;
        Destination = destination;
        Path = path;
    }

    public override object[] GetKeys()
    {
        return new Object[] { PhotoId, DestinationId};
    }
}
