using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViajeHonesto.Destinations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;
using Volo.Abp.Users;

namespace ViajeHonesto.Reviews;

[Authorize]
public class ReviewAppService :
    AbstractKeyCrudAppService<
        Review,
        ReviewDto,
        ReviewKey,
        PagedAndSortedResultRequestDto,
        CreateReviewDto,
        UpdateReviewDto>, IReviewAppService
{

    protected ICurrentUser _currentUser;

    public ReviewAppService(IRepository<Review> repository, ICurrentUser currentUser)
        : base(repository)
    {
        _currentUser = currentUser;
    }

    protected async override Task DeleteByIdAsync(ReviewKey id)
    {
        await Repository.DeleteAsync(d => d.DestinationId == id.DestinationId && d.UserId == id.UserId);
    }

    protected async override Task<Review> GetEntityByIdAsync(ReviewKey id)
    {
        var queryable = await Repository.GetQueryableAsync();
        return await AsyncExecuter.FirstOrDefaultAsync(
            queryable.Where(d => d.DestinationId == id.DestinationId && d.UserId == id.UserId));
    }

    public async override Task<ReviewDto> CreateAsync(CreateReviewDto input)
    {
        var entity = ObjectMapper.Map<CreateReviewDto, Review>(input);
        entity.UserId = _currentUser.GetId();
        await Repository.InsertAsync(entity);

        return ObjectMapper.Map<Review, ReviewDto>(entity);
    }
    public async Task<ReviewDto> UpdateAsync(Guid destinationId, UpdateReviewDto input)
    {
        var userId = _currentUser.GetId();
        var entity = await Repository.FirstOrDefaultAsync(r =>
            r.UserId == userId && r.DestinationId == destinationId);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(Review), $"{userId}-{destinationId}");
        }

        entity.SetRating(new Rating(input.Rating.Value));
        entity.SetComment(input.Comment != null ? new CComment(input.Comment.Comment) : null);

        await Repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Review, ReviewDto>(entity);
    }

    public async Task DeleteAsync(Guid destinationId)
    {
        var userId = _currentUser.GetId();

        var entity = await Repository.FirstOrDefaultAsync(r =>
            r.UserId == userId && r.DestinationId == destinationId);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(Review), $"{userId}-{destinationId}");
        }

        await Repository.DeleteAsync(entity, autoSave: true);
    }
}