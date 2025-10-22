using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace ViajeHonesto.Reviews;

public class ReviewAppService : AbstractKeyCrudAppService<Review, ReviewDto, ReviewKey>
{
    public ReviewAppService(IRepository<Review> repository)
        : base(repository)
    {
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

    protected ReviewKey GetKeyAsObject(Review entity)
    {
        return new ReviewKey
        {
            DestinationId = entity.DestinationId,
            UserId = entity.UserId
        };
    }
    public async Task<List<Review>> GetAllReviewsAsync()
    {
        var queryable = await Repository.GetQueryableAsync();
        return await AsyncExecuter.ToListAsync(queryable);
    }
}