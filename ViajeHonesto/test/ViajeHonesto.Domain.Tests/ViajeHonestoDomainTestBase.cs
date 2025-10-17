using Volo.Abp.Modularity;

namespace ViajeHonesto;

/* Inherit from this class for your domain layer tests. */
public abstract class ViajeHonestoDomainTestBase<TStartupModule> : ViajeHonestoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
