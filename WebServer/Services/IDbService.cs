﻿namespace WebServer.Services
{
    public interface IDbService<T>
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindById(int id);
        Task<T> FindByUUID(Guid UUID);
        Task<int> Insert(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
        Task<int> DeleteById(int id);
        Task<int> DeleteByUUID(Guid UUID);
    }
}