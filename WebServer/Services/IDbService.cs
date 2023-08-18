namespace WebServer.Services
{
    public interface IDbService<T, B, D>
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindById(int id);
        Task<T> FindByUUID(Guid UUID);
        Task<int> Insert(T entity);
        Task<int> Update(D entity);
        Task<int> Delete(B entity);
        Task<int> DeleteById(int id);
        Task<int> DeleteByUUID(Guid UUID);
    }
}
