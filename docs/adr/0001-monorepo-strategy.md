# ADR 0001: Monorepo Strategy for Microservices

## Status

Accepted

## Date

2026-01-11

## Context

As we scale the ExploreSG platform into multiple microservices (UserService, OrderService, etc.), we need a repository strategy that balances development velocity, code sharing, and operational complexity.

## Decision

We have chosen a **Monorepo** strategy using a single .NET Solution with a shared `src/Shared` directory.

## Consequences

- **Pros**:
  - **Atomic Commits**: Cross-service changes (e.g., updating a shared messaged schema) can be done in a single pull request.
  - **Simpler Dependency Management**: Central Package Management (CPM) ensures all services use the same versions of libraries (EF Core, Kafka, etc.).
  - **Shared Tooling**: CI/CD pipelines, linting rules, and Docker configurations are centralized and consistent.
- **Cons**:
  - **CI Complexity**: Pipelines must be optimized (e.g., using `nx` or custom scripts) to only build/test changed services to avoid long build times.
  - **Tight Coupling Risk**: Developers might be tempted to share internal logic via `Shared` projects instead of proper integration events.

## Alternatives Considered

- **Multirepo**: Rejected for this stage as it would introduce significant overhead for maintaining 10+ shared messaging libraries and keeping CI/CD configurations in sync across 5+ repositories.
