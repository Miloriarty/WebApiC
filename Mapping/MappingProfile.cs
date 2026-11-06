using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleDto>();

        CreateMap<User, UserDto>();
        CreateMap<User, UserShortDto>();

        CreateMap<Category, CategoryDto>();

        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductShortDto>();

        CreateMap<CartItem, CartItemDto>();
        CreateMap<Cart, CartDto>()
            .ForMember(dest => 
                dest.User, opt => 
                opt.MapFrom(src => src.IdUserNavigation))
            .ForMember(dest => 
                dest.Items, opt
                => opt.MapFrom(src => src.CartItems));

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Order, OrderDto>()
            .ForMember(dest =>
                dest.User, opt => 
                opt.MapFrom(src => src.IdUserNavigation))
            .ForMember(dest => dest.Items,
                opt => 
                    opt.MapFrom(src => src.OrderItems));

        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => 
                dest.IsActive, opt
                => opt.MapFrom(src => src.ReleasedAt == null));


        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();

        CreateMap<CreateUserDto, User>()
            .ForMember(dest => 
                dest.Role, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Role,
                opt => opt.Ignore());

        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Category,
                opt => opt.Ignore());
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Category,
                opt => opt.Ignore());

        CreateMap<CreateCartDto, Cart>()
            .ForMember(dest => dest.IdUserNavigation,
                opt => opt.Ignore());
        CreateMap<UpdateCartDto, Cart>()
            .ForMember(dest => dest.IdUserNavigation,
                opt => opt.Ignore());

        CreateMap<CreateCartItemDto, CartItem>()
            .ForMember(dest => dest.Cart,
                opt => opt.Ignore())
            .ForMember(dest => dest.Product,
                opt => opt.Ignore());
        CreateMap<UpdateCartItemDto, CartItem>()
            .ForMember(dest => dest.Cart,
                opt => opt.Ignore())
            .ForMember(dest => dest.Product,
                opt => opt.Ignore());

        CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.IdUserNavigation,
                opt => opt.Ignore());
        CreateMap<UpdateOrderDto, Order>()
            .ForMember(dest => dest.IdUserNavigation,
                opt => opt.Ignore());

        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Order,
                opt => opt.Ignore())
            .ForMember(dest => dest.Product,
                opt => opt.Ignore());
        CreateMap<UpdateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Order,
                opt => opt.Ignore())
            .ForMember(dest => dest.Product,
                opt => opt.Ignore());

        CreateMap<CreateReservationDto, Reservation>()
            .ForMember(dest => dest.User,
                opt => opt.Ignore())
            .ForMember(dest => dest.Product,
                opt => opt.Ignore());
    }
}
