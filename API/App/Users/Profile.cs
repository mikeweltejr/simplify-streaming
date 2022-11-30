using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using DynamoDB.DAL.App.Models;

namespace SimplifyStreaming.API.App.Users
{
    [ExcludeFromCodeCoverage]
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, User>();
        }
    }
}
