using UserProfile.DataBase;
using UserProfile.Interfaces;
using UserProfile.Models;

namespace UserProfile.Services
{
    public class UserService : IUserService
    {

        private readonly UserProfileContext _userProfileContext;

        public UserService(UserProfileContext UserProfileContext){
_userProfileContext =UserProfileContext;
        }

        public async Task<UserModel?> CreateUser(UserModel userModel)
        {
             var users  = _userProfileContext.Users.Add(userModel);
             await _userProfileContext.SaveChangesAsync();
             return await GetUserDetail(userModel.Id)??null;

        }

        public async Task<UserModel?> DeleteUser(int id)
        {
           var user = await GetUserDetail(id);
           if(user is null) return null;

           _userProfileContext.Remove(user);
           await _userProfileContext.SaveChangesAsync();
           return user;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
           var users = _userProfileContext.Users.ToList();
           return await Task.FromResult(users);
        }

        public async Task<UserModel?> GetUserDetail(int id)
        {
           var users = await _userProfileContext.Users.FindAsync(id);
           return users?? null;
        }

        public async Task<UserModel?> UpdateUser(UserModel userModel)
        {
           var user = await GetUserDetail(userModel.Id);
           if(user is null) return null;
           _userProfileContext.Update(userModel);
           await _userProfileContext.SaveChangesAsync();
           return userModel;
        }
    }
}