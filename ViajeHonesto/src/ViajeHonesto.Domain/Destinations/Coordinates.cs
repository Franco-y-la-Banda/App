using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

public class Coordinate : ValueObject
{
    public float Latitude { get; private set; }

    public float Longitude { get; private set; }

    private Coordinate()
    {

    }

    public Coordinate(
        float latitude,
        float longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
