using AutoMapper;

namespace SevenWestMediaTechInterview.Configuration
{
    /// <summary>
    /// AutoMapper mapping profile for users.
    /// </summary>
    public class UsersAutoMapperProfile : Profile
    {
        public UsersAutoMapperProfile()
        {
            CreateMap<Client.Dto.User, Models.User>().ForMember(u => u.FullName, opt => opt.Ignore());
        }
    }


}
