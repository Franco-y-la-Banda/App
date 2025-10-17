using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

public class Rating : ValueObject
{
    public int Value { get; private set; }

    private Rating()
    {

    }

    public Rating(int value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
