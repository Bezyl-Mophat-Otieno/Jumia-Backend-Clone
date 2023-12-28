using AuthMS.Data.Dtos;
using AuthMS.Models;
using AutoMapper;

namespace AuthMS.Profiles
{
    public class AuthProfile:Profile
    {
        public AuthProfile()
        {
            // Map the Register user to the Application User which extends the Identity User
            // , the username inside the Identity User should have it's value from Register User's Email
            CreateMap<RegisterUserDTO, ApplicationUser>().
                ForMember(destination=>destination.UserName , source=> source.MapFrom(user=>user.Email));

            CreateMap<ApplicationUser , UserDTO>().ReverseMap();
        }
    }
}
