using Volo.Abp.Modularity;

namespace ViajeHonesto;

[DependsOn(
    typeof(ViajeHonestoApplicationModule),
    typeof(ViajeHonestoDomainTestModule)
)]
public class ViajeHonestoApplicationTestModule : AbpModule
{

}
