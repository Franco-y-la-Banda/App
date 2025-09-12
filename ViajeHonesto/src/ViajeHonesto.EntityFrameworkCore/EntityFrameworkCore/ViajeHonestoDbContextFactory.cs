using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ViajeHonesto.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ViajeHonestoDbContextFactory : IDesignTimeDbContextFactory<ViajeHonestoDbContext>
{
    public ViajeHonestoDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        ViajeHonestoEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ViajeHonestoDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new ViajeHonestoDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ViajeHonesto.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
