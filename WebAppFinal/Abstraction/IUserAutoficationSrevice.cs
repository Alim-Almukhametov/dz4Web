using WebAppFinal.DTO;

namespace WebAppFinal.Abstraction
{
    public interface IUserAutoficationService
    {
        UserDTO Authenticate(LoginDTO login);
    }
}
