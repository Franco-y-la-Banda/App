using ViajeroHonesto.Samples;
using Xunit;

namespace ViajeroHonesto.EntityFrameworkCore.Domains;

[Collection(ViajeroHonestoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ViajeroHonestoEntityFrameworkCoreTestModule>
{

}
