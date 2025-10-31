using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;
using Volo.Abp.Validation;

public class Rating : ValueObject
{
    public int Value { get; private set; }

    private Rating()
    {

    }

    public Rating(int value)
    {
        if (value < 0 || value > 5)
        {
            throw new AbpValidationException($"Rating must be between 0 and 5. Received: {value}");
        }

        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
