using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StoreSales.API.Entities;
using StoreSales.API.Models;
using StoreSales.API.Services;

namespace StoreSales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly StoreRepositoryManager _storeRepositoryManager;

        private readonly IMapper _mapper;

        public ItemController(StoreRepositoryManager storeRepositoryManager, IMapper mapper)
        {
            _storeRepositoryManager = storeRepositoryManager ?? throw new ArgumentNullException(nameof(storeRepositoryManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Item entity with specific id from database
        /// </summary>
        /// <param name="id">Database id of Item Entity</param>
        /// <returns>ActionResult of ItemDto</returns>
        /// <response code ="200">Returns requested item entity</response>
        /// <response code ="404">Item does not exsits</response>
        [HttpGet("{id}", Name = "GetItem")]
        public async Task<ActionResult> GetItem(int id)
        {
            var item = await _storeRepositoryManager.itemRepo.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ItemDto>(item));
        }

        /// <summary>
        /// Add new Item to Database
        /// </summary>
        /// <param name="newItem">New Item written in JSON</param>
        /// <returns>ActionResult of created Item</returns>
        /// <response code="201">Item was added to database successfully</response>
        /// <response code="400">New Item was invalid</response>
        [HttpPost]
        public async Task<ActionResult> CreateItem([FromBody] ItemCreateDto newItem)
        {
            try
            {
                var item = _mapper.Map<Item>(newItem);
                await _storeRepositoryManager.itemRepo.Add(item);
                await _storeRepositoryManager.SaveRepos();

                var createdItem = _mapper.Map<ItemDto>(item);

                return CreatedAtRoute("GetItem", new { id = item.Id }, createdItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Replace database Item
        /// </summary>
        /// <param name="id">Database id of Item Entity</param>
        /// <param name="item">Item's replacement written in JSON</param>
        /// <returns>ActionResult of task complete</returns>
        /// <response code="204">Item successfully replaced</response>
        /// <response code="400">Item's replacement was invalid</response>
        /// <response code="404">Item was not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] ItemUpdateDto item)
        {
            var entityCheck = await _storeRepositoryManager.itemRepo.GetById(id);
            if (entityCheck == null)
            {
                return NotFound();
            }

            try
            {
                var updatedItem = _mapper.Map<Item>(item);
                await _storeRepositoryManager.itemRepo.Update(updatedItem);
                await _storeRepositoryManager.SaveRepos();

                return NoContent();
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Partially Update an Item in the database
        /// </summary>
        /// <param name="id">Database id of Item Entity</param>
        /// <param name="patchDocument">JsonPatchDocument list of operations and values to change in database Item</param>
        /// <returns>ActionResult of completed task</returns>
        /// <response code="204">Item successfully partially updated</response>
        /// <response code="400">Item's patch was invalid</response>
        /// <response code="404">Item was not found</response>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateItem(int id, [FromBody] JsonPatchDocument<ItemUpdateDto> patchDocument)
        {
            Item entityCheck = await _storeRepositoryManager.itemRepo.GetById(id);
            if(entityCheck == null)
            {
                return NotFound();
            }

            try
            {
                JsonPatchDocument entityPatch = _mapper.Map<JsonPatchDocument>(patchDocument);
                await _storeRepositoryManager.itemRepo.PartialUpdate(id, entityPatch);
                await _storeRepositoryManager.SaveRepos();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove Item from database
        /// </summary>
        /// <param name="id">Database id of Item Entity</param>
        /// <returns>ActionResult of completed task</returns>
        /// <response code="204">Item was successfully removed</response>
        /// <response code="404">Item was not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            Item entityCheck = await _storeRepositoryManager.itemRepo.GetById(id);
            if (entityCheck == null)
            {
                return NotFound();
            }

            await _storeRepositoryManager.itemRepo.Delete(id);
            await _storeRepositoryManager.SaveRepos();
            return NoContent();
        }
    }
}
