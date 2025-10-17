using Volo.Abp.Modularity;

namespace ViajeHonesto;

public abstract class ViajeHonestoApplicationTestBase<TStartupModule> : ViajeHonestoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
