using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
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
    private readonly IRepository<Destination, Guid> _destinationRepository;
    public DestinationAppService(IRepository<Destination, Guid> repository)
        : base(repository)
    {
        _destinationRepository = repository;
    }

    public async Task<DestinationDto> GetWithDetails(Guid id)
    {
        //Get a IQueryable<T> by including sub collections
        var queryable = await _destinationRepository.WithDetailsAsync(x => x.Photos);

        //Apply additional LINQ extension methods
        var query = queryable.Where(x => x.Id == id);

        //Execute the query and get the result
        var order = await AsyncExecuter.FirstOrDefaultAsync(query);
        return ObjectMapper.Map<Destination, DestinationDto>(order);
    }

    public override async Task<DestinationDto> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id, includeDetails: true); // trae Photos si WithDetails incluye Photos
        if (entity == null)
            throw new EntityNotFoundException(typeof(Destination), id);
        return ObjectMapper.Map<Destination, DestinationDto>(entity);
    }

    // Para GetList:
    protected override async Task<IQueryable<Destination>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        return (await Repository.WithDetailsAsync(x => x.Photos));
    }
}
