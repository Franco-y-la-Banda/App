using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ViajeHonesto.Destinations;

public class DestinationPhotoDto : EntityDto
{
    public Guid? PhotoId { get; set; }
    public string Path { get; set; }

}