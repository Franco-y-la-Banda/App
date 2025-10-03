using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViajeHonesto.Destinations;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace ViajeHonesto;

public class ViajeHonestoDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Destination, Guid> _destinationRepository;

    public ViajeHonestoDataSeederContributor(IRepository<Destination, Guid> destinationRepository)
    {
        _destinationRepository = destinationRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _destinationRepository.GetCountAsync() <= 0)
        {
            // TODO: Dejarlo bonito
            var photos = new List<DestinationPhoto>();
            var photo1 = new DestinationPhoto("aaaaaa");
            var photo2 = new DestinationPhoto("bbbbbb");
            photos.Add(photo1);
            photos.Add(photo2);

            await _destinationRepository.InsertAsync(
                new Destination
                {
                    Name = "Puerto Madero",
                    Country = "Argentina",
                    Region = "Buenos Aires",
                    Population = 5000,
                    Coordinate = new Coordinate(-34.6083f, -58.3636f),
                    Photos = photos
                },
                autoSave: true
            );

            await _destinationRepository.InsertAsync(
                new Destination
                {
                    Name = "Concepción del Uruguay",
                    Country = "Argentina",
                    Region = "Entre Ríos",
                    Population = 300000,
                    Coordinate = new Coordinate(-32.4833f, -58.2333f)
                },
                autoSave: true
            );
        }
    }
}
