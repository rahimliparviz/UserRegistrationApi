using System;
using System.Collections.Generic;
using Core.DTO;
using Entities;

namespace Core.Repositories
{
    public interface IUserRegistrationRepository
    {
        IEnumerable<User> GetAll();
        bool Delete(Guid id);
        bool Create(UserDto user);
    }
}