# Integration Testing Setup

## Overview

This project uses a modern ASP.NET Core integration testing approach to verify the complete request pipeline, from the HTTP endpoint through MediatR, Entity Framework Core, SQL Server, and back to the HTTP response.

The integration tests execute against a real SQL Server instance running inside a Docker container using Testcontainers. The tests exercise the complete application stack without mocking repositories or the database.

---

# Technology Stack

* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server 2022
* MediatR
* xUnit
* FluentAssertions
* Testcontainers
* ASP.NET Core WebApplicationFactory

---

# Architecture

```
Integration Test
        │
        ▼
HttpClient
        │
        ▼
WebApplicationFactory
        │
        ▼
ASP.NET Core API
        │
        ▼
Controller
        │
        ▼
MediatR
        │
        ▼
Handler
        │
        ▼
Repository
        │
        ▼
Entity Framework Core
        │
        ▼
SQL Server Testcontainer
```

---

# API Endpoints

| Method | Endpoint             | Description                  |
| ------ | -------------------- | ---------------------------- |
| GET    | `/api/customer`      | Returns all customers        |
| GET    | `/api/customer/{id}` | Returns a customer by Id     |
| POST   | `/api/customer`      | Creates a new customer       |
| PUT    | `/api/customer/{id}` | Updates an existing customer |
| DELETE | `/api/customer/{id}` | Deletes a customer           |

---

# Database

The integration tests execute against a SQL Server database hosted inside a Docker container.

## Customers Table

| Column    | Type                        |
| --------- | --------------------------- |
| Id        | int (Identity, Primary Key) |
| FirstName | nvarchar(50)                |
| LastName  | nvarchar(50)                |
| Email     | nvarchar(100)               |

Entity Framework Core migrations are automatically applied before the tests execute.

---

# Project Structure

```
Mediator.Api.IntegrationTests
│
├── Customers
│   ├── GetCustomersTests.cs
│   ├── GetCustomerByIdTests.cs
│   ├── CreateCustomerTests.cs
│   ├── UpdateCustomerTests.cs
│   └── DeleteCustomerTests.cs
│
├── Infrastructure
│   ├── SqlServerFixture.cs
│   ├── IntegrationTestFactory.cs
│   ├── IntegrationTestBase.cs
│   ├── IntegrationTestCollection.cs
│   └── DatabaseSeeder.cs
│
└── ...
```

---

# Test Infrastructure

## SqlServerFixture

Responsible for:

* Starting the SQL Server Docker container
* Creating the ASP.NET Core test host
* Applying Entity Framework Core migrations
* Sharing the SQL Server container across all integration tests
* Disposing of the container and test host

---

## IntegrationTestFactory

Derived from `WebApplicationFactory<Program>`.

Responsibilities:

* Starts the API using the **Testing** environment.
* Overrides the application's connection string.
* Connects the API to the SQL Server Testcontainer.

The factory does **not**:

* Apply migrations
* Seed data

These responsibilities belong to `SqlServerFixture` and `DatabaseSeeder`.

---

## IntegrationTestBase

Provides common functionality for integration tests.

Current helper methods include:

* `SeedCustomerAsync()`
* `SeedCustomersAsync(int count)`
* `ClearDatabaseAsync()`

This removes repetitive setup code from the test classes.

---

## DatabaseSeeder

Provides reusable methods for creating test data.

Examples:

```csharp
var customer = await SeedCustomerAsync();

var customers = await SeedCustomersAsync(5);
```

Returning the created entities avoids hard-coded identity values within tests.

---

# Entity Framework Core

The database schema is managed using Entity Framework Core migrations.

The test fixture automatically executes:

```csharp
await db.Database.MigrateAsync();
```

before any tests are run.

This ensures the database schema always matches the application model.

---

# Testing Pattern

Each integration test follows the Arrange / Act / Assert pattern.

Example:

```csharp
await ClearDatabaseAsync();

var customer = await SeedCustomerAsync();

var response = await Client.GetAsync($"/api/customer/{customer.Id}");

response.StatusCode.Should().Be(HttpStatusCode.OK);
```

---

# Exception Handling

The tests verify that the API returns the correct HTTP status codes.

Examples include:

* 200 OK
* 201 Created
* 204 No Content
* 400 Bad Request
* 404 Not Found
* 500 Internal Server Error

The global exception handler is exercised as part of the complete request pipeline.

