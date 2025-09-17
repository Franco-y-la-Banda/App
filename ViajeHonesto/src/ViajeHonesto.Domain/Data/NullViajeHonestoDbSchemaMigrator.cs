using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ViajeHonesto.Data;

/* This is used if database provider does't define
 * IViajeHonestoDbSchemaMigrator implementation.
 */
public class NullViajeHonestoDbSchemaMigrator : IViajeHonestoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
