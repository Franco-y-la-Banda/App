using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ViajeHonesto.Destinations;

public class DestinationPhotoDto : EntityDto<Guid>
{
    [Required]
    public string Path { get; set; }

}