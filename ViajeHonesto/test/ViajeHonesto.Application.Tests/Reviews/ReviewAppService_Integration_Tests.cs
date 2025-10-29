using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Threading.Tasks;
using ViajeHonesto.Destinations;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Xunit;

namespace ViajeHonesto.Reviews;

public abstract class ReviewAppService_Integration_Tests<TStartupModule> : ViajeHonestoApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IReviewAppService _reviewAppService;
    protected ICurrentUser _currentUser;
    private readonly Guid _userId;
    private readonly Guid _destinationId;

    protected ReviewAppService_Integration_Tests()
    {
        _reviewAppService = GetRequiredService<IReviewAppService>();
        _userId = Guid.NewGuid();
        _destinationId = Guid.NewGuid();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        //Mock the current user
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    private void Login(Guid userId)
    {
        _currentUser.Id.Returns(userId);
        _currentUser.IsAuthenticated.Returns(true);
    }

    [Fact]
    public async Task Should_Create_Review_When_Valid()
    {
        // Insertar destino relacionado
        var destinoRepo = GetRequiredService<IRepository<Destination, Guid>>();
        await destinoRepo.InsertAsync(new Destination(_destinationId)
        {
            Name = "Test name",
            Country = "Test country",
            Region = "Test region",
            Population = 100,
            Coordinate = new Coordinate(10, 20)
        });

        // Insertar usuario relacionado
        var userRepo = GetRequiredService<IRepository<IdentityUser, Guid>>();
        await userRepo.InsertAsync(new IdentityUser(_userId, "test-user", "testuser@email.com"));

        // Arrange
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            UserId = _userId,
            Rating = new RatingDto { Value = 5 },
            Comment = new CommentDto { Comment = "Excelente destino" }
        };

        // Act
        var result = await _reviewAppService.CreateAsync(input);

        // Assert
        result.ShouldNotBeNull();
        result.Rating.Value.ShouldBe(5);
        result.Comment.Comment.ShouldBe("Excelente destino");
        result.UserId.ShouldBe(_userId);
        result.DestinationId.ShouldBe(_destinationId);
    }

    [Fact]
    public async Task Should_Create_Review_With_Only_Rating()
    {
        // Insertar destino
        var destinoRepo = GetRequiredService<IRepository<Destination, Guid>>();
        await destinoRepo.InsertAsync(new Destination(_destinationId)
        {
            Name = "Solo rating",
            Country = "Chile",
            Region = "Valparaíso",
            Population = 200,
            Coordinate = new Coordinate(1, 1)
        });

        // Insertar usuario
        var userRepo = GetRequiredService<IRepository<IdentityUser, Guid>>();
        await userRepo.InsertAsync(new IdentityUser(_userId, "solo-rating", "rating@email.com"));

        // Arrange
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            UserId = _userId,
            Rating = new RatingDto { Value = 3 },
            Comment = null
        };

        // Act
        var result = await _reviewAppService.CreateAsync(input);

        // Assert
        result.ShouldNotBeNull();
        result.Rating.Value.ShouldBe(3);
        result.Comment.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Throw_When_Rating_Is_Out_Of_Range()
    {
        // Insertar destino
        var destinoRepo = GetRequiredService<IRepository<Destination, Guid>>();
        await destinoRepo.InsertAsync(new Destination(_destinationId)
        {
            Name = "Rating inválido",
            Country = "Paraguay",
            Region = "Asunción",
            Population = 400,
            Coordinate = new Coordinate(3, 3)
        });

        // Insertar usuario
        var userRepo = GetRequiredService<IRepository<IdentityUser, Guid>>();
        await userRepo.InsertAsync(new IdentityUser(_userId, "rating-invalido", "invalido@email.com"));

        // Arrange
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            UserId = _userId,
            Rating = new RatingDto { Value = 10 }, // fuera de rango
            Comment = null
        };

        // Act & Assert
        await Assert.ThrowsAsync<AbpValidationException>(() => _reviewAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Throw_When_Rating_Is_Missing()
    {
        // Insertar destino
        var destinoRepo = GetRequiredService<IRepository<Destination, Guid>>();
        await destinoRepo.InsertAsync(new Destination(_destinationId)
        {
            Name = "Rating inválido",
            Country = "Paraguay",
            Region = "Asunción",
            Population = 400,
            Coordinate = new Coordinate(3, 3)
        });

        // Insertar usuario
        var userRepo = GetRequiredService<IRepository<IdentityUser, Guid>>();
        await userRepo.InsertAsync(new IdentityUser(_userId, "rating-invalido", "invalido@email.com"));


        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            UserId = _userId,
            Rating = null, // ❌ rating ausente
            Comment = new CommentDto { Comment = "Comentario sin rating" }
        };

        await Assert.ThrowsAsync<AbpValidationException>(() => _reviewAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Get_Reviews_For_User()
    {
        // Insertar destino
        var destinoRepo = GetRequiredService<IRepository<Destination, Guid>>();
        await destinoRepo.InsertAsync(new Destination(_destinationId)
        {
            Name = "Destino para lectura",
            Country = "Argentina",
            Region = "Cuyo",
            Population = 500,
            Coordinate = new Coordinate(5, 5)
        });

        // Insertar usuario
        var userRepo = GetRequiredService<IRepository<IdentityUser, Guid>>();
        await userRepo.InsertAsync(new IdentityUser(_userId, "lector", "lector@email.com"));

        Login(_userId);

        // Insertar review
        await _reviewAppService.CreateAsync(new CreateReviewDto
        {
            DestinationId = _destinationId,
            UserId = _userId,
            Rating = new RatingDto { Value = 5 },
            Comment = new CommentDto { Comment = "Excelente destino" }
        });

        // Act
        var result = await _reviewAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        // Assert
        result.Items.ShouldNotBeEmpty();
        result.Items.ShouldContain(r => r.UserId == _userId && r.DestinationId == _destinationId);
    }
}
