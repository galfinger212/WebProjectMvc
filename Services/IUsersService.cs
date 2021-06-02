using Models;
namespace WebProject.Services
{
    public interface IUsersService
    {
        bool AddUser(UserModel user);
        bool LogIn(UserModel user);
        void UpdateUser(UserModel user);
    }
}