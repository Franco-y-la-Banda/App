using ViajeHonesto.Samples;
using Xunit;

namespace ViajeHonesto.EntityFrameworkCore.Domains;

[Collection(ViajeHonestoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ViajeHonestoEntityFrameworkCoreTestModule>
{

}
