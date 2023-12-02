using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
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
                item.Item = _mapper.Map<ItemDto>(invItem);
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
        [HttpGet("{id}", Name = "GetInventory")]
        public async Task<ActionResult> GetInventory(int id)
        {
            Inventory? item = await _storeRepositoryManager.inventoryRepo.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            InventoryDto returnItem = _mapper.Map<InventoryDto>(item);
            Item? invItem = await _storeRepositoryManager.itemRepo.GetById(item.ItemId);
            returnItem.Item = _mapper.Map<ItemDto>(invItem);

            return Ok(returnItem);
        }

        /// <summary>
        /// Add new item to Inventory
        /// </summary>
        /// <param name="inventory">Inventory entity Json to add to database</param>
        /// <returns>Returns created InventoryDto item</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <response code = "201">Item successfully added to inventory</response>
        /// <response code = "400">Inventory request was invalid</response>
        [HttpPost]
        public async Task<ActionResult> AddToInventory(InventoryCreateDto inventory)
        {
            try
            {
                var newInventory = _mapper.Map<Inventory>(inventory);
                await _storeRepositoryManager.inventoryRepo.Add(newInventory);
                await _storeRepositoryManager.SaveRepos();

                var returnInventory = _mapper.Map<InventoryDto>(newInventory);
                Item? invItem = await _storeRepositoryManager.itemRepo.GetById(newInventory.Id);
                returnInventory.Item = _mapper.Map<ItemDto>(invItem);

                return CreatedAtAction("GetInventory", new { id = returnInventory.Id }, returnInventory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates existing Inventory entity in database
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <param name="inventory">Json to replace existing Inventory entity</param>
        /// <returns>ActionResult of NoContent</returns>
        /// <response code = "204">Successfully updated Inventory entity</response>
        /// <response code = "404">Inventory entity couldn't be found</response>
        /// <response code = "400">Inventory request was invalid</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInventory(int id, [FromBody] InventoryUpdateDto inventory)
        {
            var entityCheck = await _storeRepositoryManager.inventoryRepo.GetById(id);
            if (entityCheck == null)
            {
                return NotFound();
            }

            try
            {
                var updatedInventory = _mapper.Map<Inventory>(inventory);
                await _storeRepositoryManager.inventoryRepo.Update(updatedInventory);
                await _storeRepositoryManager.SaveRepos();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Partially update existing Inventory entity
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <param name="patchDocument">JsonPatchDocument to update Inventory entity</param>
        /// <returns>ActionResult of NoContent</returns>
        /// <response code="204">Successfull partial updated of Inventory entity</response>
        /// <response code="404">Inventory entity couldn't be found</response>
        /// <response code="400">JsonPatchDocument was invalid</response>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateInventory(int id, [FromBody] JsonPatchDocument<InventoryUpdateDto> patchDocument)
        {
            var entityCheck = await _storeRepositoryManager.inventoryRepo.GetById(id);
            if (entityCheck == null)
            {
                return NotFound();
            }

            try
            {
                var inventoryPatch = _mapper.Map<JsonPatchDocument>(patchDocument);
                await _storeRepositoryManager.inventoryRepo.PartialUpdate(id, inventoryPatch);
                await _storeRepositoryManager.SaveRepos();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove Inventory entity from the database
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <returns>ActionResult of NoContent</returns>
        /// <response code="204">Successfully deleted Inventory entity</response>
        /// <response code="404">Inventory entity couldn't be found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            var nCheck = await _storeRepositoryManager.inventoryRepo.GetById(id);
            if (nCheck == null)
            {
                return NotFound();
            }

            await _storeRepositoryManager.inventoryRepo.Delete(id);
            await _storeRepositoryManager.SaveRepos();

            return NoContent();
        }
    }
}
