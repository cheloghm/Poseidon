using AutoMapper;
using Poseidon.Models;
using Poseidon.DTOs;
using Poseidon.Enums;
using Poseidon.ViewModels;
using MongoDB.Bson;

namespace Poseidon.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))  // Map Enum to String
                .ReverseMap()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<Role>(src.Role))); // Map String to Enum

            CreateMap<User, CreateUserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))  // Map Enum to String
                .ReverseMap()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<Role>(src.Role)));

            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<CreateUserDTO, User>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();


            // Passenger Mappings
            CreateMap<Passenger, PassengerDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())) // Convert ObjectId to string
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(src.Id))); // Convert string to ObjectId

            CreateMap<Passenger, PassengerViewModel>().ReverseMap();
            CreateMap<PassengerDTO, Passenger>().ReverseMap();
        }
    }
}
