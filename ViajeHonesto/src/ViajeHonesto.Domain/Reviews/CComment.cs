using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

public class CComment : ValueObject
{
    public string Comment { get; private set; }

    private CComment()
    {

    }

    public CComment(string comment)
    {
        Comment = comment;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Comment;
    }
}
