using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using OwlBankingApi.Controllers;
using OwlBankingApi.Models;
using OwlBankingApi.Repositories.Interfaces;

namespace OwlBankingApiTests.Controllers;

public class BusinessLogicTests
{
    private IBankAccountRepository _bankAccountRepository;
    private IPersonRepository _personRepository;
    private ITransactionRepository _transactionRepository;
    private BankAccountController _bankAccountController;
    private PersonController _personController;
    private TransactionController _transactionController;

    [SetUp]
    public void Setup()
    {
        this._bankAccountRepository = A.Fake<IBankAccountRepository>();
        this._personRepository = A.Fake<IPersonRepository>();
        this._transactionRepository = A.Fake<ITransactionRepository>();

        this._bankAccountController = new BankAccountController(this._bankAccountRepository);
        this._personController = new PersonController(this._personRepository);
        this._transactionController =
            new TransactionController(this._transactionRepository, this._bankAccountRepository);
    }

    [Test]
    // Create a new bank account for a customer, with an initial deposit amount. A
    // single customer may have multiple bank accounts.
    public async Task CreateNewSecondBankAccountForPersonWithInitialDepositAmount()
    {
        Person person = A.Fake<Person>();
        A.CallTo(() => this._personRepository.AddPersonAsync(person)).Returns(Task.FromResult(person));
        BankAccount initialBankAccount = A.Fake<BankAccount?>();
        person.PersonId = Guid.NewGuid();
        person.FirstName = "firstNameForTesting";
        person.LastName = "lastNameForTesting";
        initialBankAccount.BankAccountId = Guid.NewGuid();
        initialBankAccount.Balance = (decimal) 10.22;
        initialBankAccount.PersonId = person.PersonId;
        person.Accounts.Add(initialBankAccount);

        A.CallTo(() => this._bankAccountRepository.GetBankAccountByIdAsync(initialBankAccount.BankAccountId))
            .Returns(Task.FromResult(initialBankAccount));

        BankAccount newBankAccount = A.Fake<BankAccount?>();
        newBankAccount.BankAccountId = Guid.NewGuid();
        newBankAccount.Balance = (decimal) 12.22;
        newBankAccount.PersonId = person.PersonId;
        A.CallTo(() => this._bankAccountRepository.GetBankAccountByIdAsync(newBankAccount.BankAccountId))
            .Returns(Task.FromResult(newBankAccount));

        // Call controller to get PostBankAccount.
        ActionResult<BankAccount> bankAccountResult = await this._bankAccountController.PostBankAccount(newBankAccount);
        
        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._bankAccountRepository.AddBankAccountAsync(newBankAccount)).MustHaveHappened();
        A.CallTo(() => this._bankAccountRepository.SaveAsync()).MustHaveHappened();

