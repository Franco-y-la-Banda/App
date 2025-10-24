using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Reviews
{
    public class CreateReviewDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid DestinationId { get; set; }

        public RatingDto Rating { get; set; }

        public CommentDto Comment { get; set; }
    }
}