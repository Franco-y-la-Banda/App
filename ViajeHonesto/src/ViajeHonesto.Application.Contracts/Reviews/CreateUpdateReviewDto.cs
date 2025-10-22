using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Reviews
{
    public class CreateUpdateReviewDto
    {
        [Range(0, 5)]
        public RatingDto Rating { get; set; }
        [StringLength(350)]
        public CommentDto Comment { get; set; }
    }
}