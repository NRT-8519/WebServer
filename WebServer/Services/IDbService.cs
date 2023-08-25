namespace WebServer.Services
{
    public interface IDbService<T, B, D>
    {
        Task<IEnumerable<D>> FindAll();
        Task<T> FindById(int id);
        Task<D> FindByUUID(Guid UUID);
        Task<int> Insert(D entity);
        Task<int> Update(D entity);
        Task<int> Delete(B entity);
        Task<int> DeleteById(int id);
        Task<int> DeleteByUUID(Guid UUID);
    }
}
