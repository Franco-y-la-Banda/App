using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ViajeHonesto.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ViajeHonesto.Destinations;
public abstract class DestinationAppService_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IDestinationAppService _destinationAppService;
    private readonly IDbContextProvider<ViajeHonestoDbContext> _dbContextProvider;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    protected DestinationAppService_Tests()
    {
        _destinationAppService = GetRequiredService<IDestinationAppService>();
        _dbContextProvider = GetRequiredService<IDbContextProvider<ViajeHonestoDbContext>>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }


    private readonly CreateUpdateDestinationDto validDestination = new()
    {
        WikiDataId = "Q52581",
        Name = "Villa Elisa",
        Country = "Argentina",
        Region = "Entre Ríos",
        Population = 20000,
        Coordinate = new CoordinateDto
        {
            Latitude = -32.16f,
            Longitude = -58.44f
        },
        Photos = new DestinationPhotoDto[]
        {
            new DestinationPhotoDto
            {
                PhotoId = Guid.NewGuid(),
                Path = "https://example.com/photo1.jpg"
            },
            new DestinationPhotoDto
            {
                PhotoId = Guid.NewGuid(),
                Path = "https://example.com/photo2.jpg"
            }
        }
    };

    [Fact]
    public async Task Should_Get_All_Destinations()
    {
        //Act
        var destinationDtos = await _destinationAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        //Assert
        destinationDtos.TotalCount.ShouldBeGreaterThan(0);
        destinationDtos.Items.ShouldContain(x => x.Name == "Concepción del Uruguay");
        destinationDtos.Items.ShouldContain(x => x.Name == "Puerto Madero");
    }

    [Fact]
    public async Task Should_Persist_A_Valid_Destination_In_Database()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            // Arrange
            var destination = validDestination;

            // Act
            var result = await _destinationAppService.CreateAsync(destination);

            // Assert
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var savedDestination = await dbContext.Destinations.FindAsync(result.Id);

            savedDestination.ShouldNotBeNull();
            savedDestination.WikiDataId.ShouldBe(destination.WikiDataId);
            savedDestination.Name.ShouldBe(destination.Name);
            savedDestination.Country.ShouldBe(destination.Country);
            savedDestination.Population.ShouldBe(destination.Population);
            savedDestination.Coordinate.Latitude.ShouldBe(destination.Coordinate.Latitude);
            savedDestination.Coordinate.Longitude.ShouldBe(destination.Coordinate.Longitude);
            savedDestination.Photos.Count.ShouldBe(2);
            savedDestination.Photos.ShouldContain(p => p.Path == "https://example.com/photo1.jpg");
            savedDestination.Photos.ShouldContain(p => p.Path == "https://example.com/photo2.jpg");
        };
    }

    [Fact]
    public async Task Should_Update_A_Destination()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            // Arrange
            var savedDestination = await _destinationAppService.CreateAsync(validDestination);
            var dbContext = await _dbContextProvider.GetDbContextAsync();

            var updatedDestination = new CreateUpdateDestinationDto
            {
                WikiDataId = "Q1234",
                Name = savedDestination.Name + " Updated",
                Country = savedDestination.Country + " Updated",
                Region = savedDestination.Region + " Updated",
                Population = savedDestination.Population + 1000,
                Coordinate = new CoordinateDto
                {
                    Latitude = -savedDestination.Coordinate.Latitude,
                    Longitude = -savedDestination.Coordinate.Longitude
                },
                Photos = new DestinationPhotoDto[]
                {
                    new DestinationPhotoDto
                    {
                        PhotoId = savedDestination.Photos[0].PhotoId,
                        Path = savedDestination.Photos[0].Path + "Updated"
                    }
                }
            };

            // Act
            await _destinationAppService.UpdateAsync(savedDestination.Id, updatedDestination);

            // Assert
            var savedUpdatedDestination = await dbContext.Destinations.FindAsync(savedDestination.Id);

            savedUpdatedDestination.ShouldNotBeNull();
            savedUpdatedDestination.WikiDataId.ShouldBe(updatedDestination.WikiDataId);
            savedUpdatedDestination.Name.ShouldBe(updatedDestination.Name);
            savedUpdatedDestination.Country.ShouldBe(updatedDestination.Country);
            savedUpdatedDestination.Population.ShouldBe(updatedDestination.Population);
            savedUpdatedDestination.Coordinate.Latitude.ShouldBe(updatedDestination.Coordinate.Latitude);
            savedUpdatedDestination.Coordinate.Longitude.ShouldBe(updatedDestination.Coordinate.Longitude);
            savedUpdatedDestination.Photos.Count.ShouldBe(1);
            savedUpdatedDestination.Photos.ShouldContain(p => p.PhotoId == savedDestination.Photos[0].PhotoId);
            savedUpdatedDestination.Photos.ShouldContain(p => p.Path == savedDestination.Photos[0].Path + "Updated");
        }
    }

    [Fact]
    public async Task Should_Delete_A_Destination()
    {
        var dest = validDestination;
        //Arrange
        var destination = await _destinationAppService.CreateAsync(dest);

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
        var destination = validDestination;
        destination.Name = "";

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                destination);
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Name"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_Without_Country()
    {
        //Act
        var destination = validDestination;
        destination.Country = "";

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                destination);
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Country"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_Without_Region()
    {
        //Act
        var destination = validDestination;
        destination.Region = "";

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                destination);
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Region"));
    }

    [Fact]
    public async Task Should_Not_Create_A_Destination_With_Negative_Population()
    {
        //Act
        var destination = validDestination;
        destination.Population = -100;

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                destination);
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("Population"));
    }

    [Theory]
    [InlineData("Q123")]
    [InlineData("q456")]
    [InlineData("Q1")]
    public async Task Should_Create_A_Destination_With_Valid_WikiDataId(string wikiDataId)
    {
        // Arrange
        var destination = validDestination;
        destination.WikiDataId = wikiDataId;

        // Act

        using (var uow = _unitOfWorkManager.Begin())
        {
            // Act
            var result = await _destinationAppService.CreateAsync(destination);

            // Assert
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var savedDestination = await dbContext.Destinations.FindAsync(result.Id);

            savedDestination.ShouldNotBeNull();
            savedDestination.WikiDataId.ShouldBe(wikiDataId.ToUpper());
            savedDestination.Name.ShouldBe(destination.Name);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Should_Create_A_Destination_Without_WikiDataId(string? wikiDataId)
    {
        // Arrange
        var destination = validDestination;
        destination.WikiDataId = wikiDataId;

        using (var uow = _unitOfWorkManager.Begin())
        {
            // Act
            var result = await _destinationAppService.CreateAsync(destination);

            // Assert
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            var savedDestination = await dbContext.Destinations.FindAsync(result.Id);

            savedDestination.ShouldNotBeNull();
            savedDestination.WikiDataId.ShouldBeNull();
            savedDestination.Name.ShouldBe(destination.Name);
        }
    }

    [Theory]
    [InlineData("123")]
    [InlineData("QWE")]
    [InlineData("Q012")]
    [InlineData("Q123a")]
    public async Task Should_Not_Create_A_Destination_With_Invalid_WikiDataId(string? wikiDataId)
    {
        // Arrange
        var destination = validDestination;
        destination.WikiDataId = wikiDataId;

        // Act

        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _destinationAppService.CreateAsync(
                destination);
        });

        //Assert
        exception.ValidationErrors.Count.ShouldBe(1);
        exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains("WikiDataId"));
    }
}