using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Controllers;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApiTests.Controllers;

public class TransactionControllerTests
{
    private ITransactionRepository _transactionRepository;
    private IBankAccountRepository _bankAccountRepository;
    private TransactionController _transactionController;

    [SetUp]
    public void Setup()
    {
        this._transactionRepository = A.Fake<ITransactionRepository>();
        this._bankAccountRepository = A.Fake<IBankAccountRepository>();
        this._transactionController = new TransactionController(this._transactionRepository, this._bankAccountRepository);
    }

    [Test]
    public async Task GetTransactionByIdReturnsOkResult()
    {
        Transaction? transaction = A.Fake<Transaction?>();
        A.CallTo(() => this._transactionRepository.GetTransactionByIdAsync(transaction.TransactionId)).Returns(Task.FromResult(transaction));

        // Call controller to get GetTransaction.
        ActionResult<Transaction> result = await this._transactionController.GetTransaction(transaction.TransactionId);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._transactionRepository.GetTransactionByIdAsync(transaction.TransactionId)).MustHaveHappened();

        Assert.NotNull(result.Result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(OkObjectResult), Is.True, "Api should return Ok Object Response."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(200), "Response should have 200 Response");
        
        Transaction transactionResult = (Transaction) objectResult.Value;

        Assert.True(transactionResult != null);
    }
    
    [Test]
    public async Task PostTransactionReturnsCreatedResult()
    {
        // Create 1 dummy Transaction object and call repository for AddTransactionAsync.  
        Transaction transaction = A.Fake<Transaction>();
        transaction.TransactionId = Guid.NewGuid();
        transaction.TransactionAmount = (decimal) 10.22;
        
        A.CallTo(() => this._transactionRepository.AddTransactionAsync(transaction)).Returns(Task.FromResult(transaction));

        // Call controller to get PostTransaction.
        ActionResult<Transaction> result = await this._transactionController.PostTransaction(transaction);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._transactionRepository.AddTransactionAsync(transaction)).MustHaveHappened();

        Assert.NotNull(result.Result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(CreatedAtActionResult), Is.True, "Api should return Created At Action Result."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(201), "Response should have 201 Response");
        
        Transaction transactionResult = (Transaction) objectResult.Value;

        Assert.True(transactionResult != null);
        Assert.True(transactionResult.TransactionAmount == (decimal) 10.22);
    }
}