using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Reviews
{
    public class UpdateReviewDto
    {
        public RatingDto Rating { get; set; }

        public CommentDto Comment { get; set; }

        public DateTime LastModificationTime { get; set; }
    }
}