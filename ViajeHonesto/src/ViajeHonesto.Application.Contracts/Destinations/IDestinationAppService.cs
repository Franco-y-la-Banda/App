using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ViajeHonesto.Destinations;

public interface IDestinationAppService : 
    ICrudAppService< //Defines CRUD methods
        DestinationDto, //Used to show Destinations
        Guid, //Primary key of the Destination entity
        PagedAndSortedResultRequestDto, //Used for sorting
        CreateUpdateDestinationDto> //Used to create/update a Destination
{
    public Task<PagedResultDto<CityDto>> SearchCitiesByNameAsync(CitySearchRequestDto request);
}