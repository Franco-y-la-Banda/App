using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

public class DestinationPhoto : ValueObject
{
    public string Path { get; private set; }


    private DestinationPhoto()
    {

    }

    public DestinationPhoto(
        string path)
    {
        Path = path;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Path;
    }
}
