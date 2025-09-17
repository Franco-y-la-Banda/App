using ViajeHonesto.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ViajeHonesto.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ViajeHonestoEntityFrameworkCoreModule),
    typeof(ViajeHonestoApplicationContractsModule)
)]
public class ViajeHonestoDbMigratorModule : AbpModule
{
}
