using Volo.Abp.Modularity;

namespace ViajeroHonesto;

/* Inherit from this class for your domain layer tests. */
public abstract class ViajeroHonestoDomainTestBase<TStartupModule> : ViajeroHonestoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