        Assert.NotNull(bankAccountResult.Result);
        ObjectResult? objectResult = (ObjectResult) bankAccountResult.Result;

        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(CreatedAtActionResult), Is.True,
            "Api should return Created At Action Result.");
        Assert.That(objectResult.StatusCode, Is.EqualTo(201), "Response should have 201 Response");

        person.Accounts.Add(newBankAccount);
        A.CallTo(() => this._personRepository.GetPersonByIdAsync(person.PersonId)).Returns(Task.FromResult(person));
        // Call controller to get the Person to check their second account was added.
        ActionResult<Person> personResult = await this._personController.GetPerson(person.PersonId);

        Assert.That(objectResult.GetType() == typeof(CreatedAtActionResult), Is.True,
            "Api should return Created At Action Result.");
        Assert.That(objectResult.StatusCode, Is.EqualTo(201), "Response should have 201 Response");

        Assert.True(personResult != null);

        // Confirm a second bank account was added and that the second bank account has the balance of 12.22 as above.
        Person personValue = (Person) ((ObjectResult) personResult.Result).Value;
        Assert.True(personValue.Accounts.Count == 2);
        Assert.True(personValue.Accounts.Last().Balance == (decimal) 12.22);
    }


    [Test]
    // - Transfer amounts between any two accounts, including those owned by
    //     different customers.
    public async Task TransferAmountsBetweenTwoAccountsForTheSameCustomer()
    {
        Person person = A.Fake<Person>();
        A.CallTo(() => this._personRepository.AddPersonAsync(person)).Returns(Task.FromResult(person));
        BankAccount? firstBankAccount = A.Fake<BankAccount?>();
        person.PersonId = Guid.NewGuid();
        person.FirstName = "firstNameForTesting";
        person.LastName = "lastNameForTesting";
        firstBankAccount.BankAccountId = Guid.NewGuid();
        firstBankAccount.Balance = (decimal) 10.22;
        firstBankAccount.PersonId = person.PersonId;
        person.Accounts.Add(firstBankAccount);

        BankAccount? secondBankAccount = A.Fake<BankAccount?>();
        secondBankAccount.BankAccountId = Guid.NewGuid();
        secondBankAccount.Balance = (decimal) 12.22;
        secondBankAccount.PersonId = person.PersonId;
        person.Accounts.Add(secondBankAccount);


        // Transfer $10.22 from the first bank account to the second one for the same customer.
        Transaction transaction = A.Fake<Transaction>();
        transaction.TransactionId = Guid.NewGuid();
        transaction.TransactionAmount = (decimal) 10.22;
        transaction.SourceBankAccountId = firstBankAccount.BankAccountId;
        transaction.DestinationBankAccountId = secondBankAccount.BankAccountId;

        A.CallTo(() => this._transactionRepository.AddTransactionAsync(transaction))
            .Returns(Task.FromResult(transaction));
        A.CallTo(() => this._bankAccountRepository.GetBankAccountByIdAsync(secondBankAccount.BankAccountId))
            .Returns(Task.FromResult(secondBankAccount));
        
        // Either return the first or second bank accounts based on the Guid (BankAccountId) passed in. 
        A.CallTo(() => this._bankAccountRepository.GetBankAccountByIdAsync(A<Guid>._))
            .ReturnsLazily(call => Task.FromResult(
                call.GetArgument<Guid>(0) == firstBankAccount.BankAccountId
                    ? firstBankAccount
                    : secondBankAccount
            ));
        
        // Invoke the actual call make a transaction between the balances of first and second bank accounts
        A.CallTo(() => _transactionRepository.AddTransactionAsync(A<Transaction>._))
            .Invokes(call =>
            {
                // Modify the balances in memory when AddTransactionAsync is called
                Transaction? transaction = call.GetArgument<Transaction>(0);
                firstBankAccount.Balance -= transaction.TransactionAmount;
                secondBankAccount.Balance += transaction.TransactionAmount;
            })
            .ReturnsLazily(call => Task.FromResult(call.GetArgument<Transaction>(0)));

        
        ActionResult<Transaction>? bankAccountResult = await this._transactionController.PostTransaction(transaction);

        // Ensure that the methods in the repository were called. 
        A.CallTo(() => this._transactionRepository.AddTransactionAsync(transaction)).MustHaveHappened();
        A.CallTo(() => this._transactionRepository.SaveAsync()).MustHaveHappened();

        firstBankAccount = await this._bankAccountRepository.GetBankAccountByIdAsync(firstBankAccount.BankAccountId);
        secondBankAccount = await this._bankAccountRepository.GetBankAccountByIdAsync(secondBankAccount.BankAccountId);

        Assert.NotNull(bankAccountResult.Result);
        ObjectResult? objectResult = (ObjectResult) bankAccountResult.Result;

        // Assert that returned results are correct. 
        Assert.That(objectResult.GetType() == typeof(CreatedAtActionResult), Is.True,
            "Api should return Created At Action Result.");
        Assert.That(objectResult.StatusCode, Is.EqualTo(201), "Response should have 201 Response");
        
        // Confirm that the transfer has taken place
        Assert.AreEqual((decimal) 0.00, firstBankAccount.Balance);
        Assert.AreEqual((decimal) 22.44, secondBankAccount.Balance);
    }
}