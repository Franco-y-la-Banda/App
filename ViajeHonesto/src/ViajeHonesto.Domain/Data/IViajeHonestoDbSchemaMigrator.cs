using System.Threading.Tasks;

namespace ViajeHonesto.Data;

public interface IViajeHonestoDbSchemaMigrator
{
    Task MigrateAsync();
}
