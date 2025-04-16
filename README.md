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

