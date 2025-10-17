using ViajeHonesto.Samples;
using Xunit;

namespace ViajeHonesto.EntityFrameworkCore.Applications;

[Collection(ViajeHonestoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ViajeHonestoEntityFrameworkCoreTestModule>
{

}
