using AutoMapper;
using CustomerService.DataTransferObjects.General;
using CustomerService.DataTransferObjects.Registration;
using CustomerService.DataTransferObjects.Update;
using CustomerService.DataTransferObjects.Verify;
using CustomerService.Models;

namespace CustomerService.Profiles {

    public class UserProfile : Profile {

        public UserProfile() {

            // out objects maps
            CreateMap<User, GeneralOut>()
                .ForMember(x => x.idString, opt => opt.MapFrom(o => Utils.RepositoryUtils.getVal(o, "Id")))
                .ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<User, RegisterOut>()
                .ForMember(x => x.idString, opt => opt.MapFrom(o => Utils.RepositoryUtils.getVal(o, "Id")))
                .ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });

            CreateMap<VerifyIn, Info>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<UpdateIn, User>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<RegisterIn, User>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<Settings, Settings>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<Info, Info>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<UpdateInfo, Info>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<Info, UpdateInfo>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<Address, Address>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<UpdateAddress, Address>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<Address, UpdateAddress>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
            CreateMap<Notification, Notification>().ForAllMembers(opt => { opt.Condition((src, dest, sourceMember) => sourceMember != null); });
        }
    }
}
