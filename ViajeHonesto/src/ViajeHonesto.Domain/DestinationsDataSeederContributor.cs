using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViajeHonesto.Destinations;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using ViajeHonesto.Constants;

namespace ViajeHonesto;

public class DestinationsDataSeederContributor : ITransientDependency
{
    private readonly IRepository<Destination, Guid> _destinationRepository;

    public DestinationsDataSeederContributor(IRepository<Destination, Guid> destinationRepository)
    {
        _destinationRepository = destinationRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _destinationRepository.GetCountAsync() <= 0)
        {
            var guidIndex = 0;

            var destination = new Destination(TestGuids.DestinationGuid(guidIndex))
            {
                Name = "El Chaltén",
                Country = "Argentina",
                Region = "Santa Cruz",
                Population = 1600,
                Coordinate = new Coordinate(-49.3315f, -72.8863f)
            };

            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/el-chalten/fitz-roy.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/el-chalten/laguna-capri.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/el-chalten/sendero-laguna-torre.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/el-chalten/pueblo-niebla.jpg");

            await _destinationRepository.InsertAsync(
                destination,
                autoSave: true
            );

            guidIndex++;

            destination = new Destination(TestGuids.DestinationGuid(guidIndex))
            {
                Name = "Puerto Madero",
                Country = "Argentina",
                Region = "Buenos Aires",
                Population = 5000,
                Coordinate = new Coordinate(-34.6083f, -58.3636f)
            };

            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/puerto-madero/puente-de-la-mujer.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/puerto-madero/dique-de-noche.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/puerto-madero/skyline-rio.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/puerto-madero/restaurantes-costanera.jpg");


            await _destinationRepository.InsertAsync(
                destination,
                autoSave: true
            );

            guidIndex++;

            destination = new Destination(TestGuids.DestinationGuid(guidIndex))
            {
                Name = "Concepción del Uruguay",
                Country = "Argentina",
                Region = "Entre Ríos",
                Population = 300000,
                Coordinate = new Coordinate(-32.4833f, -58.2333f)
            };

            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/concepcion-del-uruguay/ramblas-costanera.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/concepcion-del-uruguay/plaza-ramirez.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/concepcion-del-uruguay/palacio-san-jose.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/concepcion-del-uruguay/playa-banco-pelay.jpg");

            await _destinationRepository.InsertAsync(
                destination,
                autoSave: true
            );

            guidIndex++;

            destination = new Destination(TestGuids.DestinationGuid(guidIndex))
            {
                Name = "Villa La Angostura",
                Country = "Argentina",
                Region = "Neuquén",
                Population = 14000,
                Coordinate = new Coordinate(-40.7617f, -71.6461f)
            };

            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/villa-la-angostura/bosque-arrayanes.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/villa-la-angostura/lago-nahuel-huapi.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/villa-la-angostura/centro-invierno.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/villa-la-angostura/mirador-belvedere.jpg");

            await _destinationRepository.InsertAsync(
                destination,
                autoSave: true
            );

            guidIndex++;

            destination = new Destination(TestGuids.DestinationGuid(guidIndex))
            {
                Name = "Tilcara",
                Country = "Argentina",
                Region = "Jujuy",
                Population = 8000,
                Coordinate = new Coordinate(-23.5733f, -65.3928f)
            };

            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/tilcara/pucara-de-tilcara.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/tilcara/quebrada-humahuaca.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/tilcara/mercado-artesanal.jpg");
            destination.AddPhoto(Guid.NewGuid(), "/images/destinations/tilcara/callecitas-colores.jpg");

            await _destinationRepository.InsertAsync(
                destination,
                autoSave: true
            );

            guidIndex++;
        }
    }
}
