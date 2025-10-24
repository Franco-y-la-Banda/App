using System;
using System.ComponentModel.DataAnnotations;

namespace ViajeHonesto.Reviews;

public class RatingDto
{
    [Range(0, 5)]
    public int Value { get; set; }
}
