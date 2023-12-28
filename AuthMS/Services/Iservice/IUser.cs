using AuthMS.Data.Dtos;

namespace AuthMS.Services.Iservice
{
    public interface IUser
    {
        Task<string> RegisterUser(RegisterUserDTO newuser);

        // I will look into the perfomance issues .
        Task<string> LoginUser(LoginRequestDTO user);
        Task<bool> AssignUserRole(AssignRoleDTO assign);
        Task<UserDTO> GetUserById(Guid userId);

        Task<UserDTO> GetUserByEmail(string Email);
    }
}
