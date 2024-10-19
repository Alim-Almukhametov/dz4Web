using System.Text;
using WebAppFinal.DB;
using WebAppFinal.DTO;
using WebAppFinal.Models;
using System.Security.Cryptography;

namespace WebAppFinal.Abstraction
{
    public class UserAutoficationService : IUserAutoficationService
    {
        private readonly UserContext _context;

        public UserAutoficationService(UserContext context)
        { _context = context; }

        public UserDTO Authenticate(LoginDTO login)
        {
            using (_context)
            {

                if (_context.Users.Any(x => x.Name == login.Name))
                {
                    var user = _context.Users.FirstOrDefault(x => x.Name == login.Name);

                    var passwordCompare = Encoding.UTF8.GetBytes(login.Password).Concat(user.Salt).ToArray();
                    var hash = new SHA512Managed().ComputeHash(passwordCompare);

                    if (hash.SequenceEqual( user.Password))
                    {
                        return new UserDTO { Name = login.Name, Password = login.Password, Role = 0 };
                    }
                    else 
                    {
                        throw new Exception("Ошибка сравнения");
                    }
                }

                throw new Exception("Пользователь не найден");
            }
        }

    }
}
