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

namespace ViajeHonesto.Reviews;

[Authorize]
public class ReviewAppService :
    AbstractKeyCrudAppService<
        Review,
        ReviewDto,
        ReviewKey,
        PagedAndSortedResultRequestDto,
        CreateReviewDto,
        UpdateReviewDto>
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
}