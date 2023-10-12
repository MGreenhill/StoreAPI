using StoreSales.API.DbContexts;
using StoreSales.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace StoreSales.API.Services
{
    public class StoreSalesRepository: IStoreSalesRepository
    {
        private readonly StoreSalesContext _context;
        public StoreSalesRepository(StoreSalesContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
