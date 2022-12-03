using Core.Services;
using Entity.DTOModels;
using Entity.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Implementations
{
    public class UserRepository : IUserRepo

    {
        private readonly ApplicationContext _applicationContext;
        private readonly IValidations _validations;

        public UserRepository(ApplicationContext applicationContext, IValidations validations)
        {
            _applicationContext = applicationContext;
            _validations = validations;
        }
        public bool Login(UserRegistionDto user)
        {
            var oldUser = _applicationContext.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if(oldUser == null || oldUser.Password != user.Password)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Register(UserRegistionDto userRegistionDto)
        {
            if(_validations.IsValidEmail(userRegistionDto.Email) && _validations.IsValidUsername(userRegistionDto.UserName)
                && _validations.IsValidPassword(userRegistionDto.Password) && _validations.IsValidName(userRegistionDto.FirstName)
                && _validations.IsValidName(userRegistionDto.LastName))
            {

                var newUser = new UserModel()
                {
                    FirstName = userRegistionDto.FirstName,
                    LastName = userRegistionDto.LastName,
                    Email = userRegistionDto.Email,
                    Password = userRegistionDto.Password,
                    UserName = userRegistionDto.UserName
                };

                await _applicationContext.AddAsync(newUser);
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            return false;

            
        }

        public async Task <bool> RemoveUser(UserRegistionDto user)
        {
            var removeduser =  await _applicationContext.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);

            if(removeduser == null)
            {
                return false;
            }

            _applicationContext.Remove(removeduser);
            _applicationContext.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateUser(UserRegistionDto user)
        {
           var editedUser = await _applicationContext.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if(editedUser == null)
            {
                return false;
            }
            editedUser.UserName = user.UserName;
            editedUser.LastName = user.LastName;
            editedUser.FirstName = user.FirstName;
            editedUser.Email = user.Email;
            
            _applicationContext.Update(editedUser);
            _applicationContext.SaveChanges();


            return true;
        }

        public async  Task<UserRegistionDto> GetUserById (string id)
        {
            var pickedUser = await _applicationContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if(pickedUser == null)
            {
                return null;
            }
            return new UserRegistionDto
            {
                UserName = pickedUser.UserName,
                LastName = pickedUser.LastName,
                FirstName = pickedUser.FirstName,
                Email = pickedUser.Email
            };

        }

        public async Task<IEnumerable<UserRegistionDto>> GetAllUsers()
        {
            //List<UserModel> users = new List<UserModel>();


            var pickedUser = await _applicationContext.Users.Select(x=> new UserRegistionDto
            {
                UserName = x.UserName,
                LastName = x.LastName,
                FirstName = x.FirstName,
                Email = x.Email
            }).ToListAsync();

            if (pickedUser == null)
                return null;
            return pickedUser;



        }
    }
}
