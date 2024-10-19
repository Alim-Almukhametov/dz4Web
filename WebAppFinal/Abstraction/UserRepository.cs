using System.Text;
using WebAppFinal.DB;
using WebAppFinal.DTO;
using WebAppFinal.Models;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebAppFinal.Abstraction
{
    public class UserRepository : IUserRepository//, IUserAutoficationSrevice
    {
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        private readonly UserContext _context;

        public int AddUser(UserDTO userDTO)
        {
            using (var context = new UserContext())
            {
                if (context.Users.Any(x => x.Name == userDTO.Name))
                    throw new Exception("User is already exist");

                if (userDTO.Role == UserRoleDTO.Admin)
                    if (context.Users.Any(x => x.RoleId == RoleId.Admin))
                        throw new Exception("Admin is already exist");

                var entity = new User() { Name = userDTO.Name, RoleId = (RoleId)userDTO.Role };



                entity.Salt = new byte[16];
                new Random().NextBytes(entity.Salt);
                var data = Encoding.UTF8.GetBytes(userDTO.Password).Concat(entity.Salt).ToArray();

                entity.Password = new SHA512Managed().ComputeHash(data);
                //entity.Password = new SHA512Managed().ComputeHash(data);

                context.Users.Add(entity);
                context.SaveChanges();

                return entity.Id;
            }
        }



        public RoleId CheckUser(LoginDTO loginDTO)
        {
            using (var context = new UserContext())
            {
               var user = context.Users.FirstOrDefault(x => x.Name == loginDTO.Name);

                if (user == null)
                    throw new Exception("No user like this!");

                var data = Encoding.UTF8.GetBytes(loginDTO.Password).Concat(user.Salt).ToArray();
                var hash = new SHA512Managed().ComputeHash(data);

                if (user.Password.SequenceEqual(hash))
                    return (RoleId)user.RoleId;
                //return user.RoleId;

                throw new Exception($"Wrong password!");
                //throw new Exception($"Wrong password!");
            }
        }

        public string DeleteUser(LoginDTO loginDTO) 
        {
            using (var context = new UserContext()) 
            {
                if (context.Users.Any(u=>u.Name==loginDTO.Name))
                {

                    context.Users.Where(u => u.Name == loginDTO.Name).ExecuteDelete();
                    return $"Пользователь с ником {loginDTO.Name} - удалён";
                }
                throw new Exception("пользователь не найден");
            }
        }

        public string AddUserTest(UserDTO user)
        {
            using (var context = new UserContext()) 
            {
                var data = user.Password.ToArray();
                var bytes = Encoding.UTF8.GetBytes(data);

                string pass = "string";

                var data2 = pass.ToArray();
                var bytes2 = Encoding.UTF8.GetBytes(data2);

                int count = 0;

                var hash1 = new SHA512Managed().ComputeHash(bytes);
                var hash2 = new SHA512Managed().ComputeHash(bytes2);

                //if (bytes == bytes2) return "OK";
                //for (int i = 0; i < data.Length; i++)
                //{
                //    if (data[i] == data2[i])
                //        count++;
                //}
                //return $"{count}, {data.Length}";
                //if (data.Equals(data2) ) return "OK";
                if (hash1.SequenceEqual(hash2)) return "OK";
                //if (user.Password == pass) return "OK";
                return "Not Ok";
            }

            
        }

    }
}
