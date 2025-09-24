using Xunit;

namespace ViajeHonesto.EntityFrameworkCore;

[CollectionDefinition(ViajeHonestoTestConsts.CollectionDefinitionName)]
public class ViajeHonestoEntityFrameworkCoreCollection : ICollectionFixture<ViajeHonestoEntityFrameworkCoreFixture>
{

}
