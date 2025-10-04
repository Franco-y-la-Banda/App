using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

public class DestinationPhoto : Entity<Guid>
{

    public string Path { get; private set; }


    private DestinationPhoto()
    {

    }

    public DestinationPhoto(string path)
    {
        Path = path;
    }
}
