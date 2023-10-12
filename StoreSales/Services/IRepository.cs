using StoreSales.API.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace StoreSales.API.Services
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        //All IRepositories will follow the CRUD standard
        //GetByID, GetAll, Create, Update, Delete
        Task<TEntity?> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task PartialUpdate(int id, JsonPatchDocument patchDocument);
        Task Delete(int id);
        Task Save();
    }
}
