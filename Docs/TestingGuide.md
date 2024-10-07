# Poseidon API Orchestrator - Testing Guide

## Table of Contents

1. [Introduction](#1-introduction)
2. [Types of Tests](#2-types-of-tests)
   - [Unit Tests](#unit-tests)
   - [Integration Tests](#integration-tests)
3. [Testing Frameworks Used](#3-testing-frameworks-used)
4. [Running Tests Locally](#4-running-tests-locally)
5. [Running Tests in CI/CD Pipeline](#5-running-tests-in-cicd-pipeline)
6. [Writing New Tests](#6-writing-new-tests)
   - [Writing Unit Tests](#writing-unit-tests)
   - [Writing Integration Tests](#writing-integration-tests)
7. [Test Coverage](#7-test-coverage)
8. [Best Practices for Testing](#8-best-practices-for-testing)

---

## 1. Introduction

Testing is essential for ensuring the **Poseidon API Orchestrator** operates reliably and securely. This guide outlines the testing approaches used in the project, focusing on **Unit Tests** and **Integration Tests**. Effective testing helps identify and fix issues early, maintaining high code quality and performance.

---

## 2. Types of Tests

### Unit Tests

**Unit Tests** evaluate individual components, such as functions or classes, in isolation. They ensure that each part of the application behaves as expected under various conditions.

- **Purpose**: Validate the functionality of specific units of code.
- **Location**: `Poseidon.Tests/UnitTests/`
- **Characteristics**:
  - Fast execution
  - No external dependencies

### Integration Tests

**Integration Tests** assess how different components interact with each other and with external systems like databases. They verify that the integrated parts of the application work together seamlessly.

- **Purpose**: Test interactions between multiple components or systems.
- **Location**: `Poseidon.Tests/IntegrationTests/`
- **Characteristics**:
  - May involve external dependencies
  - Slower execution compared to unit tests

---

## 3. Testing Frameworks Used

- **xUnit**: A flexible and extensible testing framework for .NET, used for writing and running tests.
- **Moq**: A mocking library that allows simulation of dependencies, enabling isolated unit testing.
- **FluentAssertions**: Provides readable and fluent syntax for writing test assertions.
- **TestServer**: Facilitates integration testing by simulating an in-memory server environment.

---

## 4. Running Tests Locally

### Prerequisites

- Ensure the **.NET SDK** is installed.
- Restore project dependencies using:
  ```bash
  dotnet restore
  ```

### Running All Tests

Navigate to the project root and execute:
```bash
dotnet test
```

### Running Specific Tests

To run unit tests:
```bash
cd Poseidon.Tests/UnitTests
dotnet test
```

To run integration tests:
```bash
cd Poseidon.Tests/IntegrationTests
dotnet test
```

---

## 5. Running Tests in CI/CD Pipeline

Tests are integrated into the CI/CD pipeline using **GitHub Actions**. Every push or pull request triggers the pipeline to:

1. **Build the Project**: Compiles the application.
2. **Run Unit Tests**: Executes all unit tests.
3. **Run Integration Tests**: Executes all integration tests.
4. **Vulnerability Scanning**: Scans Docker images for security vulnerabilities.

Successful test execution ensures that only quality code is merged and deployed.

---

## 6. Writing New Tests

### Writing Unit Tests

1. **Create a Test Class**: Corresponding to the class under test.
2. **Mock Dependencies**: Use **Moq** to simulate external dependencies.
3. **Write Test Methods**: Focus on specific functionalities and edge cases.
4. **Assert Outcomes**: Use **FluentAssertions** for readable assertions.

**Example**:
```csharp
public class UserServiceTests
{
    [Fact]
    public void CreateUser_ShouldReturnUser_WhenValidInput()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### Writing Integration Tests

1. **Setup Test Environment**: Utilize **TestServer** to simulate the API.
2. **Execute API Calls**: Use **HttpClient** to interact with endpoints.
3. **Verify Responses**: Ensure the API behaves as expected with real dependencies.

**Example**:
```csharp
public class PassengerControllerIntegrationTests
{
    [Fact]
    public async Task GetAllPassengers_ShouldReturnOk()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

---

## 7. Test Coverage

**Test Coverage** measures the extent to which the codebase is tested. Aim for high coverage to ensure most of the code is validated by tests.

- **Tools**: Use **coverlet** to generate coverage reports.
- **Command**:
  ```bash
  dotnet test --collect:"XPlat Code Coverage"
  ```
- **Goal**: Strive for meaningful coverage, focusing on critical and complex parts of the application.

---

## 8. Best Practices for Testing

- **Isolate Tests**: Ensure unit tests do not depend on external systems.
- **Use Mocks Appropriately**: Simulate dependencies to test components in isolation.
- **Write Descriptive Test Names**: Clearly indicate the purpose and expected outcome.
- **Maintain Test Independence**: Tests should not affect each other's outcomes.
- **Regularly Update Tests**: Reflect changes in the codebase to keep tests relevant.
- **Prioritize Critical Paths**: Focus testing efforts on the most important and error-prone areas of the application.
- **Automate Testing**: Integrate tests into CI/CD pipelines for continuous validation.

---
