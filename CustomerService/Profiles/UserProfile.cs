using AutoMapper;
using CustomerService.DataTransferObjects;
using CustomerService.Models;

namespace CustomerService.Profiles {
    public class UserProfile : Profile {

        public UserProfile() {
            CreateMap<User, RegisterFirstStep>();
            CreateMap<RegisterFirstStep, User>();
        }
    }
}
