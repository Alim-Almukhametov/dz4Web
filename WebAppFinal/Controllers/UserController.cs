using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppFinal.Abstraction;
using WebAppFinal.DTO;
using WebAppFinal.Models;

namespace WebAppFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _config = configuration;
        }

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;



        [HttpPost("AddUser")]
        public ActionResult<int> AddUser(UserDTO userDTO)
        {
            try
            {
                //return Ok(_userRepository.AddUser(userDTO));
                return Ok(_userRepository.AddUser(userDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(409, ex.Message);
            }
        }


        [HttpPost("Test")]
        public string AddUserTest(UserDTO userDTO)
        {
            return _userRepository.AddUserTest(userDTO);
            //return userDTO.Name;

        }


        [HttpPost("CheckUser")]
        public ActionResult<string> CheckUser(LoginDTO loginDTO)
        {
            try
            {
                var roleId = _userRepository.CheckUser(loginDTO);
                return Ok($"Проверка пользователя прошла успешно - номер роли: {roleId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        } 
        
        
        [HttpDelete("DeleteUser")]
        public ActionResult<string> DeeleteUser(LoginDTO loginDTO)
        {
            try
            {
                return Ok(_userRepository.DeleteUser(loginDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }




       



        //[HttpPost("login")]
        //public ActionResult<string> Login(LoginDTO loginDTO)
        //{
        //    var user = _userRepository.CheckUser(loginDTO.Name, loginDTO.Password);

        //    if (user == null)
        //        return Unauthorized();

        //    var token = GenerateJwtToken(user);
        //    return Ok(token);
        //}
    }
}
