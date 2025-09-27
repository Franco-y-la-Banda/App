using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace ViajeHonesto.Destinations;
public abstract class DestinationAppService_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IDestinationAppService _destinationAppService;

    protected DestinationAppService_Tests()
    {
        _destinationAppService = GetRequiredService<IDestinationAppService>();
    }

    [Fact]
    public async Task Should_Get_All_Destinations()
    {
        //Act
        var destinationDtos = await _destinationAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        //Assert
        destinationDtos.TotalCount.ShouldBeGreaterThan(0);
        destinationDtos.Items.ShouldContain(x => x.Name == "Concepción del Uruguay");
    }

    [Fact]
    public async Task Should_Create_A_Valid_Destination()
    {
        //Act
        var result = await _destinationAppService.CreateAsync(
            new CreateUpdateDestinationDto
            {
                Name = "Villa Elisa",
                Country = "Argentina",
                Region = "Entre Ríos",
                Population = 20000
            });

        //Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("Villa Elisa");
    }

    [Fact]
    public async Task Should_Update_A_Destination()
    {
        //Arrange
        var destination = await _destinationAppService.CreateAsync(
            new CreateUpdateDestinationDto
            {
                Name = "Villa Elisa",
                Country = "Argentina",
                Region = "Entre Ríos",
                Population = 20000
            });

        //Act
        var updatedDestination = await _destinationAppService.UpdateAsync(
            destination.Id,
            new CreateUpdateDestinationDto
            {
                Name = "Villa Elisa Updated",
                Country = "Argentina",
                Region = "Entre Ríos",
                Population = 25000
            });

        //Assert
        updatedDestination.Name.ShouldBe("Villa Elisa Updated");
        updatedDestination.Population.ShouldBe(25000);
    }

    [Fact]
    public async Task Should_Delete_A_Destination()
    {
        //Arrange
        var destination = await _destinationAppService.CreateAsync(
            new CreateUpdateDestinationDto
            {
                Name = "Villa Elisa",
                Country = "Argentina",
                Region = "Entre Ríos",
                Population = 20000
            });

        //Act
        await _destinationAppService.DeleteAsync(destination.Id);

        //Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _destinationAppService.GetAsync(destination.Id);
        });
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_Without_Name()
    {
        //Act
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                new CreateUpdateDestinationDto
                {
                    Name = "",
                    Country = "Argentina",
                    Region = "Entre Ríos",
                    Population = 20000
                });
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_Without_Country()
    {
        //Act
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                new CreateUpdateDestinationDto
                {
                    Name = "Villa Elisa",
                    Country = "",
                    Region = "Entre Ríos",
                    Population = 20000
                });
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Country"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_Without_Region()
    {
        //Act
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                new CreateUpdateDestinationDto
                {
                    Name = "Villa Elisa",
                    Country = "Argentina",
                    Region = "",
                    Population = 20000
                });
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Region"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_With_Negative_Population()
    {
        //Act
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                new CreateUpdateDestinationDto
                {
                    Name = "Villa Elisa",
                    Country = "Argentina",
                    Region = "Entre Ríos",
                    Population = -100
                });
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Population"));
    }
}