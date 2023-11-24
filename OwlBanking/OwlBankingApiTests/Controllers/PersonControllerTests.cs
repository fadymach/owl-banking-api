using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Controllers;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApiTests.Controllers;

public class PersonControllerTests
{
    private IPersonRepository _personRepository; 
    private PersonController _personController;

    [SetUp]
    public void Setup()
    {
        this._personRepository = A.Fake<IPersonRepository>();
        this._personController = new PersonController(this._personRepository);
    }

    [Test]
    public async Task GetPersonsReturnsOkResult()
    {
        // Create 4 dummy People objects and call repository for GetPersonsAsync.  
        IList<Person> persons = A.CollectionOfFake<Person>(4);
        A.CallTo(() => this._personRepository.GetPersonsAsync()).Returns(Task.FromResult(persons));

        // Call controller to get GetPersons.
        IActionResult result = await this._personController.GetPersons();
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._personRepository.GetPersonsAsync()).MustHaveHappened();

        // Assert that returned results are correct. 
        Assert.That(result.GetType() == typeof(OkObjectResult), Is.True, "Api should return Ok Object Response."); 
        Assert.That(((ObjectResult) result).StatusCode, Is.EqualTo(200), "Response should have 200 Response");
        
        OkObjectResult? resultObject = result as OkObjectResult;
        IList<Person>? personList = resultObject?.Value as IList<Person>;

        Assert.True(personList != null);
        Assert.That(personList.Count, Is.EqualTo(4), "There should have been 4 Persons returned");
    }
    
    [Test]
    public async Task GetPersonReturnsOkResult()
    {
        // Create 1 dummy Person object and call repository for GetPersonByIdAsync.  
        Person person = A.Fake<Person>();
        person.PersonId = Guid.NewGuid();
        A.CallTo(() => this._personRepository.GetPersonByIdAsync(person.PersonId)).Returns(Task.FromResult(person));

        // Call controller to get GetPersons.
        ActionResult<Person> result = await this._personController.GetPerson(person.PersonId);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._personRepository.GetPersonByIdAsync(person.PersonId)).MustHaveHappened();

        Assert.NotNull(result.Result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(OkObjectResult), Is.True, "Api should return Ok Object Response."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(200), "Response should have 200 Response");
        
        Person personResult = (Person) objectResult.Value;

        Assert.True(personResult != null);
    }
    
    [Test]
    public async Task PostPersonReturnsCreatedResult()
    {
        // Create 1 dummy Person object and call repository for AddPersonAsync.  
        Person person = A.Fake<Person>();
        person.PersonId = Guid.NewGuid();
        person.FirstName = "firstNameForTesting";
        person.LastName = "lastNameForTesting";
        
        A.CallTo(() => this._personRepository.AddPersonAsync(person)).Returns(Task.FromResult(person));

        // Call controller to get PostPerson.
        ActionResult<Person> result = await this._personController.PostPerson(person);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._personRepository.AddPersonAsync(person)).MustHaveHappened();
        A.CallTo(() => this._personRepository.SaveAsync()).MustHaveHappened();

        Assert.NotNull(result.Result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(CreatedAtActionResult), Is.True, "Api should return Created At Action Result."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(201), "Response should have 201 Response");
        
        Person personResult = (Person) objectResult.Value;

        Assert.True(personResult != null);
        Assert.True(personResult.FirstName == "firstNameForTesting");
        Assert.True(personResult.LastName == "lastNameForTesting");
    }
}