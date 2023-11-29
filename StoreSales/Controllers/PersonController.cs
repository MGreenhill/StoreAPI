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
    public class PersonController : ControllerBase
    {
        private readonly StoreRepositoryManager _storeRepositoryManager;
        private readonly IMapper _mapper;

        public PersonController(StoreRepositoryManager storeRepositoryManager, IMapper mapper)
        {
            _storeRepositoryManager = storeRepositoryManager ?? throw new ArgumentNullException(nameof(storeRepositoryManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns a Person Entity using it's Id.
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <param name="viewPurchases">Boolean to include person's transaction history</param>
        /// <returns>An ActionResult of PersonDto or PersonWithoutTransactionDto</returns>
        /// <response code ="200">Returns requested person entity</response>
        /// <response code ="404">Person does not exist</response>
        /// <response code ="400">Request for person was invalid</response>
        [HttpGet("{id}", Name = "GetPerson")]
        public async Task<ActionResult> GetPerson(int id, bool viewPurchases)
        {
            var person = await _storeRepositoryManager.personRepo.GetById(id);
            if (person == null)
            {
                return NotFound();
            }
            if (viewPurchases)
            {
                return Ok(_mapper.Map<PersonDto>(person));
            }
            return Ok(_mapper.Map<PersonWithoutTransactionsDto>(person));
        }

        /// <summary>
        /// Create new person in database
        /// </summary>
        /// <param name="newPerson">Json of new person to add into database</param>
        /// <returns>ActionResult PersonDto of newly created person</returns>
        /// <response code="201">Person was successfully created</response>
        /// <response code="400">Request was invalid</response>
        [HttpPost]
        public async Task<ActionResult> CreatePerson([FromBody] PersonCreateDto newPerson)
        {
            try
            {
                var personEntity = _mapper.Map<Person>(newPerson);
                await _storeRepositoryManager.personRepo.Add(personEntity);
                await _storeRepositoryManager.SaveRepos();

                var createdPerson = _mapper.Map<PersonDto>(personEntity);

                return CreatedAtRoute("GetPerson", new { id = personEntity.Id, viewPurchases = false }, createdPerson);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates person in database with submitted Json
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <param name="person">Json to replace person entity</param>
        /// <returns>ActionResult of completed task</returns>
        /// <response code="204">Person successfully updated</response>
        /// <response code="400">User request was invalid</response>
        /// <response code="404">Person to replace was not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDto person)
        {
            var entityCheck = await _storeRepositoryManager.personRepo.GetById(id);
            if (entityCheck == null)
            {
                return NotFound();
            }

            try
            {
                var updatedPerson = _mapper.Map<Person>(person);
                await _storeRepositoryManager.personRepo.Update(updatedPerson);
                await _storeRepositoryManager.SaveRepos();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Partially updates a person in database using a JsonPatchDocument
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <param name="patchDocument">Set of properties to update person in database</param>
        /// <returns>ActionResult of completed Task</returns>
        /// <response code="204">Person has been successfully partially updated</response>
        /// <response code="400">User request was invalid</response>
        /// <response code="404">Person does not exist</response>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdatePerson(int id, [FromBody] JsonPatchDocument<PersonUpdateDto> patchDocument)
        {
            var personToUpdate = await _storeRepositoryManager.personRepo.GetById(id);
            if (personToUpdate == null)
            {
                return NotFound();
            }

            try
            {
                var entityPatch = _mapper.Map<JsonPatchDocument>(patchDocument);
                await _storeRepositoryManager.personRepo.PartialUpdate(id, entityPatch);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes person from database
        /// </summary>
        /// <param name="id">Database id of Person Entity</param>
        /// <returns>ActionResult of completed task</returns>
        /// <response code = "204">Successfully deleted Person</response>
        /// <response code = "404">Person to delete not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            var entityCheck = await _storeRepositoryManager.personRepo.GetById(id);
            if (entityCheck == null)
            {
                return NotFound();
            }

            await _storeRepositoryManager.personRepo.Delete(id);
            await _storeRepositoryManager.SaveRepos();

            return NoContent();
        }

    }
}
