using Works_API.Models.Domain;

namespace Works_API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            new User()
            {
                FirstName = "David", LastName = "Mboko", EmailAddress = "meekdavid@yahoo.com", Id = Guid.NewGuid(), Username = "Meekdavid", Password = "Meekdavid",
                Roles = new List<string> {"reader" }
            },

            new User()
            {
                 FirstName = "Daniel", LastName = "Mboko", EmailAddress = "meekdaniel@yahoo.com", Id = Guid.NewGuid(), Username = "Meekdaniel", Password = "Meekdaniel",
                Roles = new List<string> {"reader", "writer"}
            }
        };
        public async Task<User> AuthenticateAsync(string username, string password)   
        {
            var user = Users.Find(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Password == password);
            if (user == null)
            {
                return user;
            }
            return user;
        }
    }
}
ss