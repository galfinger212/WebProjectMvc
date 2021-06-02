using Dal;
using Models;
using System.Linq;

namespace WebProject.Services
{
    public class UsersServiceSql : IUsersService
    {
		private MyContext context;
		public UsersServiceSql(MyContext context) => this.context = context;
        public bool AddUser(UserModel user)
        {
            if(context.Users.All((u) => u.UserName!=user.UserName))
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool LogIn(UserModel user)
        {
            UserModel userLogIn = context.Users.Where((u) => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password)).SingleOrDefault();
            return userLogIn != null;
        }
        public void UpdateUser(UserModel user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }
    }
}