---

# Current Integration Tests

## GET /api/customer

* Returns all customers
* Returns an empty collection when no customers exist
* Returns the expected customer data

---

## GET /api/customer/{id}

* Returns a customer when the customer exists
* Returns 404 when the customer does not exist

---

## POST /api/customer

* Creates a customer successfully
* Returns 400 when the request is invalid

---

## PUT /api/customer/{id}

* Updates an existing customer
* Returns 404 when the customer does not exist
* Returns 400 when the request is invalid

---

## DELETE /api/customer/{id}

* Deletes an existing customer
* Returns 404 when the customer does not exist
* Verifies the customer count decreases after deletion

---

# Benefits of the Current Approach

* Tests the complete ASP.NET Core request pipeline
* Uses a real SQL Server database
* Uses Entity Framework Core exactly as production
* Exercises MediatR handlers
* Exercises repositories
* Exercises middleware
* Exercises global exception handling
* No mocked database
* High confidence in application behaviour

---

# Testing Design Decisions

The integration testing framework was designed to closely mirror the application's production environment while remaining fast, reliable, and maintainable.

## Why Testcontainers?

A SQL Server instance is hosted inside a Docker container for the integration tests.

This approach provides several benefits:

* Tests execute against a real SQL Server instance rather than an in-memory database.
* Every developer and CI pipeline uses the same database environment.
* No local SQL Server installation is required.
* Test environments are isolated from development and production databases.

---

## Why Entity Framework Core Migrations?

The test database schema is created using Entity Framework Core migrations.

Benefits include:

* The database schema always matches the application model.
* No manual SQL scripts need to be maintained.
* Database changes are version controlled alongside the application code.
* New developers can build the database automatically when running the tests.

---

## Why WebApplicationFactory?

`WebApplicationFactory<Program>` hosts the ASP.NET Core application in memory during testing.

This allows the tests to execute the complete HTTP request pipeline, including:

* Routing
* Model binding
* Request validation
* Dependency Injection
* Middleware
* Exception handling
* Controller actions

The tests interact with the API in the same way as a client application.

---

## Why Integration Tests Instead of Mocking?

The objective of these tests is to validate that all application components work together correctly.

Each test exercises:

* HTTP endpoints
* Controllers
* MediatR request handlers
* Repository implementations
* Entity Framework Core
* SQL Server

By avoiding mocked repositories and databases, the tests provide greater confidence that the application behaves correctly in a production-like environment.

---

## Test Data Management

Test data is created using a shared `DatabaseSeeder` utility.

This approach provides:

* Reusable seed methods across all test classes.
* Predictable and consistent test data.
* Reduced duplication within the test suite.
* Easier maintenance as new test scenarios are added.

The seeder supports creating both individual customers and a configurable number of customers, allowing each test to create only the data it requires.

---

## Test Structure

All integration tests follow the **Arrange / Act / Assert** pattern.

Common functionality is shared through `IntegrationTestBase`, reducing duplication and ensuring consistency across the test suite.

Each test is responsible for:

1. Preparing the required test data.
2. Executing an HTTP request against the API.
3. Verifying both the HTTP response and, where appropriate, the resulting database state.

This structure makes the tests easy to read, maintain, and extend as the application evolves.

---

# Future Improvements

## Short Term

* Validate `ProblemDetails` response bodies
* Test duplicate customer scenarios
* Test validation edge cases

## Medium Term

* Introduce Test Data Builders
* Add reusable assertion helpers
* Configure GitHub Actions or Azure DevOps CI
* Generate code coverage reports

## Long Term

* Parallel integration testing with isolated databases
* Performance and load testing

---

# Running the Tests

Run all tests:

```bash
dotnet test
```

Run the integration test project:

```bash
dotnet test Mediator.Api.IntegrationTests
```

Run with code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

# Prerequisites

* .NET 9 SDK
* Docker Desktop running
* SQL Server Docker image available (downloaded automatically if required)

---

# Notes

The integration test suite is isolated from the development environment.

Each test run:

1. Starts a SQL Server Docker container.
2. Creates the ASP.NET Core test host.
3. Applies Entity Framework Core migrations.
4. Seeds the database as required by each test.
5. Executes requests through the complete HTTP pipeline.

This provides a reliable, repeatable, production-like testing environment while ensuring the application behaves correctly from the HTTP endpoint through to the database.
