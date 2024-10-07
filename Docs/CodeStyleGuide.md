# Poseidon API Orchestrator - Code Style Guide

This document outlines the coding conventions, best practices, and patterns to be followed when contributing to the Poseidon project. Adhering to these standards ensures that the codebase remains consistent, clean, and easy to maintain.

## Table of Contents

- [General Guidelines](#general-guidelines)
- [Naming Conventions](#naming-conventions)
- [Formatting](#formatting)
- [Commenting](#commenting)
- [Error Handling](#error-handling)
- [Architecture & Structure](#architecture--structure)
- [Unit Testing](#unit-testing)
- [Version Control](#version-control)

---

## General Guidelines

1. **Consistency**: Follow existing patterns in the codebase. Refactor legacy code to align with these guidelines when necessary.
2. **Readability**: Write clear and understandable code. Prioritize clarity over brevity.
3. **DRY Principle (Don’t Repeat Yourself)**: Avoid code duplication by abstracting shared logic into reusable functions or components.
4. **Single Responsibility Principle**: Each class or function should handle one specific task or responsibility.
5. **Minimize Dependencies**: Utilize dependency injection to reduce tight coupling between components.

---

## Naming Conventions

### Classes & Interfaces

- **Classes**: Use PascalCase.
  - *Example*: `PassengerService`, `JwtUtility`
- **Interfaces**: Prefix with `I` and use PascalCase.
  - *Example*: `IPassengerService`, `IRepository`

### Variables & Parameters

- **Local Variables**: Use camelCase.
  - *Example*: `var passengerService = new PassengerService();`
- **Class Properties**: Use PascalCase.
  - *Example*: `public string Name { get; set; }`
- **Private Fields**: Use camelCase with an underscore prefix.
  - *Example*: `_passengerRepository`, `_mapper`
- **Constants**: Use PascalCase and prefix with `const`.
  - *Example*: `const int MaxRetryAttempts = 3;`

### Methods

- **Method Names**: Use PascalCase and clearly describe their functionality.
  - *Example*: `CreatePassenger()`, `ValidateToken()`

---

## Formatting

### Indentation

- Use **4 spaces** per indentation level.
- Avoid using tabs. Configure your editor to insert spaces when the tab key is pressed.

### Braces

- Always use braces `{}` for control structures (`if`, `for`, `while`, etc.), even for single-line statements.

  **Correct:**
  ```csharp
  if (condition)
  {
      DoSomething();
  }
  ```

  **Incorrect:**
  ```csharp
  if (condition)
      DoSomething();
  ```

### Line Length

- Limit lines to **100 characters**. Break longer lines into multiple lines for better readability.

### Whitespace

- Leave **one blank line** between class members (methods, properties, etc.).
- Do not include trailing whitespaces at the end of lines.
- Separate logical blocks within methods with a **single blank line** to enhance readability.

### File Structure

- Each file should contain **one class or interface**.
- Follow a standard file structure:
  1. Namespace imports (with `System` namespaces first).
  2. Class or interface definition.
  3. Fields, properties, constructor, methods (in that order).

---

## Commenting

### XML Documentation

- Use XML comments for **public methods and properties** to generate comprehensive API documentation.

  **Example:**
  ```csharp
  /// <summary>
  /// Creates a new passenger and saves it to the database.
  /// </summary>
  /// <param name="passengerDTO">The passenger details.</param>
  /// <returns>The newly created passenger.</returns>
  public async Task<Passenger> CreatePassengerAsync(PassengerDTO passengerDTO)
  {
      // Implementation...
  }
  ```

### Inline Comments

- Use inline comments sparingly to explain **why** certain decisions are made, not **what** the code does.

  **Example:**
  ```csharp
  // Use UTC to avoid timezone discrepancies
  var currentTime = DateTime.UtcNow;
  ```

### TODO Comments

- Use `TODO:` comments to indicate areas that require future improvements or refactoring.

  **Example:**
  ```csharp
  // TODO: Optimize this query for better performance
  ```

---

## Error Handling

1. **Use Exceptions for Exceptional Situations**: Only use exceptions for unexpected or rare conditions, not for regular control flow.
2. **Graceful Degradation**: Handle errors in a way that minimizes disruption to the user experience.
3. **Catch Specific Exceptions**: Avoid catching generic `Exception`. Instead, catch specific exceptions to handle known error conditions appropriately.
4. **Logging**: Log all exceptions with sufficient detail for troubleshooting, avoiding the inclusion of sensitive information.

  **Example:**
  ```csharp
  try
  {
      var result = await _passengerService.CreatePassengerAsync(passengerDTO);
  }
  catch (ArgumentNullException ex)
  {
      _logger.LogError(ex, "Passenger DTO was null.");
      throw new BadRequestException("Passenger details cannot be null.");
  }
  ```

---

## Architecture & Structure

The Poseidon project follows a **layered architecture** to ensure separation of concerns:

- **Controllers**: Handle HTTP requests and delegate processing to services.
- **Services**: Contain business logic and interact with repositories.
- **Repositories**: Manage data access and communication with MongoDB.
- **DTOs (Data Transfer Objects)**: Facilitate data transfer between layers.
- **Models**: Represent data entities stored in MongoDB.
- **Utilities**: Provide helper functions like JWT handling and password hashing.
- **Event Handlers**: Manage events triggered by specific actions.
- **Middlewares**: Implement cross-cutting concerns like error handling and rate limiting.

### Dependency Injection

- Register all services, repositories, and utilities in `Program.cs` using dependency injection.
- Prefer **constructor injection** to promote loose coupling and easier testing.

---

## Unit Testing

Unit tests verify the functionality of individual components in isolation. Follow these guidelines when writing unit tests:

1. **Descriptive Test Method Names**: Clearly indicate the purpose and expected outcome.

   **Example**: `CreatePassenger_ValidData_ReturnsCreatedPassenger()`

2. **Test Structure**: Use the **Arrange-Act-Assert** pattern.
   - **Arrange**: Set up the necessary objects and state.
   - **Act**: Execute the method being tested.
   - **Assert**: Verify the outcome.

3. **Mock Dependencies**: Utilize mocking frameworks like **Moq** to simulate dependencies, ensuring tests remain isolated.

4. **Cover Edge Cases**: Test scenarios with invalid inputs or unexpected conditions to ensure robustness.

---

## Version Control

Maintain a clean and organized Git repository by following these version control practices:

1. **Commit Messages**: Write clear and concise commit messages using the following structure:
   - `Feature/bugfix: Brief description of the change`

   **Example**: `Feature: Implement user registration and login functionality`

2. **Branching Strategy**:
   - `main`: Production-ready code.
   - `dev`: Ongoing development and integration.
   - **Feature Branches**: Create separate branches for each new feature or bug fix.

3. **Pull Requests**:
   - All code changes must be reviewed through pull requests before merging.
   - Include a descriptive title and detailed description of the changes.
   - Reference related issues or feature requests.
   - Ensure all tests pass before merging.

---

## Conclusion

Adhering to this **Code Style Guide** ensures that the Poseidon API Orchestrator remains clean, consistent, and maintainable. By following these conventions and best practices, contributors can enhance the codebase's readability and reliability, fostering a collaborative and efficient development environment.

---

Feel free to reach out to the project maintainers if you have any questions or need further clarification on any of these guidelines.