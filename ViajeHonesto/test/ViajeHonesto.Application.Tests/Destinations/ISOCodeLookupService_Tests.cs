using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace ViajeHonesto.Destinations
{
    public abstract class ISOCodeLookupService_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IISOCodeLookupService _ISOCodeLookupService;

        protected ISOCodeLookupService_Tests()
        {
            _ISOCodeLookupService = GetRequiredService<IISOCodeLookupService>();
        }

        [Fact]
        public void GetCountries_Should_Return_Countries_List()
        {
            // Act
            var countries = _ISOCodeLookupService.GetCountries();

            // Assert
            countries.ShouldNotBeEmpty();
            countries.ShouldContain(c => c.Name == "Argentina" && c.ISOCode == "AR");
        }

        [Fact]
        public void GetRegions_Should_Return_Regions_List_For_Valid_Country_Code()
        {
            // Arrange
            var countryCode = "AR";

            // Act
            var regions = _ISOCodeLookupService.GetRegions(countryCode);

            // Assert
            regions.ShouldNotBeEmpty();
            regions.ShouldContain(r => r.Name == "Entre Ríos" && r.ISOCode == "E");
        }

        [Theory]
        [InlineData("")]
        [InlineData("Z")]
        [InlineData("ZZ")]
        [InlineData("ZZZ")]
        public void GetRegions_Should_Throw_For_Invalid_Country_Code(string CountryCode)
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _ISOCodeLookupService.GetRegions(CountryCode);
            });

            exception.ParamName.ShouldBe("countryCode");
            exception.Message.Contains("country code");
        }
    }
}
