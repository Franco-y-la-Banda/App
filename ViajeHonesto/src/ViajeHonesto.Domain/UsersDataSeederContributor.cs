using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using ViajeHonesto.Constants;

namespace ViajeHonesto;
class UsersDataSeederContributor : ITransientDependency
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IIdentityUserRepository _identityUserRepository;

    public UsersDataSeederContributor(IdentityUserManager identityUserManager, 
        IIdentityUserRepository identityUserRepository)
    {
        _identityUserManager = identityUserManager;
        _identityUserRepository = identityUserRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _identityUserRepository.GetCountAsync() <= 1)
        {
            int guidIndex = 0;

            // Add users
            IdentityUser identityUser1 = new IdentityUser(TestGuids.UserGuid(guidIndex), "test_maria.gomez", "maria.gomez@testmail.com");
            await _identityUserManager.CreateAsync(identityUser1, "1q2w3E*");
            await _identityUserManager.AddToRoleAsync(identityUser1, "Admin");
            guidIndex++;

            IdentityUser identityUser2 = new IdentityUser(TestGuids.UserGuid(guidIndex), "test_luis.rodriguez", "luis.rodriguez@testmail.com");
            await _identityUserManager.CreateAsync(identityUser2, "1q2w3E*");
            await _identityUserManager.AddToRoleAsync(identityUser2, "Admin");
            guidIndex++;

            IdentityUser identityUser3 = new IdentityUser(TestGuids.UserGuid(guidIndex), "test.valentina.perez", "valentina.perez@testmail.com");
            await _identityUserManager.CreateAsync(identityUser3, "1q2w3E*");
            //await _identityUserManager.AddToRoleAsync(identityUser3, "Admin");  // Intentionally not making this user an admin
            guidIndex++;
        }
    }
}