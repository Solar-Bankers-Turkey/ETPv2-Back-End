using System.Collections.Generic;
using AutoMapper;
using CustomerService.DataTransferObjects;
using CustomerService.Models;

namespace CustomerService.Profiles {

    public class UserProfile : Profile {

        public UserProfile() {
            CreateMap<User, Register>();
            CreateMap<Register, User>();
            CreateMap<User, UserGeneralOut>();
            CreateMap<UserGeneralOut, User>();
            CreateMap<User, UserGeneralIn>();
            CreateMap<UserGeneralIn, User>();
            CreateMap<Verify, Detail>();
            CreateMap<Detail, Verify>();
        }
    }
}
