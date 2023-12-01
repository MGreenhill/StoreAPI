using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreSales.API.Entities;
using StoreSales.API.Models;
using StoreSales.API.Services;
using System.Runtime.CompilerServices;

namespace StoreSales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly StoreRepositoryManager _storeRepositoryManager;
        private readonly IMapper _mapper;

        public InventoryController(StoreRepositoryManager storeRepositoryManager, IMapper mapper)
        {
            _storeRepositoryManager = storeRepositoryManager ?? throw new ArgumentNullException(nameof(storeRepositoryManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns a list of items with quantities from Inventory
        /// </summary>
        /// <returns>An ActionResult IEnumerable of InventoryDTOs</returns>
        /// <response code="200">Successfully returned Inventory list</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> ViewInventory()
        {
            var inventory = await _storeRepositoryManager.inventoryRepo.GetAll();
            var inventoryList = _mapper.Map<IEnumerable<InventoryDto>>(inventory);
            foreach (var item in inventoryList)
            {
                Item? invItem = await _storeRepositoryManager.itemRepo.GetById(item.ItemId);
                item.ItemName = invItem.Name ?? throw new ArgumentNullException(nameof(invItem));
            }
            return Ok(inventoryList);
        }

        /// <summary>
        /// Get Item with quantity from inventory
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <returns>ActionResult of InventoryDto</returns>
        /// <response code="200">Successfully returned Inventory item</response>
        /// <response code="404">Inventory item was not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetInventory(int id)
        {
            Inventory? item = await _storeRepositoryManager.inventoryRepo.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            InventoryDto returnItem = _mapper.Map<InventoryDto>(item);
            Item? invItem = await _storeRepositoryManager.itemRepo.GetById(id);
            returnItem.ItemName = invItem.Name ?? throw new ArgumentNullException(nameof(invItem));

            return Ok(returnItem);
        }
    }
}
