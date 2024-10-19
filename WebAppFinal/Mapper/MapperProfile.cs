using AutoMapper;
using WebAppFinal.DTO;
using WebAppFinal.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppFinal.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // используем маппинг. Название свойств entity совпадает с названиями свойств в DTO 
            CreateMap<Role, UserRoleDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<RoleId, LoginDTO>().ReverseMap();
            // ReverseMap для маппирования в обе стороны
        }
    }
}
