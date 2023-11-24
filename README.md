Testing the API via Swagger 

1. run the program - this should open a browser with swagger UI 
2. Under the Person heading - run the Persons GET call to confirm there are 4 default users. 
200 repsonse expected
3. Add a bank account to two of the users via the api/BankAccount POST call. 
  use the following json to make two separate requests - this will give person 1 and 2 a bank account each. 
  (a call to persons will not return their bank accounts - to see each person's accounts, you must make a call to GET a specific person via api/person/{personId})

{
  "bankAccountId": "3fa85f64-5717-4562-b3fc-2c963f661111",
  "accountNumber": 0,
  "balance": 10.10,
  "accountType": 0,
  "signUpDate": "2023-11-10T16:15:17.800Z",
  "personId": "3fa85f64-5717-4562-b3fc-2c963f66afa1"
}
{
  "bankAccountId": "3fa85f64-5717-4562-b3fc-2c963f662222",
  "accountNumber": 0,
  "balance": 12.12,
  "accountType": 0,
  "signUpDate": "2023-11-10T16:15:17.800Z",
  "personId": "3fa85f64-5717-4562-b3fc-2c963f66afa2"
}

201 response expected - twice one for each call 


4. Under the Transaction heading - run the transaction POST method with the following data to create a transaction between person 1 and 2


{
  "transactionId": "31235f64-5717-4562-b3fc-2c963f66afa3",
  "transactionAmount": 10.22,
  "transactionDate": "2023-11-10T18:21:30.615Z",
  "sourceBankAccountId": "3fa85f64-5717-4562-b3fc-2c963f661111",
  "destinationBankAccountId": "3fa85f64-5717-4562-b3fc-2c963f662222"
}

201 response expected.

5. Under the BankAccount heading - run the api/BankAccount/{id} GET method to confirm the two bank account's balances have been manipulated 

3fa85f64-5717-4562-b3fc-2c963f661111
3fa85f64-5717-4562-b3fc-2c963f662222













### Objective

Your assignment is to build an internal API for a fake financial institution using C# and .Net.

### Brief

While modern banks have evolved to serve a plethora of functions, at their core, banks must provide certain basic features. Today, your task is to build the basic HTTP API for one of those banks! Imagine you are designing a backend API for bank employees. It could ultimately be consumed by multiple frontends (web, iOS, Android etc).

### Tasks

- Implement assignment using:
  - Language: **C#**
  - Framework: **.Net**
- There should be API routes that allow them to:
  - Create a new bank account for a customer, with an initial deposit amount. A
    single customer may have multiple bank accounts.
  - Transfer amounts between any two accounts, including those owned by
    different customers.
  - Retrieve balances for a given account.
  - Retrieve transfer history for a given account.
- Write tests for your business logic

Feel free to pre-populate your customers with the following:

```json
[
  {
    "id": 1,
    "name": "Arisha Barron"
  },
  {
    "id": 2,
    "name": "Branden Gibson"
  },
  {
    "id": 3,
    "name": "Rhonda Church"
  },
  {
    "id": 4,
    "name": "Georgina Hazel"
  }
]
```

You are expected to design any other required models and routes for your API.

### Evaluation Criteria

- **C#** best practices
- Completeness: did you complete the features?
- Correctness: does the functionality act in sensible, thought-out ways?
- Maintainability: is it written in a clean, maintainable way?
- Testing: is the system adequately tested?
- Documentation: is the API well-documented?

### CodeSubmit

Please organize, design, test and document your code as if it were going into production - then push your changes to the master branch. After you have pushed your code, you may submit the assignment on the assignment page.

All the best and happy coding,

The Microquest Inc. Team