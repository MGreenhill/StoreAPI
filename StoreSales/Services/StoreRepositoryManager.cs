using Microsoft.EntityFrameworkCore;
using StoreSales.API.DbContexts;
using StoreSales.API.Entities;

namespace StoreSales.API.Services
{
    public class StoreRepositoryManager
    {
        private readonly StoreSalesContext _context;

        public IRepository<Item> itemRepo;
        public IRepository<Inventory> inventoryRepo;
        public IRepository<Person> personRepo;
        public IRepository<Order> orderRepo;
        public IRepository<Transaction> transactionRepo;


        public StoreRepositoryManager(StoreSalesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            itemRepo = new Repository<Item>(_context);
            inventoryRepo = new Repository<Inventory>(_context);
            personRepo = new Repository<Person>(_context);
            orderRepo = new Repository<Order>(_context);
            transactionRepo = new Repository<Transaction>(_context);
        }

        public async Task SaveRepos()
        {
            try
            {
                await itemRepo.Save();
                await inventoryRepo.Save();
                await personRepo.Save();
                await orderRepo.Save();
                await transactionRepo.Save();
            }
            catch (Exception ex) { }
        }
    }
}
