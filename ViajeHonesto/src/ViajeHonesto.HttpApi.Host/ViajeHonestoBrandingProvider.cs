using Microsoft.Extensions.Localization;
using ViajeHonesto.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ViajeHonesto;

[Dependency(ReplaceServices = true)]
public class ViajeHonestoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ViajeHonestoResource> _localizer;

    public ViajeHonestoBrandingProvider(IStringLocalizer<ViajeHonestoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
