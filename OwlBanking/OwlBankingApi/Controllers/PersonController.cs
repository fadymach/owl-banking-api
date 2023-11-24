using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        
        public PersonController(IPersonRepository personRepository)
        {
            this._personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }
        
        // GET: api/Persons
        [HttpGet("/Persons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersons()
        {
            IList<Person> persons = await this._personRepository.GetPersonsAsync();
            return Ok(persons);
        }

        // GET: api/Person/5
        [HttpGet("{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Person>> GetPerson(Guid personId)
        {
            Person? person = await this._personRepository.GetPersonByIdAsync(personId);
            
            if (person == null)
            {
                return NotFound();
            }
            
            return Ok(person);
        }
        

        // POST: api/Person
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this._personRepository.AddPersonAsync(person);
                    await this._personRepository.SaveAsync();
                    return CreatedAtAction("PostPerson", new { id = person.PersonId }, person);
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, "Unable to save changes.");
            }
            
            return NoContent();
        }
    }
}
