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
    public class OrderController : ControllerBase
    {
        private readonly StoreRepositoryManager _storeRepositoryManager;
        private readonly IMapper _mapper;

        public OrderController(StoreRepositoryManager storeRepositoryManager, IMapper mapper)
        {
            _storeRepositoryManager = storeRepositoryManager ?? throw new ArgumentNullException(nameof(storeRepositoryManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get an Order by it's database id
        /// </summary>
        /// <param name="id">Identifying key of the Order</param>
        /// <returns>ActionResult Ok with OrderDto matching inputed id.</returns>
        /// <response code="404">Order with matching id could not be found</response>
        /// <response code="200">Successfully found a matching order.</response>
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult> GetOrder(int id)
        {
            Order order = await _storeRepositoryManager.orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Item = await _storeRepositoryManager.itemRepo.GetById(order.ItemId);
            return Ok(_mapper.Map<OrderDto>(order));
        }

        /// <summary>
        /// Create a new Order in database
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns>ActionResult CreatedAtRoute OrderDto of created Order.</returns>
        /// <response code="400">New Order was invalid.</response>
        /// <response code="201">Successfully created new Order!</response>
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderCreateDto orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);

            try
            {
                await _storeRepositoryManager.orderRepo.Add(order);
                await _storeRepositoryManager.SaveRepos();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var createdOrder = _mapper.Map<OrderDto>(order);
            return CreatedAtRoute("GetOrder",new {id = order.Id }, createdOrder);
        }

        /// <summary>
        /// Update existing order by replacing current data with a new order.
        /// </summary>
        /// <param name="id">Identifying key of the Order</param>
        /// <param name="orderUpdateDto">New order to replace existing order</param>
        /// <returns>ActionResult of NoContent</returns>
        /// <response code="404">Could not find matching existing </response>
        /// <response code="400">New Order was invalid</response>
        /// <response code="204">Successfully updated the Order</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
        {
            Order oldOrder = await _storeRepositoryManager.orderRepo.GetById(id);
            if (oldOrder == null)
            {
                return NotFound();
            }

            Order newOrder = _mapper.Map<Order>(orderUpdateDto);
            try
            {
                await _storeRepositoryManager.orderRepo.Update(newOrder);
                await _storeRepositoryManager.SaveRepos();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Update some properties of an existing Order in Database.
        /// </summary>
        /// <param name="id">Identifying key of the Order</param>
        /// <param name="orderPatchDocument">Json array of properties to change in existing order</param>
        /// <returns>ActionResult of NoContent</returns>
        /// <response code="404">Could not find matching existing Order.</response>
        /// <response code="400">New Order properties were invalid.</response>
        /// <response code="204">Successfully updated existing Order.</response>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateOrder(int id, JsonPatchDocument<OrderUpdateDto> orderPatchDocument)
        {
            Order oldOrder = await _storeRepositoryManager.orderRepo.GetById(id);
            if(oldOrder == null)
            {
                return NotFound();
            }

            JsonPatchDocument patch = _mapper.Map<JsonPatchDocument>(orderPatchDocument);

            try
            {
                await _storeRepositoryManager.orderRepo.PartialUpdate(id, patch);
                await _storeRepositoryManager.SaveRepos();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete Order from database
        /// </summary>
        /// <param name="id">Identifying key of the Order</param>
        /// <returns>ActionResult of NoContent</returns>
        /// <response code="404">Could not find matching existing Order.</response>
        /// <response code="204">Successfully deleted Order.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            Order nullCheck = await _storeRepositoryManager.orderRepo.GetById(id);
            if (nullCheck == null)
            {
                return NotFound();
            }

            await _storeRepositoryManager.orderRepo.Delete(id);
            return NoContent();
        }
    }
}
