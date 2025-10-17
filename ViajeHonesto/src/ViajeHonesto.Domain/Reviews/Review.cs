using System;
using ViajeHonesto.Filtering;
using Volo.Abp.Domain.Entities.Auditing;

namespace ViajeHonesto.Reviews;

public class Review : AuditedAggregateRoot, IUserOwned
{
    public Rating? Rating { get; private set; }
    public CComment? Comment { get; private set; }

    public Guid DestinationId { get; protected set; }
    public Guid UserId { get; set; }

    private Review()
    {

    }

    public Review(Guid destinationId, Guid userId, Rating? rating, CComment? comment)
    {
        DestinationId = destinationId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
    }

    public override object?[] GetKeys()
    {
        return new Object[] { DestinationId, UserId };
    }
}