using ViajeHonesto.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ViajeHonesto.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ViajeHonestoController : AbpControllerBase
{
    protected ViajeHonestoController()
    {
        LocalizationResource = typeof(ViajeHonestoResource);
    }
}
