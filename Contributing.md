# Contributing Guidelines

## How to Contribute

| Step | Action                                                                                                                                         |
| ---- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| 1.   | **Fork** the repository and clone it locally.                                                                                                  |
| 2.   | **Create a new branch**:<br>Use the format `feature/<your-feature>` or `fix/<your-bug>`.                                                       |
| 3.   | **Write clear commit messages** following our [Commit Message Guidelines](#-commit-message-guidelines).                                        |
| 4.   | **Open a Pull Request (PR)**:<br>Ensure your branch is updated with the `main` branch. Provide a clear description and link the related issue. |

## Code Style

- Follow **C# coding conventions** and .NET best practices
- Maintain consistency in **naming conventions** (PascalCase for public members, camelCase for private fields)
- Use **async/await** patterns for asynchronous operations
- Follow the **dependency injection** pattern established in the project
- Keep controllers thin - business logic belongs in services
- Use meaningful variable and method names

## Project Structure

```
src/
├── OrderService/      # Order management microservice
├── UserService/       # User management microservice
docs/
├── architecture/      # PlantUML diagrams
└── adr/              # Architecture Decision Records
```

## Running Tests

```bash
# Run all tests
dotnet test

# Run tests for specific service
dotnet test src/OrderService.Tests/OrderService.Tests.csproj
```

## Building and Running

```bash
# Build the solution
dotnet build

# Run with Docker Compose
docker-compose up --build

# Run individual service
cd src/UserService
dotnet run
```

## Reporting Issues

- Check the existing issues before raising a new one.
- When reporting, please include:
  - A **clear title**
  - Steps to **reproduce the issue**
  - Expected vs actual behavior
  - Logs or error messages, if relevant
  - Environment details (.NET version, OS)

## Commit Message Guidelines

We follow a structured commit format to ensure traceability and consistency.

### Format

```
GIC-XX-TYPE: Title Case Summary

Optional longer description.

Closes #XX
```

| Component       | Description                                                                                 |
| --------------- | ------------------------------------------------------------------------------------------- |
| `GIC-XX`        | Replace with the GitHub issue number (e.g., `GIC-42`) or use `NO-ISSUE` for general changes |
| `TYPE`          | One of the [Commit Types](#-commit-types) listed below, always **uppercase**                |
| `Title`         | Short, descriptive summary in **Title Case**                                                |
| `Body` (opt.)   | Provide additional context or rationale                                                     |
| `Footer` (opt.) | `Closes #XX` (optional but useful for automation)                                           |

### Commit Types

| Type       | Description                                    |
| ---------- | ---------------------------------------------- |
| `FEAT`     | A new feature                                  |
| `FIX`      | A bug fix                                      |
| `DOCS`     | Documentation updates                          |
| `STYLE`    | Code formatting or styling (no logic changes)  |
| `REFACTOR` | Code refactoring without feature or bug change |
| `TEST`     | Adding or updating tests                       |
| `CHORE`    | Maintenance tasks, dependency updates, etc.    |
| `BUILD`    | Changes to build process or Docker configs     |

### Examples

#### Feature Addition

```
GIC-42-FEAT: Add Input Validation to Order Service

Implemented FluentValidation for order creation endpoint.
Added unit tests for validation logic.

Closes #42
```

#### Bug Fix

```
GIC-08-FIX: Handle Kafka Connection Timeout

Added retry policy with exponential backoff for Kafka producer.
Prevents service crash on temporary Kafka unavailability.

Closes #8
```

#### Documentation Update

```
NO-ISSUE-DOCS: Update API Usage in README

Added examples for creating users and orders.
Included sample cURL commands for testing.
```

#### Refactoring

```
NO-ISSUE-REFACTOR: Extract Kafka Configuration to Separate Class

Improved code organization and testability.
```

#### Chore

```
GIC-33-CHORE: Upgrade to .NET 9.0

Updated all project files and dependencies.
Verified Docker images build successfully.

Closes #33
```

## Git Tips

Use this CLI-friendly command:

```bash
git commit -m "GIC-XX-TYPE: Title Case Summary" -m "Optional detailed description"
```

For example:

```bash
git commit -m "GIC-07-FIX: Resolve Database Context Threading Issue" -m "Changed DbContext lifetime to Scoped to prevent concurrent access errors."
```

## Before Submitting a Pull Request

- [ ] Your branch is updated with `main`
- [ ] Code compiles without warnings (`dotnet build`)
- [ ] Tests are passing (`dotnet test`)
- [ ] Code follows .NET conventions and project style
- [ ] Commit messages follow the format
- [ ] Related issue(s) are referenced in the PR
- [ ] Docker Compose builds and runs successfully (`docker-compose up --build`)
- [ ] API endpoints tested manually or via automated tests

## Architecture Decisions

For significant architectural changes:

1. Create an ADR (Architecture Decision Record) in `docs/adr/`
2. Follow the existing ADR format (see `adr-001-event-schema.md`)
3. Reference the ADR in your PR description

## Code Review Process

- All PRs require review before merging
- Address review comments promptly
- Keep PRs focused and reasonably sized
- Update your PR based on feedback

---

By following these practices, we can maintain a high-quality, collaborative codebase that's easy to manage and scale.
