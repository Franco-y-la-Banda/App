using ViajeHonesto.Localization;
using Volo.Abp.Application.Services;

namespace ViajeHonesto;

/* Inherit your application services from this class.
 */
public abstract class ViajeHonestoAppService : ApplicationService
{
    protected ViajeHonestoAppService()
    {
        LocalizationResource = typeof(ViajeHonestoResource);
    }
}
