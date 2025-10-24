using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ViajeHonesto.Reviews;

public class ReviewDto : AuditedEntityDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid DestinationId { get; set; }

    public RatingDto Rating { get; set; }
    public CommentDto Comment { get; set; }
}