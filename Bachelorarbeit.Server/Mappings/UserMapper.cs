using Bachelorarbeit.Server.Models;
using Bachelorarbeit.Server.Repository.Entities;
using AutoMapper;

namespace Bachelorarbeit.Server.Mappings;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserModel, UserEntity>();
        CreateMap<UserEntity, UserModel>();
    }

}