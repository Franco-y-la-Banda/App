using Volo.Abp.Modularity;

namespace ViajeroHonesto;

public abstract class ViajeroHonestoApplicationTestBase<TStartupModule> : ViajeroHonestoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
