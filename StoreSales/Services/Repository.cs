using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using StoreSales.API.DbContexts;
using StoreSales.API.Entities;

namespace StoreSales.API.Services
{
    public class Repository<TEntitiy> : IRepository<TEntitiy> where TEntitiy : class, IEntity
    {
        private readonly StoreSalesContext _context;

        public Repository(StoreSalesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task Add(TEntitiy entity)
        {
            _context.Set<TEntitiy>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var entityToRemove = await GetById(Id);
            if (entityToRemove != null)
            {
                _context.Set<TEntitiy>().Remove(entityToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(TEntitiy entity)
        {
            TEntitiy? oldEntity = await _context.Set<TEntitiy>().FirstOrDefaultAsync(e => e.Id == entity.Id);
            if(oldEntity != null)
            {
                _context.Entry(oldEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task PartialUpdate(int Id, JsonPatchDocument patchDocument)
        {
            TEntitiy? oldEntity = await _context.Set<TEntitiy>().FirstOrDefaultAsync(e => e.Id == Id);
            if (oldEntity != null)
            {
                patchDocument.ApplyTo(oldEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TEntitiy>> GetAll()
        {
            return await _context.Set<TEntitiy>().Select(a => a).ToListAsync();
        }

        public async Task<TEntitiy?> GetById(int Id)
        {
            return await _context.Set<TEntitiy>().FirstOrDefaultAsync(e => e.Id == Id);
        }
    }
}
