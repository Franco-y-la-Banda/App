using ViajeHonesto.Destinations;
using ViajeHonesto.Reviews;
using AutoMapper;

namespace ViajeHonesto;

public class ViajeHonestoApplicationAutoMapperProfile : Profile
{
    public ViajeHonestoApplicationAutoMapperProfile()
    {
        CreateMap<Destination, DestinationDto>();
        CreateMap<CreateUpdateDestinationDto, Destination>();
        CreateMap<Destination, CityDetailsDto>();

        CreateMap<Coordinate, CoordinateDto>();
        CreateMap<CoordinateDto, Coordinate>();

        CreateMap<DestinationPhoto, DestinationPhotoDto>();
        CreateMap<DestinationPhotoDto, DestinationPhoto>();

        CreateMap<Review, ReviewDto>();        
        CreateMap<CreateReviewDto, Review>();
        CreateMap<UpdateReviewDto, Review>();
        
        CreateMap<Rating, RatingDto>();
        CreateMap<RatingDto, Rating>();

        CreateMap<CComment, CommentDto>();
        CreateMap<CommentDto, CComment>();       
    }
}
