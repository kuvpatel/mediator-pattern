# Integration Testing Setup

## Overview

This project uses a modern ASP.NET Core integration testing approach to verify the complete request pipeline, from the HTTP endpoint through MediatR, Entity Framework Core, SQL Server, and back to the HTTP response.

The integration tests execute against a real SQL Server instance running inside a Docker container using Testcontainers.

---

# Technology Stack

* .NET 9
* xUnit
* Entity Framework Core
* SQL Server
* Testcontainers
* FluentAssertions
* ASP.NET Core WebApplicationFactory
* Respawn (database reset)

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

The tests exercise the complete application stack instead of mocking dependencies.

---

# Project Structure

```
Mediator.Api.IntegrationTests
│
├── CustomerController
│   ├── GetCustomerTests.cs
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
* Applying EF Core migrations
* Initializing Respawn
* Resetting the database between tests
* Disposing of the container and host

This fixture is shared across all integration test classes.

---

## IntegrationTestFactory

Derived from `WebApplicationFactory<Program>`.

Responsibilities:

* Starts the API in the **Testing** environment.
* Overrides the database connection string.
* Uses the SQL Server Testcontainer database.

The factory does **not**:

* Apply migrations
* Seed data
* Reset the database

These responsibilities belong to `SqlServerFixture`.

---

## IntegrationTestBase

Provides common functionality for all integration tests.

Current helper methods include:

* `ClearDatabaseAsync()`
* `SeedCustomerAsync()`

This removes repetitive code from each test class.

---

## DatabaseSeeder

Provides reusable methods for creating test data.

Example:

```csharp
var customer = await SeedCustomerAsync();
```

Returning the created entity avoids hard-coded identity values in tests.

---

## Respawn

Respawn is used to quickly reset the database after each test.

Advantages:

* Faster than deleting records manually
* Handles related tables automatically
* Preserves the `__EFMigrationsHistory` table
* Keeps every test isolated

---

# Entity Framework Core

Database schema is managed using EF Core migrations.

The fixture automatically executes:

```csharp
await db.Database.MigrateAsync();
```

before any tests run.

No manual SQL scripts are required.

---

# Database

The integration tests use:

* SQL Server 2022
* Docker container
* Dedicated test database (`SimpleCustomerDb`)

The production or development database is never used during testing.

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
* 400 Bad Request
* 404 Not Found
* 500 Internal Server Error

The global exception handler is exercised as part of the complete request pipeline.

---

# Current Integration Tests

Implemented:

* Get customer by ID (existing customer)
* Get customer by ID (customer not found)

Additional CRUD endpoint tests are planned.

---

# Benefits of the Current Approach

* Tests the complete ASP.NET Core pipeline
* Uses a real SQL Server database
* Uses Entity Framework Core exactly as production
* Exercises MediatR handlers
* Exercises repositories
* Exercises middleware
* Exercises global exception handling
* No mocked database
* High confidence in application behaviour

---

# Future Improvements

## Short Term

* Complete CRUD endpoint integration tests
* Validate ProblemDetails responses
* Test validation failures
* Test duplicate resource scenarios
* Test pagination and filtering endpoints

## Medium Term

* Add test data builders
* Add reusable response assertion helpers
* Add GitHub Actions CI pipeline
* Measure code coverage

## Long Term

* True parallel integration testing using isolated databases
* Deterministic database naming
* Faster CI execution
* Performance and load testing

---

# Running the Tests

Run all tests:

```bash
dotnet test
```

Run a single test project:

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
* SQL Server Docker image available (pulled automatically if required)

---

# Notes

The integration test suite is intentionally isolated from the development environment.

Each test run:

1. Starts a SQL Server container.
2. Creates the ASP.NET Core test host.
3. Applies EF Core migrations.
4. Resets the database between tests using Respawn.
5. Executes tests against the full HTTP pipeline.

This provides a reliable, repeatable, and production-like testing environment.
