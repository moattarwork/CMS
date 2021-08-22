using AutoMapper;
using Emporos.Core.Domain;
using Emporos.Core.Dto;

namespace Emporos.Core.Profiles
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>(MemberList.Source).ReverseMap();
        }
    }
}