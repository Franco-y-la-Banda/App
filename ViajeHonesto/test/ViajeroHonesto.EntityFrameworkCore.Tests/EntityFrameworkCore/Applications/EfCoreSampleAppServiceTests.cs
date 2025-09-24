using ViajeroHonesto.Samples;
using Xunit;

namespace ViajeroHonesto.EntityFrameworkCore.Applications;

[Collection(ViajeroHonestoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ViajeroHonestoEntityFrameworkCoreTestModule>
{

}
