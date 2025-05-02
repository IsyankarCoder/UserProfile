 
using Microsoft.EntityFrameworkCore;
using UserProfile.Models;

namespace UserProfile.DataBase

{
    public class UserProfileContext
    :DbContext
    {
        public UserProfileContext(DbContextOptions<UserProfileContext> options)
        :base(options)
        {
            
        }

        public DbSet<UserModel> Users{get;set;}
    }
}