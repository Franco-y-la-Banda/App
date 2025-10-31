using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ViajeHonesto.Reviews;

public interface IReviewAppService :
    ICrudAppService<                    //Defines CRUD methods
        ReviewDto,                      //Used to show Reviews
        ReviewKey,                      //Primary key of the Review entity
        PagedAndSortedResultRequestDto, //Used for sorting
        CreateReviewDto,                //Used to create a Review
        UpdateReviewDto>                //Used to update a Review
{
    public Task<ReviewDto> UpdateAsync(Guid destinationId, UpdateReviewDto input);
    public Task DeleteAsync(Guid destinationId);
}