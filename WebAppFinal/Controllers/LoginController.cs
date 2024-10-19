using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppFinal.Abstraction;
using WebAppFinal.DTO;

namespace WebAppFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        
        /*
         Нужен для хранения конфигурации из appsettings.json
         */
        private readonly IConfiguration _configuration;
        private readonly IUserAutoficationService _userAutoficationSrevice;

        public LoginController(IConfiguration configuration, IUserAutoficationService userAutoficationSrevice)
        {
            _configuration = configuration;
            _userAutoficationSrevice = userAutoficationSrevice;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginDTO loginDTO)
            // атрибут указывает на то что параметр будет получен из тела запроса
        {
            // попытка аутентификации
            var user = _userAutoficationSrevice.Authenticate(loginDTO);
            // я не указываю здесь как-то на репозиторий где реализован метод
            // либо реализация подразумевается через связку метода с любой реализацие
            // либо?
            if (user != null)
            {
                var token = GenToken(user);
                /*
                 Токен генерируется под пользователя, а не под логин/пароль
                 */
                return Ok(token);
            }
            return NotFound("Пользователь не найден");

        }
        // Встроена какая-то защита: при модифиакторе public - сервер
        // выдаёт ошибку 500, а терминал говорит о неоднозначности
        // метода GetToken. Необходим private
        private string GenToken(UserDTO user)
        {
            // ключ извлекается из поля токена JWT из Appsettings. Он нужен для подписи токенов
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // объект принимает ключ и алгоритм для шифрования - используется для подписи токена
            var tokenMeneger = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // сlaims - данные, которые содержат токен: имя пользователя и его роль
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Name),
                 new Claim(ClaimTypes.Role, user.Role.ToString())
             };

            // создание объекта токена 
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],// указывает на создаетля токена
                _configuration["Jwt:Audience"],// кому предназначен токен
                claims, 
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: tokenMeneger);
            // получение строки токена
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public UserDTO Authenticate(LoginDTO login)
        //{
        //    if (login.Name == "string" && login.Password == "string")
        //    {
        //        return new UserDTO { Name = login.Name, Password = login.Password, Role = 0 };
        //    }

        //    if (login.Name == "1" && login.Password == "1")
        //    {
        //        return new UserDTO { Name = login.Name, Password = login.Password, Role = 0 };
        //    }

        //    return null;
        //}
    }
}

