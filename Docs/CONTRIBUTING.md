## `CONTRIBUTING.md`

```markdown
# Contributing to Poseidon API Orchestrator

First off, thank you for considering contributing to the **Poseidon API Orchestrator**! 🎉 Your efforts are greatly appreciated.

The following is a set of guidelines to help you contribute effectively to this project. By participating, you agree to abide by these rules to maintain a welcoming and productive environment for all contributors.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How to Contribute](#how-to-contribute)
  - [Reporting Issues](#reporting-issues)
  - [Suggesting Enhancements](#suggesting-enhancements)
  - [Pull Requests](#pull-requests)
- [Coding Standards](#coding-standards)
- [Commit Messages](#commit-messages)
- [License](#license)

---

## Code of Conduct

By participating in this project, you agree to uphold the [Code of Conduct](CODE_OF_CONDUCT.md). Please read it to understand the expectations for all contributors.

## How to Contribute

### Reporting Issues

If you encounter any bugs or have suggestions for improvements, please open an issue in the [GitHub Issues](https://github.com/cheloghm/Poseidon/issues) section. When reporting a bug, include the following:

- **Description**: A clear and concise description of what the bug is.
- **Steps to Reproduce**: Detailed steps to help us reproduce the issue.
- **Expected Behavior**: What you expected to happen.
- **Screenshots**: If applicable, add screenshots to help explain your problem.
- **Environment**: Information about your environment (e.g., OS, browser, API version).

### Suggesting Enhancements

We welcome your ideas for enhancements! To suggest a new feature or improvement:

1. Check if the suggestion already exists in the [Issues](https://github.com/cheloghm/Poseidon/issues) section.
2. If not, open a new issue with a detailed description of your proposal.
3. Provide any relevant context or examples to illustrate your idea.

### Pull Requests

Contributions through pull requests are highly encouraged. Here's how to get started:

1. **Fork the Repository**: Click the "Fork" button at the top-right corner of the repository page.

2. **Clone Your Fork**:
   ```bash
   git clone https://github.com/your-username/Poseidon.git
   ```

3. **Create a Branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Make Your Changes**: Implement your feature or fix, adhering to the [Coding Standards](#coding-standards).

5. **Run Tests**: Ensure all existing tests pass and add new tests for your changes.

6. **Commit Your Changes**:
   ```bash
   git commit -m "docs: Add XML documentation to PassengerController and UserController"
   ```

7. **Push to Your Fork**:
   ```bash
   git push origin feature/your-feature-name
   ```

8. **Open a Pull Request**: Navigate to the original repository and click "Compare & pull request". Provide a clear description of your changes.

## Coding Standards

Please refer to the [Code Style Guide](./Docs/CodeStyleGuide.md) for detailed information on coding conventions, formatting, naming conventions, and best practices to maintain a consistent and high-quality codebase.

## Commit Messages

Use clear and descriptive commit messages to convey the purpose of your changes. Follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification for consistency.

**Example:**
```
docs: Add XML documentation to PassengerController and UserController

- Implemented comprehensive XML comments for all endpoints in PassengerController
- Added XML summaries, parameter descriptions, and return value explanations to each method
- Enhanced Swagger integration by providing detailed method descriptions
- Improved code readability and maintainability through consistent documentation practices
```

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE).

---

Thank you for contributing to the Poseidon API Orchestrator! Your support helps make this project better for everyone.

```

---

## Commit Message

When adding the `CONTRIBUTING.md` file along with the XML documentation updates to the `PassengerController` and `UserController`, a comprehensive commit message is essential to clearly communicate the nature of the changes. Below is an example of a well-structured commit message following the Conventional Commits specification.

---

```
docs: Add CONTRIBUTING.md and XML documentation to controllers

- Introduced CONTRIBUTING.md to guide new contributors on how to effectively contribute to the project.
- Implemented comprehensive XML comments for all endpoints in PassengerController and UserController.
- Enhanced Swagger integration with detailed method descriptions for better API documentation.
- Improved code readability and maintainability by adhering to documentation best practices.
```

---

### Breakdown of the Commit Message

1. **Header (`docs: Add CONTRIBUTING.md and XML documentation to controllers`):**
   - **`docs`**: This prefix indicates that the commit relates to documentation changes.
   - **Summary**: Clearly states the addition of the `CONTRIBUTING.md` file and the XML documentation enhancements in the controllers.

2. **Body:**
   - **Introduced CONTRIBUTING.md**: Specifies the creation of the contributing guide, helping new contributors understand how to participate.
   - **Implemented comprehensive XML comments**: Details the addition of XML comments to the `PassengerController` and `UserController`, enhancing code documentation.
   - **Enhanced Swagger integration**: Notes that Swagger documentation has been improved, which aids in API usability and developer experience.
   - **Improved code readability and maintainability**: Highlights the overall benefit of the changes, emphasizing better code quality.

### Why This Commit Message Works

- **Clarity**: It clearly outlines what was changed (`Add CONTRIBUTING.md and XML documentation to controllers`), making it easy for team members to understand the scope of the commit.
  
- **Categorization**: Using the `docs` prefix helps in categorizing the commit, making it easier to filter and identify documentation-related changes in the project history.
  
- **Detail**: The body provides enough detail to understand the specific changes without being overly verbose, ensuring that the commit message is informative yet concise.
  
- **Relevance**: By mentioning both the contributing guide and the XML documentation, it covers all the key updates made in this commit.

### Additional Tips for Commit Messages

- **Be Descriptive**: Ensure that the commit message accurately reflects the changes made.
  
- **Use the Imperative Mood**: Start the commit message with a verb (e.g., Add, Implement, Enhance) to maintain consistency.
  
- **Keep It Concise**: While detail is important, avoid unnecessary verbosity. Aim for clarity and brevity.
  
- **Follow Conventions**: Adhering to a commit message convention like Conventional Commits improves readability and automation compatibility (e.g., generating changelogs).

By following this structured approach to commit messaging, you ensure that your contributions are well-documented and easily understandable by all project stakeholders.

---
