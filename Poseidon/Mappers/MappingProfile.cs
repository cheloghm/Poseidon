using AutoMapper;
using Poseidon.Models;
using Poseidon.DTOs;
using Poseidon.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Poseidon.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();

            // Passenger Mappings
            CreateMap<Passenger, PassengerDTO>().ReverseMap();
            CreateMap<Passenger, PassengerViewModel>().ReverseMap();
        }
    }
}
