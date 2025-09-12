using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ViajeHonesto.Data;
using Volo.Abp.DependencyInjection;

namespace ViajeHonesto.EntityFrameworkCore;

public class EntityFrameworkCoreViajeHonestoDbSchemaMigrator
    : IViajeHonestoDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreViajeHonestoDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ViajeHonestoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ViajeHonestoDbContext>()
            .Database
            .MigrateAsync();
    }
}
