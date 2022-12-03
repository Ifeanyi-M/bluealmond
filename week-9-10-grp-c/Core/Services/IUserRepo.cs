using Entity.DTOModels;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IUserRepo
    {
        Task<bool> Register(UserRegistionDto userRegistionDto);
        bool Login(UserRegistionDto user);

        Task<bool> RemoveUser(UserRegistionDto user);

       Task <bool> UpdateUser(UserRegistionDto user);

        Task<UserRegistionDto> GetUserById(string id);

        Task<IEnumerable<UserRegistionDto>> GetAllUsers();
    }
}
