using ViajeHonesto.Destinations;
using AutoMapper;

namespace ViajeHonesto;

public class ViajeHonestoApplicationAutoMapperProfile : Profile
{
    public ViajeHonestoApplicationAutoMapperProfile()
    {
        CreateMap<Destination, DestinationDto>();
        CreateMap<CreateUpdateDestinationDto, Destination>();

        CreateMap<Coordinate, CoordinateDto>();
        CreateMap<CoordinateDto, Coordinate>();
    }
}
