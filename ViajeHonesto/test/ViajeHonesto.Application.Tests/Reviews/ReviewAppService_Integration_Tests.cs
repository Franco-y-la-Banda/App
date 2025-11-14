using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Threading.Tasks;
using ViajeHonesto.Constants;
using ViajeHonesto.Destinations;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
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
        _userId = TestGuids.UserGuid(0);
        _destinationId = TestGuids.DestinationGuid(0);
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
        Login(_userId);
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
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
        // Arrange
        Login(_userId);
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
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
        // Arrange
        Login(_userId);
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = new RatingDto { Value = 10 }, // fuera de rango
            Comment = null
        };

        // Act & Assert
        await Assert.ThrowsAsync<AbpValidationException>(() => _reviewAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Throw_When_Rating_Is_Missing()
    {
        Login(_userId);
        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = null,
            Comment = new CommentDto { Comment = "Comentario sin rating" }
        };

        await Assert.ThrowsAsync<AbpValidationException>(() => _reviewAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Only_Return_Reviews_Of_Current_User()
    {
        var otherUserId = TestGuids.UserGuid(1);

        // Insertar reviews de usuario actual
        Login(_userId);

        await _reviewAppService.CreateAsync(new CreateReviewDto
        {
            DestinationId = TestGuids.DestinationGuid(0),
            Rating = new RatingDto { Value = 5 },
            Comment = new CommentDto { Comment = "Review del usuario actual" }
        });

        await _reviewAppService.CreateAsync(new CreateReviewDto
        {
            DestinationId = TestGuids.DestinationGuid(2),
            Rating = new RatingDto { Value = 4 },
            Comment = new CommentDto { Comment = "Otra review del mismo usuario" }
        });

        // Insertar review de otro usuario
        Login(TestGuids.UserGuid(1));

        await _reviewAppService.CreateAsync(new CreateReviewDto
        {
            DestinationId = TestGuids.DestinationGuid(2),
            Rating = new RatingDto { Value = 3 },
            Comment = new CommentDto { Comment = "Review de otro usuario" }
        });

        // Act
        Login(_userId);
        var result = await _reviewAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        // Assert
        result.Items.ShouldNotBeEmpty();
        result.Items.ShouldAllBe(r => r.UserId == _userId);
    }


    [Fact]
    public async Task Should_Not_Allow_Duplicate_Review_For_Same_User_And_Destination()
    {
        Login(_userId);

        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = new RatingDto { Value = 4 },
            Comment = new CommentDto { Comment = "Primera review" }
        };

        await _reviewAppService.CreateAsync(input);

        var duplicateInput = new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = new RatingDto { Value = 5 },
            Comment = new CommentDto { Comment = "Segunda review duplicada" }
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _reviewAppService.CreateAsync(duplicateInput));
    }

    [Fact]
    public async Task Should_Throw_When_Creating_Review_Without_Authentication()
    {
        _currentUser.Id.Returns((Guid?)null);
        _currentUser.IsAuthenticated.Returns(false);

        var input = new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = new RatingDto { Value = 4 },
            Comment = new CommentDto { Comment = "Intento sin login" }
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reviewAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Throw_When_Updating_Review_Without_Authentication()
    {
        _currentUser.Id.Returns((Guid?)null);
        _currentUser.IsAuthenticated.Returns(false);

        var input = new UpdateReviewDto
        {
            Rating = new RatingDto { Value = 5 },
            Comment = new CommentDto { Comment = "Intento sin login" }
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reviewAppService.UpdateAsync(_destinationId, input));
    }

    [Fact]
    public async Task Should_Throw_When_Deleting_Review_Without_Authentication()
    {
        _currentUser.Id.Returns((Guid?)null);
        _currentUser.IsAuthenticated.Returns(false);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _reviewAppService.DeleteAsync(_destinationId));
    }

    [Fact]
    public async Task Should_Update_Review_When_Valid()
    {
        Login(_userId);

        // Arrange: crear una review inicial
        await _reviewAppService.CreateAsync(new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = new RatingDto { Value = 3 },
            Comment = new CommentDto { Comment = "Comentario inicial" }
        });

        // Act: actualizar la review
        var updated = await _reviewAppService.UpdateAsync(_destinationId, new UpdateReviewDto
        {
            Rating = new RatingDto { Value = 5 },
            Comment = new CommentDto { Comment = "Comentario actualizado" }
        });

        // Assert
        updated.ShouldNotBeNull();
        updated.Rating.Value.ShouldBe(5);
        updated.Comment.Comment.ShouldBe("Comentario actualizado");
        updated.UserId.ShouldBe(_userId);
        updated.DestinationId.ShouldBe(_destinationId);
    }

    [Fact]
    public async Task Should_Delete_Review_When_Exists()
    {
        Login(_userId);

        // Arrange: crear una review
        await _reviewAppService.CreateAsync(new CreateReviewDto
        {
            DestinationId = _destinationId,
            Rating = new RatingDto { Value = 4 },
            Comment = new CommentDto { Comment = "Review a eliminar" }
        });

        // Act: eliminar la review
        await _reviewAppService.DeleteAsync(_destinationId);

        // Assert: intentar obtenerla y verificar que ya no existe
        var result = await _reviewAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        result.Items.ShouldNotContain(r => r.UserId == _userId && r.DestinationId == _destinationId);
    }
}
