# Poseidon API - Code Style Guide

This document outlines the coding conventions, best practices, and patterns to be followed when contributing to the Poseidon project. By adhering to these standards, we ensure that the codebase remains clean, maintainable, and easy to understand for all contributors.

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

1. **Consistency**: Follow existing patterns in the codebase. If you encounter legacy code that doesn’t follow these guidelines, consider refactoring it.
2. **Readability**: Code should be easy to read and understand. Prioritize clarity over brevity when writing code.
3. **DRY Principle (Don’t Repeat Yourself)**: Avoid duplicating code. Refactor shared logic into functions or reusable components.
4. **Single Responsibility Principle**: Each class or function should have one responsibility. Avoid adding multiple responsibilities in one place.
5. **Minimize Dependencies**: Use dependency injection where possible to minimize tight coupling between components.

---

## Naming Conventions

Adopting consistent naming conventions helps ensure that the code is intuitive to read and understand. The following are the preferred conventions:

### Classes & Interfaces

- **Classes**: Use PascalCase.
  - Example: `PassengerService`, `JwtUtility`
- **Interfaces**: Prefix with `I`, followed by PascalCase.
  - Example: `IPassengerService`, `IRepository`

### Variables & Parameters

- **Local Variables**: Use camelCase.
  - Example: `var passengerService = new PassengerService();`
- **Class Properties**: Use PascalCase.
  - Example: `public string Name { get; set; }`
- **Private Fields**: Use camelCase prefixed with an underscore (`_`).
  - Example: `_passengerRepository`, `_mapper`
- **Constants**: Use PascalCase, and prefix with `const`.
  - Example: `const int MaxRetryAttempts = 3;`

### Methods

- **Method Names**: Use PascalCase, and methods should clearly describe what they do.
  - Example: `CreatePassenger()`, `ValidateToken()`

---

## Formatting

Consistent formatting makes code easier to scan and debug. The following formatting guidelines are mandatory:

### Indentation

- Use **4 spaces** for indentation.
- Do not use tabs. Configure your editor to insert spaces when the tab key is pressed.

### Braces

- Always use braces (`{}`) for `if`, `for`, `while`, and other block statements, even for single-line statements.
  
  **Correct**:
  ```csharp
  if (condition)
  {
      DoSomething();
  }
  ```

  **Incorrect**:
  ```csharp
  if (condition)
      DoSomething();
  ```

### Line Length

- Limit lines to **100 characters**. If a line exceeds this limit, consider breaking it into multiple lines.

### Whitespace

- Leave **one blank line** between class members (methods, properties, etc.).
- Leave **no trailing whitespaces** at the end of lines.
- Separate logical blocks of code within methods by a **single blank line** to improve readability.

### File Structure

- Class files should contain **one class or interface per file**.
- Use a standard file structure:
  - Namespace imports (with `System` namespaces listed first).
  - Class definition.
  - Fields, properties, constructor, methods (in that order).

---

## Commenting

Comments are essential for providing context or explaining complex logic. However, avoid obvious or redundant comments. Let the code speak for itself.

### XML Documentation

- Use XML documentation for **public methods and properties**. This ensures that documentation tools can automatically generate useful API documentation.

  **Example**:
  ```csharp
  /// <summary>
  /// Creates a new passenger and saves it to the database.
  /// </summary>
  /// <param name="passengerDTO">The passenger details.</param>
  /// <returns>The newly created passenger.</returns>
  public async Task<Passenger> CreatePassengerAsync(PassengerDTO passengerDTO)
  {
      // Method implementation...
  }
  ```

### Inline Comments

- Use inline comments sparingly to explain **why** something is done in a specific way, rather than what is being done.
  
  **Example**:
  ```csharp
  // Use UTC to avoid timezone issues
  var currentTime = DateTime.UtcNow;
  ```

### TODO Comments

- If there’s a part of the code that requires future changes or refactoring, use `TODO:` comments.
  
  **Example**:
  ```csharp
  // TODO: Improve performance by caching this result
  ```

---

## Error Handling

Error handling is a critical aspect of the Poseidon API to ensure reliability and user-friendliness.

1. **Use Exceptions for Exceptional Situations**: Exceptions should only be used for unexpected or exceptional circumstances. Avoid using them for normal control flow.
2. **Graceful Degradation**: When possible, handle errors in a way that doesn’t interrupt the user experience.
3. **Catch Specific Exceptions**: Avoid catching generic `Exception`. Always catch specific exceptions to ensure you handle only what you expect.
4. **Logging**: Log all exceptions with enough detail to diagnose issues, but avoid logging sensitive information (e.g., passwords).

  **Example**:
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

The Poseidon project follows a layered architecture with separation of concerns between different layers:

- **Controllers**: Handle HTTP requests and return responses.
- **Services**: Contain business logic. Controllers delegate work to services.
- **Repositories**: Handle data access and interaction with MongoDB.
- **DTOs**: Used to transfer data between layers (e.g., between controllers and services).
- **Models**: Represent the data stored in MongoDB.
- **Utilities**: Provide helper functions and utilities such as JWT handling and password hashing.

### Dependency Injection

All services, repositories, and utilities must be registered in `Program.cs` via dependency injection (DI). Always prefer **constructor injection** over service location.

---

## Unit Testing

Unit tests ensure that each individual component works as expected. Follow these guidelines for writing unit tests:

1. **Test Method Names**: Use a descriptive naming pattern for test methods:
   - `MethodName_StateUnderTest_ExpectedBehavior`
   
   **Example**: `CreatePassenger_ValidData_ReturnsCreatedPassenger()`
   
2. **Test Structure**: Use the **Arrange-Act-Assert** pattern in tests:
   - **Arrange**: Set up the objects and prepare the necessary data.
   - **Act**: Perform the action you want to test.
   - **Assert**: Verify that the action produces the expected result.
   
3. **Mocking Dependencies**: Use mocking libraries (e.g., Moq) to mock dependencies such as services and repositories.

4. **Edge Cases**: Always test edge cases, including invalid inputs and error scenarios.

---

## Version Control

Follow these version control guidelines to maintain a clean and organized Git repository:

1. **Commit Messages**: Use clear and concise commit messages. Use the following structure:
   - `Feature/bugfix: What was done`
   
   **Example**: `Feature: Implement user registration and login functionality`
   
2. **Branching Strategy**:
   - `main`: Production-ready code.
   - `dev`: In-progress development. All features and bug fixes should branch from here.
   - Feature branches: Each new feature or bugfix should be developed in its own branch.

3. **Pull Requests**: All code should be reviewed via pull requests before merging into the `main` branch. Ensure that your pull request includes:
   - A description of what the code does.
   - A link to any relevant issue or feature request.
   - Any screenshots or test results, if applicable.

---

## Conclusion

By following this Code Style Guide, we ensure that the Poseidon project remains clean, scalable, and maintainable. Developers should always prioritize code readability, simplicity, and consistency. If you’re ever unsure of a decision, refer back to this guide or consult with the team.
