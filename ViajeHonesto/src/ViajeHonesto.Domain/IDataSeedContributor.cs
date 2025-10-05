using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViajeHonesto.Destinations;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

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
            var destination = new Destination(Guid.NewGuid())
            {
                Name = "Puerto Madero",
                Country = "Argentina",
                Region = "Buenos Aires",
                Population = 5000,
                Coordinate = new Coordinate(-34.6083f, -58.3636f)
            };

            destination.addPhoto(Guid.NewGuid(), "Esto es una foto de prueba");
            destination.addPhoto(Guid.NewGuid(), "Prueba es una foto de");


            await _destinationRepository.InsertAsync(
                destination,
                autoSave: true
            );

            await _destinationRepository.InsertAsync(
                new Destination(Guid.NewGuid())
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
