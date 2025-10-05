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
    private readonly IRepository<DestinationPhoto> _photoRepository;
    public DestinationAppService(
        IRepository<Destination, Guid> repository,
        IRepository<DestinationPhoto> photoRepository)
        : base(repository)
    {
        _destinationRepository = repository;
        _photoRepository = photoRepository;
    }

    private async Task<Destination> GetDestinationWithDetailsAsync(Guid id)
    {
        //Get a IQueryable<T> by including sub collections
        var queryable = await _destinationRepository.WithDetailsAsync(x => x.Photos);

        //Apply additional LINQ extension methods
        var query = queryable.Where(x => x.Id == id);

        //Execute the query and get the result
        var order = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (order == null)
            throw new EntityNotFoundException(typeof(Destination), id);
        return order;
    }

    public async Task<DestinationDto> GetWithDetailsAsync(Guid id)
    {
        return ObjectMapper.Map<Destination, DestinationDto>(
            await GetDestinationWithDetailsAsync(id));
    }

    public override async Task<DestinationDto> UpdateAsync(Guid id, CreateUpdateDestinationDto input)
    {
        var savedDestination = await GetDestinationWithDetailsAsync(id);
        
        // NOTA: Puede ser que no mapee todos los datos de las fotos
        // No sé si es un problema, pero funca. tkm ef core
        await MapToEntityAsync(input, savedDestination);
        await _destinationRepository.UpdateAsync(savedDestination, autoSave: true);

        return await MapToGetOutputDtoAsync(savedDestination);
    }

    // Para GetList:
    protected override async Task<IQueryable<Destination>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        return (await Repository.WithDetailsAsync(x => x.Photos));
    }
}
