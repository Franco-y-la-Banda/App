using System;
using System.Threading.Tasks;
using ViajeHonesto.Constants;
using ViajeHonesto.Reviews;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace ViajeHonesto;

class ReviewsDataSeederContributor: ITransientDependency
{
    private readonly IRepository<Review> _reviewRepository;

    public ReviewsDataSeederContributor(IRepository<Review> reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _reviewRepository.GetCountAsync() <= 0)
        {
            var review = new Review(
            TestGuids.DestinationGuid(0),
            TestGuids.UserGuid(0),
            new Rating(5),
            new CComment("Muy buena"));
            await _reviewRepository.InsertAsync(review, autoSave: true);

            review = new Review(
                TestGuids.DestinationGuid(1),
                TestGuids.UserGuid(1),
                new Rating(4),
                new CComment("Hermosa vista y buena comida"));
            await _reviewRepository.InsertAsync(review, autoSave: true);

            review = new Review(
                TestGuids.DestinationGuid(2),
                TestGuids.UserGuid(2),
                new Rating(3),
                new CComment("Lindo lugar pero algo caro"));
            await _reviewRepository.InsertAsync(review, autoSave: true);

            review = new Review(
                TestGuids.DestinationGuid(3),
                TestGuids.UserGuid(0),
                new Rating(5),
                new CComment("Una experiencia inolvidable"));
            await _reviewRepository.InsertAsync(review, autoSave: true);

            review = new Review(
                TestGuids.DestinationGuid(1),
                TestGuids.UserGuid(2),
                new Rating(4),
                new CComment("Ideal para relajarse un fin de semana"));
            await _reviewRepository.InsertAsync(review, autoSave: true);
        }
    }
}