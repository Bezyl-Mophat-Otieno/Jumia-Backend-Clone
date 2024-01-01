using TransactionMS.Data.Dtos;

namespace TransactionMS.Services.Iservice
{
    public interface IUser
    {
        Task<UserDTO> GetUserById(Guid id);
    }
}
