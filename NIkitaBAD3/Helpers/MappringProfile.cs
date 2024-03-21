using AutoMapper;
using NIkitaBAD3.Models;

namespace NIkitaBAD3.Helpers
{
    public class MappringProfile : Profile
    {
        public MappringProfile()
        {
            CreateMap<UserRegistrationModel, User>().ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

        }
    }
}
