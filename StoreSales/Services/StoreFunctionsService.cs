using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using StoreSales.API.Entities;
using StoreSales.API.Models;

namespace StoreSales.API.Services
{
    public class StoreFunctionsService
    {
        private readonly StoreRepositoryManager _storeRepositoryManager;
        private readonly IMapper _mapper;

        public StoreFunctionsService(StoreRepositoryManager storeRepositoryManager, IMapper mapper)
        {
            _storeRepositoryManager = storeRepositoryManager ?? throw new ArgumentNullException(nameof(storeRepositoryManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Subtracts a specified quantities from items in the inventory database.
        /// </summary>
        /// <param name="invChanges">List of Item and Quantity pairs to adjust in database</param>
        public async void EditInventory(IEnumerable<(int itemId, int quantityChange)> invChanges)
        {
            var inventory = await _storeRepositoryManager.inventoryRepo.GetAll();

            foreach (var ord in invChanges)
            {
                //Find associated items in inventory database and then subtract from inventory quantity
                //Log change pair if item doesn't already exist in inventory
                try { 
                    Inventory? inv = inventory.FirstOrDefault(x => x.ItemId == ord.itemId);
                    if (inv == null)
                    {
                        //Logs missed Inventory changes of missing items.  --Add proper logger later
                        System.Diagnostics.Debug.WriteLine($"Inventory missing Item: {ord.itemId} QuantityChange: {ord.quantityChange}");
                        continue;
                    }
                    inv.Quantity -= ord.quantityChange;
                    await _storeRepositoryManager.inventoryRepo.Update(inv);
                    await _storeRepositoryManager.inventoryRepo.Save();
                }
                catch(Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }

        public List<OrderPutDto> CompareOrders(List<OrderPutDto> newOrders, List<Order> oldOrders, out List<Order> ordersToRemove, out List<OrderPutDto> ordersToAdd)
        {
            ordersToAdd = newOrders.Where(o => !oldOrders.Any(o2 => o2.Id == o.Id)).ToList();
            ordersToRemove = oldOrders.Where(o => !newOrders.Any(o2 => o2.Id == o.Id)).ToList();
            return newOrders.Where(o => oldOrders.Any(o2 => o2.Id == o.Id)).ToList();

        }
    }
}
