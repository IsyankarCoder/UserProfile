using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserProfile.Models;

namespace UserProfile.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel?> GetUserDetail(int id);
        
        Task<UserModel?> CreateUser(UserModel userModel);
        Task<UserModel?> UpdateUser(UserModel userModel); 
        Task<UserModel?> DeleteUser(int id);

    }
}