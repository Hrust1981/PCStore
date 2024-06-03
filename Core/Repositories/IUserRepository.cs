﻿using Core.Entities;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        User Get(string login);
        List<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(string login);
    }
}
