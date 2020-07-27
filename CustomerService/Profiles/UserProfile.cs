using AutoMapper;
using CustomerService.DataTransferObjects;
using CustomerService.Models;

namespace CustomerService.Profiles {

    public class UserProfile : Profile {

        public UserProfile() {

            CreateMap<User, Register>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<Register, User>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<User, UserGeneralOut>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<UserGeneralOut, User>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<User, UserGeneralIn>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);

            });
            CreateMap<UserGeneralIn, User>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<Verify, Detail>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<Detail, Verify>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<Detail, Detail>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });

            CreateMap<Address, Address>().ForAllMembers(opt => {
                opt.Condition((src, dest, sourceMember) => sourceMember != null);
            });
        }
    }
}
