using AutoMapper;
using Order.API.Dtos;
using Order.Domain.Entities;

namespace Order.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Order, OrderDto>().ReverseMap();
            CreateMap<Domain.Entities.OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
