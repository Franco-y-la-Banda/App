using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace ViajeHonesto;
class ViajeHonestoDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly UsersDataSeederContributor _users;
    private readonly DestinationsDataSeederContributor _destinations;
    private readonly ReviewsDataSeederContributor _reviews;

    public ViajeHonestoDataSeederContributor(
        UsersDataSeederContributor users,
        DestinationsDataSeederContributor destinations,
        ReviewsDataSeederContributor reviews)
    {
        _users = users;
        _destinations = destinations;
        _reviews = reviews;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await _users.SeedAsync(context);
        await _destinations.SeedAsync(context);
        await _reviews.SeedAsync(context);
    }
}