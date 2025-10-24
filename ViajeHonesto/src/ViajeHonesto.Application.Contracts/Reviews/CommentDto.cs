using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ViajeHonesto.Reviews;

public class CommentDto
{
    [StringLength(350)]
    public string Comment { get; set; }
}
