using System;
using Volo.Abp.Application.Dtos;

namespace ViajeHonesto.Reviews;

public class ReviewDto : AuditedEntityDto<Guid>
{
    public RatingDto Rating { get; set; }
    public CommentDto Comment { get; set; }
}