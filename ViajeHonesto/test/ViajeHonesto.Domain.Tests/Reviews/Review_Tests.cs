using Shouldly;
using System;
using Xunit;

namespace ViajeHonesto.Reviews.Tests;
public class Review_Tests
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _destinationId = Guid.NewGuid();

    [Fact]
    public void Should_Create_Review_With_Valid_Data()
    {
        var rating = new Rating(4);
        var comment = new CComment("Muy bueno");

        var review = new Review(_destinationId, _userId, rating, comment);

        review.UserId.ShouldBe(_userId);
        review.DestinationId.ShouldBe(_destinationId);
        review.Rating.Value.ShouldBe(4);
        review.Comment.Comment.ShouldBe("Muy bueno");
    }

    [Fact]
    public void Should_Update_Rating_When_SetRating_Is_Called()
    {
        var review = new Review(_destinationId, _userId, new Rating(3), null);

        review.SetRating(new Rating(5));

        review.Rating.Value.ShouldBe(5);
    }

    [Fact]
    public void Should_Update_Comment_When_SetComment_Is_Called()
    {
        var review = new Review(_destinationId, _userId, new Rating(3), null);

        review.SetComment(new CComment("Actualizado"));

        review.Comment.Comment.ShouldBe("Actualizado");
    }

    [Fact]
    public void Should_Allow_Null_Comment_When_SetComment_Is_Called()
    {
        var review = new Review(_destinationId, _userId, new Rating(3), new CComment("Inicial"));

        review.SetComment(null);

        review.Comment.ShouldBeNull();
    }
}
