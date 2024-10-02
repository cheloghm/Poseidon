### Testing Guide for Poseidon API

---

## Table of Contents

1. [Introduction](#introduction)
2. [Types of Tests](#types-of-tests)
   - [Unit Tests](#unit-tests)
   - [Integration Tests](#integration-tests)
3. [Testing Frameworks Used](#testing-frameworks-used)
4. [Running Tests Locally](#running-tests-locally)
5. [Running Tests in CI/CD Pipeline](#running-tests-in-cicd-pipeline)
6. [Writing New Tests](#writing-new-tests)
   - [Writing Unit Tests](#writing-unit-tests)
   - [Writing Integration Tests](#writing-integration-tests)
7. [Test Coverage](#test-coverage)
8. [Best Practices for Testing](#best-practices-for-testing)

---

## 1. Introduction

Testing is a critical aspect of ensuring the Poseidon API's reliability, security, and correctness. This guide explains the various tests implemented in the Poseidon API project, how to run them, and how to write new tests.

We focus on two main types of tests: **Unit Tests** and **Integration Tests**, which cover different layers of the application.

---

## 2. Types of Tests

### Unit Tests

**Unit Tests** focus on testing individual components of the application, such as controllers, services, and utility classes. These tests isolate the logic of specific methods to ensure they behave as expected under various conditions. Unit tests are fast and do not require external dependencies like databases or external services.

Unit tests are located in the following directory:
```
Poseidon.Tests/UnitTests/
```

### Integration Tests

**Integration Tests** verify that various components work together correctly. These tests interact with external systems like databases (e.g., MongoDB) or external services. They test end-to-end scenarios, such as API calls or database interactions.

Integration tests are located in the following directory:
```
Poseidon.Tests/IntegrationTests/
```

---

## 3. Testing Frameworks Used

Poseidon API uses the following frameworks for testing:

1. **xUnit**: A widely used testing framework in .NET for unit testing.
2. **Moq**: A popular mocking library to simulate external dependencies like repositories or services during unit testing.
3. **FluentAssertions**: A library that makes assertions more readable and user-friendly.
4. **TestServer** (for integration tests): A tool that simulates an in-memory server to test API endpoints without running the full application.

---

## 4. Running Tests Locally

### Prerequisites

Before running the tests, ensure you have .NET SDK installed and the project dependencies restored. If not, you can restore them by running the following command:

```bash
dotnet restore
```

### Running All Tests

To run all unit and integration tests locally, navigate to the root of the `Poseidon` project and run:

```bash
dotnet test
```

### Running Specific Tests

To run tests within a specific category (unit tests or integration tests), navigate to the relevant test directory and run:

- **Unit Tests**:
  ```bash
  cd Poseidon.Tests/UnitTests
  dotnet test
  ```

- **Integration Tests**:
  ```bash
  cd Poseidon.Tests/IntegrationTests
  dotnet test
  ```

You can also run individual tests by specifying the test name:

```bash
dotnet test --filter "FullyQualifiedName~Namespace.TestClass.TestMethod"
```

---

## 5. Running Tests in CI/CD Pipeline

The tests are integrated into the CI/CD pipeline using GitHub Actions. The test suite is automatically triggered upon every push or pull request to the main or development branches.

The `ci-cd-pipeline.yml` file in the `CICD/` directory defines the steps for running the tests in the pipeline.

### Test Steps in CI/CD

1. **Unit Test Execution**: The pipeline first builds the project and runs all unit tests using `dotnet test`.
2. **Integration Test Execution**: Integration tests are run in the same manner to ensure external dependencies like MongoDB work correctly.
3. **Vulnerability Scanning**: After the tests pass, security scans (e.g., using Trivy) are performed to detect vulnerabilities in the Docker image.

---

## 6. Writing New Tests

When adding new features or making changes, it's crucial to write corresponding unit or integration tests to ensure the correctness of the implementation. This section explains how to write unit and integration tests.

### Writing Unit Tests

Unit tests should focus on small, isolated units of code such as methods in services or utilities.

1. **Create a Test Class**: In the `Poseidon.Tests/UnitTests/` directory, create a new test class that corresponds to the class you're testing.
   
   Example:
   ```csharp
   public class UserServiceTests
   {
       private readonly UserService _userService;
       private readonly Mock<IUserRepository> _userRepositoryMock;

       public UserServiceTests()
       {
           _userRepositoryMock = new Mock<IUserRepository>();
           _userService = new UserService(_userRepositoryMock.Object, /* Other dependencies */);
       }

       [Fact]
       public void CreateUser_ShouldReturnUser_WhenValidInput()
       {
           // Arrange
           var userDto = new CreateUserDTO { /* Fill with test data */ };

           // Act
           var result = _userService.CreateUser(userDto);

           // Assert
           result.Should().NotBeNull();
           result.Username.Should().Be(userDto.Username);
       }
   }
   ```

2. **Use Mocks**: Use Moq to mock dependencies such as repositories or external services. This isolates the logic of the class you're testing.

3. **Assertions**: Use FluentAssertions to assert the expected behavior in a human-readable way.

### Writing Integration Tests

Integration tests simulate the full API behavior by testing how various components interact with one another.

1. **Setup TestServer**: For API integration tests, use the `TestServer` to simulate a running server without needing to deploy the entire application.

   Example:
   ```csharp
   public class PassengerControllerIntegrationTests
   {
       private readonly HttpClient _client;

       public PassengerControllerIntegrationTests()
       {
           var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
           _client = server.CreateClient();
       }

       [Fact]
       public async Task GetAllPassengers_ShouldReturnOk_WhenCalled()
       {
           var response = await _client.GetAsync("/api/Passenger/all");

           response.StatusCode.Should().Be(HttpStatusCode.OK);
       }
   }
   ```

2. **Database Interaction**: Integration tests can include actual database calls. For example, verify that data is correctly saved to MongoDB by running queries within the test.
3. **Test API Endpoints**: Use `HttpClient` to simulate real API requests and validate the responses from the Poseidon API.

---

## 7. Test Coverage

Test coverage is a metric that shows how much of your codebase is covered by tests. While high test coverage is encouraged, ensure that you write meaningful tests that cover important logic rather than aiming solely for a number.

To check test coverage locally, install a tool like **coverlet** and run:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## 8. Best Practices for Testing

1. **Test Small Units**: Unit tests should focus on testing small pieces of code in isolation.
2. **Mock External Dependencies**: Use mocks for external dependencies to avoid network/database overhead in unit tests.
3. **Write Meaningful Tests**: Focus on writing tests that validate critical logic, edge cases, and failure scenarios.
4. **Ensure All Tests Pass**: Always ensure that all tests pass before merging code into the main branch.
5. **Run Tests Frequently**: Integrate tests into your development workflow and run them frequently.
6. **Use Descriptive Test Names**: Use test names that clearly describe the behavior being tested.
7. **Maintain Test Code**: Keep the test codebase clean and easy to maintain, just like the production code.

---

This **TestingGuide.md** provides a comprehensive explanation of how to run, write, and maintain tests for the Poseidon API project. Following these practices will ensure that the API remains reliable and free from regressions during development.