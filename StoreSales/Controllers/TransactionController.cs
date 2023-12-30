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
    public class TransactionController : ControllerBase
    {
        private readonly StoreRepositoryManager _storeRepositoryManager;
        private readonly IMapper _mapper;
        private readonly StoreFunctionsService _storeFunctionsService;


        public TransactionController(StoreRepositoryManager storeRepositoryManager, IMapper mapper, StoreFunctionsService storeFunctionsService)
        {
            _storeRepositoryManager = storeRepositoryManager ?? throw new ArgumentNullException(nameof(storeRepositoryManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _storeFunctionsService = storeFunctionsService ?? throw new ArgumentNullException(nameof(storeFunctionsService));
        }

        /// <summary>
        /// Get Transaction by id.  Can include associated Orders.
        /// </summary>
        /// <param name="id">Identifying key of Transaction</param>
        /// <param name="includeOrders">Bool to request associated Orders</param>
        /// <returns>Json of Transaction and, if includeOrders is true, associated Orders </returns>
        /// <response code = "200">Successfully retrieved transaction!</response>
        /// <response code = "404">Could not find transaction.</response>
        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<ActionResult> GetTransaction(int id, bool includeOrders)
        {
            //Get transaction and check if null
            Transaction? transaction =  await _storeRepositoryManager.transactionRepo.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            //Assign Orders to transaction
            IEnumerable<Order> orders = await _storeRepositoryManager.orderRepo.GetAll();
            transaction.Contents = (ICollection<Order>)orders.Where(o => o.TransactionId == transaction.Id).ToList();

            decimal price = 0;
            //Add Items to transaction's orders
            //Add prices together for return dto
            foreach(Order order in transaction.Contents)
            {
                order.Item = await _storeRepositoryManager.itemRepo.GetById(order.ItemId) ?? null;
                price += (order.Item.Price * order.Quantity);
            }

            //Add person to transaction
            transaction.Person = await _storeRepositoryManager.personRepo.GetById(transaction.PersonId);

            //Map dto based on whether to include orders
            dynamic returnTransaction = includeOrders ? _mapper.Map<TransactionWithOrdersDto>(transaction) : _mapper.Map<TransactionDto>(transaction);
            returnTransaction.TotalPrice = price;
            return Ok(returnTransaction);
        }

        /// <summary>
        /// Create New Transaction and associated Orders.  Uses Json Array
        /// </summary>
        /// <param name="transactionPost">JsonArray of TransactionCreate Dto plus array of OrderCreateDtos</param>
        /// <returns>Json of created Transaction and associated list of Orders</returns>
        /// <response code = "204">Successfully created transaction</response>
        /// <response code = "400">Could not created transaction.</response>
        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionPostDto transactionPost)
        {
            //Split DTOs to manage
            TransactionCreateDto transaction = transactionPost.Transaction;
            List<OrderCreateDto> orders = transactionPost.Orders;
            try
            {
                //Add timestamp to transaction, then convert it to entity and submit to database.
                transaction.TimeOccurred = DateTime.UtcNow;
                Transaction newTransaction = _mapper.Map<Transaction>(transaction);
                await _storeRepositoryManager.transactionRepo.Add(newTransaction);

                //Convert newTransaction to DTO for later CreatedAtRoute
                var createdTransaction = _mapper.Map<TransactionDto>(newTransaction);

                //Get new entity's Id by searching TimeOccured so that it can be added to orders list.
                IEnumerable<Transaction> allTransactions = await _storeRepositoryManager.transactionRepo.GetAll();
                createdTransaction.Id = allTransactions.FirstOrDefault(x => x.TimeOccurred == createdTransaction.TimeOccurred).Id;
                createdTransaction.Person = _mapper.Map<PersonWithoutTransactionsDto>(
                                                await _storeRepositoryManager.personRepo.GetById(
                                                        createdTransaction.PersonId));

                List<(int, int)> invChanges = new List<(int, int)>();

                //Add TransactionId to each Order DTO and then add to database
                foreach(OrderCreateDto order in orders)
                {
                    order.TransactionId = createdTransaction.Id;
                    var newOrder = _mapper.Map<Order>(order);
                    await _storeRepositoryManager.orderRepo.Add(newOrder); 
                    //Add ItemId and Quantity to list to adjust inventory
                    invChanges.Add((order.ItemId, order.Quantity));
                }

                _storeFunctionsService.EditInventory(invChanges);

                await _storeRepositoryManager.SaveRepos();

                //Return created Transaction and Orders
                return CreatedAtRoute("GetTransaction", new { id = createdTransaction.Id, includeOrders = true }, createdTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
