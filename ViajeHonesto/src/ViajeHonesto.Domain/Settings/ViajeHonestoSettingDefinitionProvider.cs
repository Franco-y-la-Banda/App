using Volo.Abp.Settings;

namespace ViajeHonesto.Settings;

public class ViajeHonestoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ViajeHonestoSettings.MySetting1));
    }
}
