using Microsoft.Extensions.DependencyInjection;
using System;
using ViajeHonesto.Destinations;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace ViajeHonesto;

[DependsOn(
    typeof(ViajeHonestoDomainModule),
    typeof(ViajeHonestoApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpHttpClientModule)
    )]
public class ViajeHonestoApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ViajeHonestoApplicationModule>();
        });

        context.Services.AddHttpClient<IGeoDbApiClient, GeoDbApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://wft-geo-db.p.rapidapi.com/v1/geo/");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "0d591376bamsh69ea0c8ddcb541ep152145jsn345c066e6f52");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");
        });
    }
}
