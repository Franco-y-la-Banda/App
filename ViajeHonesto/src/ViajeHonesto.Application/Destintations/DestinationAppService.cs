using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ViajeHonesto.Destinations;

public class DestinationAppService :
    CrudAppService<
        Destination, //The Destinations entity
        DestinationDto, //Used to show Destinations
        Guid, //Primary key of the Destinations entity
        PagedAndSortedResultRequestDto, //Used for sorting
        CreateUpdateDestinationDto>, //Used to create/update a Destination
    IDestinationAppService //implement the IDestinationAppService
{
    public DestinationAppService(IRepository<Destination, Guid> repository)
        : base(repository)
    {
        
    }
}
