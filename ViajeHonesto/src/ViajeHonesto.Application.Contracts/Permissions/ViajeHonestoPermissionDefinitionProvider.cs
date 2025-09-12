using ViajeHonesto.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace ViajeHonesto.Permissions;

public class ViajeHonestoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ViajeHonestoPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ViajeHonestoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ViajeHonestoResource>(name);
    }
}
