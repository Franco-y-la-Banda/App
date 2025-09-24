using Volo.Abp.Modularity;

namespace ViajeHonesto;

[DependsOn(
    typeof(ViajeHonestoDomainModule),
    typeof(ViajeHonestoTestBaseModule)
)]
public class ViajeHonestoDomainTestModule : AbpModule
{

}
