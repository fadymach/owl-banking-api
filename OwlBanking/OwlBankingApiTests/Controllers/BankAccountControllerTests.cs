using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Controllers;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApiTests.Controllers;

public class BankAccountControllerTests
{
    private IBankAccountRepository _bankAccountRepository;
    private BankAccountController _bankAccountController;

    [SetUp]
    public void Setup()
    {
        this._bankAccountRepository = A.Fake<IBankAccountRepository>();
        this._bankAccountController = new BankAccountController(this._bankAccountRepository);
    }

    [Test]
    public async Task GetPersonsReturnsOkResult()
    {
        BankAccount bankAccount = A.Fake<BankAccount?>();
        A.CallTo(() => this._bankAccountRepository.GetBankAccountByIdAsync(bankAccount.BankAccountId)).Returns(Task.FromResult(bankAccount));

        // Call controller to get GetBankAccount.
        ActionResult<BankAccount> result = await this._bankAccountController.GetBankAccount(bankAccount.BankAccountId);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._bankAccountRepository.GetBankAccountByIdAsync(bankAccount.BankAccountId)).MustHaveHappened();

        Assert.NotNull(result.Result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(OkObjectResult), Is.True, "Api should return Ok Object Response."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(200), "Response should have 200 Response");
        
        BankAccount bankAccountResult = (BankAccount) objectResult.Value;

        Assert.True(bankAccountResult != null);
    }
    
    [Test]
    // - Retrieve transfer history for a given account.
    public async Task GetBankAccountTransactionsReturnsOkResult()
    {
        List<Transaction?> transactions = A.CollectionOfFake<Transaction?>(4).ToList();
        BankAccount bankAccount = A.Fake<BankAccount>();
        A.CallTo(() => this._bankAccountRepository.GetBankAccountTransactionsAsync(bankAccount.BankAccountId)).Returns(Task.FromResult(transactions));

        // Call controller to get GetBankAccountTransactions.
        IActionResult result = await this._bankAccountController.GetBankAccountTransactions(bankAccount.BankAccountId);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._bankAccountRepository.GetBankAccountTransactionsAsync(bankAccount.BankAccountId)).MustHaveHappened();

        // Assert that returned results are correct. 
        Assert.That(result.GetType() == typeof(OkObjectResult), Is.True, "Api should return Ok Object Response."); 
        Assert.That(((ObjectResult) result).StatusCode, Is.EqualTo(200), "Response should have 200 Response");
        
        OkObjectResult? resultObject = result as OkObjectResult;
        IList<Transaction>? transactionsList = resultObject?.Value as IList<Transaction>;

        Assert.True(transactionsList != null);
        Assert.That(transactionsList.Count, Is.EqualTo(4), "There should have been 4 Transactions returned");
    }
    
    
    [Test]
    // - Retrieve balances for a given account.
    public async Task GetBankAccountBalanceReturnsOkResult()
    {
        BankAccount bankAccount = A.Fake<BankAccount>();
        bankAccount.Balance = (decimal) 10.22;
        A.CallTo(() => this._bankAccountRepository.GetBankAccountBalanceAsync(bankAccount.BankAccountId)).Returns(Task.FromResult(bankAccount?.Balance));

        // Call controller to get GetBankAccountBalance.
        ActionResult<decimal> result = await this._bankAccountController.GetBankAccountBalance(bankAccount.BankAccountId);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._bankAccountRepository.GetBankAccountBalanceAsync(bankAccount.BankAccountId)).MustHaveHappened();

        Assert.NotNull(result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(OkObjectResult), Is.True, "Api should return Ok Object Result."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(200), "Response should have 200 Response");
        
        decimal bankAccountResult = (decimal) objectResult.Value;
        Assert.True(bankAccountResult == (decimal) 10.22);
    }
    
    [Test]
    public async Task PostBankAccountReturnsCreatedResult()
    {
        BankAccount bankAccount = A.Fake<BankAccount>();
        bankAccount.BankAccountId = Guid.NewGuid();   
        bankAccount.Balance = (decimal) 10.22;

        A.CallTo(() => this._bankAccountRepository.AddBankAccountAsync(bankAccount)).Returns(Task.FromResult(bankAccount));

        // Call controller to get PostBankAccount.
        ActionResult<BankAccount> result = await this._bankAccountController.PostBankAccount(bankAccount);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._bankAccountRepository.AddBankAccountAsync(bankAccount)).MustHaveHappened();
        A.CallTo(() => this._bankAccountRepository.SaveAsync()).MustHaveHappened();

        Assert.NotNull(result.Result);
        ObjectResult? objectResult = (ObjectResult) result.Result;
        
        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(CreatedAtActionResult), Is.True, "Api should return Created At Action Result."); 
        Assert.That(objectResult.StatusCode, Is.EqualTo(201), "Response should have 201 Response");
        
        
        decimal bankAccountResult = ((BankAccount) objectResult.Value).Balance;
        Assert.True(bankAccountResult != null);
        Assert.True(bankAccountResult == (decimal) 10.22);
    }
    
}