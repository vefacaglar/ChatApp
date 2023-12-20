﻿namespace ChatApp.Infrastructure.Repositories.Abstractions
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
