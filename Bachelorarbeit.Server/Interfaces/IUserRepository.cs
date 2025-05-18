using Bachelorarbeit.Server.Models;

namespace Bachelorarbeit.Server.Interfaces;

public interface IUserRepository
{
    Task<UserModel> GetByIdAsync(Guid id);
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<UserModel> CreateAsync(UserModel user);
    Task<UserModel> UpdateAsync(UserModel user);
    Task<UserModel> DeleteAsync(Guid id);
    Task<UserModel> GetByUsernameAsync(string username);
    Task<UserModel> GetByEmailAsync(string email);
}