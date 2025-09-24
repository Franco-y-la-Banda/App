using Volo.Abp.Modularity;

namespace ViajeroHonesto;

[DependsOn(
    typeof(ViajeroHonestoDomainModule),
    typeof(ViajeroHonestoTestBaseModule)
)]
public class ViajeroHonestoDomainTestModule : AbpModule
{

}
