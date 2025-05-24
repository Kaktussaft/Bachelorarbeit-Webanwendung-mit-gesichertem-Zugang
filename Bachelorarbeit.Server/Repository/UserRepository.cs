using Bachelorarbeit.Server.Interfaces;
using Bachelorarbeit.Server.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Bachelorarbeit.Server.Repository;

public class UserRepository : IUserRepository
{
    private readonly MyDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(MyDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<UserModel> GetByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if(user == null)
            return null;

        return _mapper.Map<UserModel>(user);
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var users = await _dbContext.Users.ToListAsync();
        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task<UserModel> CreateAsync(UserModel user)
    {
        var newUser = await _dbContext.AddAsync(user);
        return _mapper.Map<UserModel>(newUser.Entity);
    }

    public async Task<bool> UpdateAsync(UserModel newUser)
    {
        var oldUser = await _dbContext.Users.FindAsync(newUser.Id);
        if (oldUser == null)
            return false;
    
        _mapper.Map(newUser, oldUser); 
        _dbContext.Update(oldUser);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<UserModel> DeleteAsync(Guid id)
    {
        var userToDelete = await _dbContext.Users.FindAsync(id);
        if (userToDelete == null)
            return null;

        var result = _dbContext.Remove(userToDelete);
        return _mapper.Map<UserModel>(result.Entity);
    }

    public async Task<UserModel> GetByUsernameAsync(string username)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return null;
        
        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return null;
        
        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> GetUserByRefreshTokenAsync(byte[] refreshToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null)
            return null;
        
        return _mapper.Map<UserModel>(user);
    }
    
    
    public async Task SaveChangesAsync()
    {
         await _dbContext.SaveChangesAsync();
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

}