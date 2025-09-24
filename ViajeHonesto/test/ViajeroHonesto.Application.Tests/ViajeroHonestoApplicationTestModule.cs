using Volo.Abp.Modularity;

namespace ViajeroHonesto;

[DependsOn(
    typeof(ViajeroHonestoApplicationModule),
    typeof(ViajeroHonestoDomainTestModule)
)]
public class ViajeroHonestoApplicationTestModule : AbpModule
{

}
