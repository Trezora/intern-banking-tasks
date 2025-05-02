# intern-banking-tasks
simple banking task

Class: Customer
    Properties:
        CustomerId (GUID)
        FullName (string)
        EmailAddress (string)
        DateOfBirth (DateTime)
        Constructor should require all fields
        Method GetCustomerSummary() returns a formatted string

Assessment Focus:
    Proper use of types, access modifiers, and constructor
    Encapsulation and property naming
    Clear output formatting for summary

Bonus: Return age from DateOfBirth

######################################################

Goal: Practice control flow and input/output

Scenario:
        Simulate a minimal CLI app for creating a new bank account.

Deliverable:
        Ask user for Customer Name, Email, Initial Deposit
        Instantiate Customer and BankAccount objects

    Display account summary:
        Customer Name
        Account Number (generate random)
        Initial Balance

Classes to Create:
    BankAccount:
        Properties: AccountNumber, Balance, Customer
        Method: PrintAccountSummary()

Assessment Focus:
    Class composition (Account → Customer)
    Data input validation (positive balance)
    Object initialization flow

Bonus: Loop to allow creating multiple accounts

######################################################

Goal: Introduce unit testing and testable logic

Deliverable:
    Method: Deposit(decimal amount) on BankAccount
        Validates that amount > 0
        Increases balance

    Unit test project using xUnit:
        Test deposit increases balance
        Test deposit with a negative amount throws an exception

Assessment Focus:
    Method design + testability
    Naming of test cases
    Clean assertions


######################################################

Task 1: Design a BankAccount Class with Behavior

Goal: Model a proper object with data + behavior (not just a data bag)

Deliverable:
    Class: BankAccount
    Private fields for Balance, AccountNumber
    Constructor sets AccountNumber and initial balance

Methods:
    Deposit(decimal amount)
    Withdraw(decimal amount)
    GetBalance()

Requirements:
    Guard conditions (no negative deposits, no overdrafts)
    Use Guid for AccountNumber
    Use private setters to enforce encapsulation

Assessment Focus:
    Class structure
    Use of private fields
    Validation inside methods
    Encapsulation

Bonus: Return OperationResult with success/failure on Withdraw


######################################################

Goal: Practice composition and constructor-based dependency

Deliverable:
    Class: Customer
    Properties:
        CustomerId (GUID)
        FullName, EmailAddress
        List of BankAccount (owned accounts)

    Method:
        OpenNewAccount(initialDeposit) → creates and adds new BankAccount
        ListAccounts() → returns summaries

    Assessment Focus:
        Object composition (Customer owns BankAccount)
        Controlled object creation via method
        Use of constructors and immutability

    Bonus: Prevent duplicate emails (case-insensitive)


#####################################################
    
Goal: Apply Single Responsibility Principle and Open/Closed Principle

Deliverable:
    Refactor logic so:
    BankAccount focuses only on state and rules
    Move logic like overdraft notification or logging into separate service class (e.g., AccountNotifier)
    Add interface ITransactionLogger with implementation ConsoleTransactionLogger

Assessment Focus:
    Class responsibility separation
    Open/closed extension via interfaces
    Interface implementation and dependency usage

Bonus: Inject logger into account class (constructor or method param)

#####################################################

Goal: Demonstrate Interface Segregation and test-friendly design

Scenario:
    You need a service that processes customer onboarding and creates their default account.

Deliverable:
    Interface: ICustomerOnboardingService
    Implementation: CustomerOnboardingService
        Takes a Customer and auto-creates an account with base balance
        Logs onboarding via ILogger
    Write unit test mocking the logger and asserting onboarding logic

Assessment Focus:
    Interface-first design
    Testability via abstraction
    Constructor injection and service separation

Bonus: Introduce simple validation rules via separate class (e.g., CustomerValidator)


Goal: Reinforce clean code practices

Deliverable:
    Push all work into GitHub repo under feature/oop-srp-refactor
    Open a Pull Request with:
    Summary of changes
    Justification of design choices
    Screenshots of test results

Assessment Focus:
    Code readability and structure
    Naming, formatting, separation of concerns
    PR quality and professionalism