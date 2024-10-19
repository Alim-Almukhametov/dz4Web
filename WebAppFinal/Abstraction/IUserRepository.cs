using WebAppFinal.DTO;
using WebAppFinal.Models;

namespace WebAppFinal.Abstraction
{
    public interface IUserRepository
    {
        int AddUser(UserDTO userDTO);
        string DeleteUser(LoginDTO userDTO);
        string AddUserTest(UserDTO user);

        RoleId CheckUser(LoginDTO loginDTO);
    }
}
