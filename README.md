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


#####################################################

Goal: Set up a baseline project structure using Clean Architecture.

    Deliverable:
        Solution structure with:
            Banking.Domain
            Banking.Application
            Banking.Infrastructure
            Banking.API
            Each project references only what it should (Domain has no deps, API has all)

    Assessment Focus:
        Correct project references
        Separation of concerns
        Naming conventions

    Bonus: Add .sln file and GitHub repo setup (feature/clean-arch-scaffold)


#####################################################

Goal: Create a rich domain model using proper DDD patterns.

Deliverable:
    In Banking.Domain.Entities:
        Customer aggregate root with:
        CustomerId (Value Object)
        FullName, EmailAddress (Value Objects)
        List<BankAccount>
        Method: OpenAccount(initialDeposit)

    Value Objects:
        EmailAddress with validation
        Money (amount + currency)

    Domain Event: CustomerRegisteredEvent

Assessment Focus:
    Aggregate boundaries and invariants
    Use of private setters and encapsulation
    Constructor enforcement
    Proper use of domain events

Bonus: Make entities immutable where possible


#####################################################


Description

    Goal: Represent core banking behavior cleanly in the domain layer.
        Deliverable:
            BankAccount entity with:
            AccountId (Guid)
            Balance (Money)
            CustomerId
    Domain behavior methods:
        Deposit(Money)
        Withdraw(Money)
        GetBalance()

    Throw exceptions on business rule violations (e.g., overdrafts)

    Assessment Focus:
        Encapsulated logic (no public setters!)
        Use of value objects to reduce primitive obsession
        Domain consistency rules (e.g., no negative balances)

    Bonus: Add AccountOverdrawnEvent domain event


#####################################################

Description

    Goal: Separate transport and internal models cleanly.
        Deliverable:
            In Banking.Application.DTOs:
                CustomerDto, CreateCustomerRequest, CreateCustomerResponse
            In Banking.API.Controllers:
                Accept CreateCustomerRequest
                Map to domain model
                Return CreateCustomerResponse

        Assessment Focus:
            Flatten nested domain objects where appropriate
            DTOs are never reused in the domain
            Mapping kept in Application layer or via services (not in controllers)

    Bonus: Use AutoMapper or manual mapping extension methods


#####################################################


Goal: Abstract infrastructure behind clean repository contracts.

    Deliverable:
        In Banking.Domain.Repositories:
            ICustomerRepository:
                Task<Customer?> GetByIdAsync(Guid id);
                Task AddAsync(Customer customer);

        In Banking.Infrastructure.Persistence:
            In-memory or stub implementation

    Assessment Focus:
        Interface-only in Domain layer
        No implementation logic leaking into Application
        Async signatures

    Bonus: Add IBankAccountRepository