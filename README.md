# Assessment: Event-Driven Microservices Platform

This project features a resilient, event-driven microservices architecture built with .NET 9, Kafka, and Docker. It showcases decoupled DDD patterns for cross-service communication, distributed resilience, and automated CI/CD coupled with TDD and containerization.

## Architecture Overview

The system follows a **Modular Monolith/Microservices** hybrid approach within a monorepo structure to maximize developer agility while maintaining clear bounded contexts.

- **Event-Driven**: Services communicate asynchronously via Apache Kafka ([ADR-0002](docs/adr/0002-event-streaming-protocol.md)).
- **Monorepo Strategy**: Atomic commits and centralized dependency management ([ADR-0001](docs/adr/0001-monorepo-strategy.md)).
- **Shared Foundation**: A `Shared.Messaging` library centralizes resilient Kafka logic (using Polly).
- **Resilience**: Implements exponential backoff, circuit-breaker patterns (logic ready), and idempotent processing.
- **Decision Tracking**: Technical rationale is captured in [Architecture Decision Records (ADRs)](docs/adr/README.md).

### Visual Architecture

For a comprehensive understanding of the system design, refer to the following diagrams:

- **[System Context](docs/architecture/diagrams/context-diagram-simple.puml)**: High-level view of the system and external actors
- **[Component Architecture](docs/architecture/diagrams/component-diagram.puml)**: Internal service structure and boundaries
- **[Event Flow](docs/architecture/diagrams/event-flow-diagram.puml)**: Message flow through Kafka topics
- **[Sequence Interactions](docs/architecture/diagrams/sequence-diagram.puml)**: Request-response patterns and event propagation
- **[Data Model](docs/architecture/diagrams/erd.puml)**: Entity relationships across services
- **[Deployment](docs/architecture/diagrams/deployment-diagram.puml)**: Container orchestration and networking

### Services

- **User Service (Port 5001)**: Handles user registration and emits `UserCreatedEvent`.
- **Order Service (Port 5002)**: Consumes user events via a Background Service and manages the order lifecycle.
- **Shared.Messaging**: Shared plumbing for resilient Kafka Producers and Consumers.

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for local development)

### Running the App

The entire stack, including Kafka and Zookeeper, is orchestrated via Docker Compose.

```bash
docker compose up --build
```

- **Swagger UI (User Service)**: `http://localhost:5001/swagger`
- **Swagger UI (Order Service)**: `http://localhost:5002/swagger`
- **Health Checks**: Available at `/health` for all services.

## Engineering Standards

### 1. Test-Driven Development (TDD)

We follow a strict **Red-Green-Refactor** cycle.

- **Integration Tests**: Use `WebApplicationFactory` and mocks for infrastructure to ensure fast, reliable CI.
- **Test Command**: `dotnet test TakeHomeTest.sln`

### 2. Structured Logging & Observability

- **Serilog**: Configured with a `CompactJsonFormatter` for easy ingestion into ELK/Splunk.
- **Health Checks**: Standard ASP.NET Core health checks integrated with Docker orchestration.

### 3. Resilient Messaging

- **Polly Integration**: Kafka producers use exponential backoff for transient failure handling.
- **Manual Commits**: Consumers use manual offset commits to ensure exactly-once processing (At-Least-Once + Idempotency).
- **Event Schema Management**: All integration events are versioned and documented in the `Shared.Messaging` library to prevent breaking changes across services.
- **Idempotency**: Duplicate message delivery is handled through unique event ID tracking in the database, preventing duplicate processing.

### 4. Containerization & Orchestration

The development workflow is entirely **Docker-based**. We use **Docker Compose** to orchestrate the full stack, including Kafka and Zookeeper. This ensures high local fidelity and rapid onboarding, as the entire environment can be spun up with a single command.

See the [Deployment Diagram](docs/architecture/diagrams/deployment-diagram.puml) for container orchestration details and network topology.

### 5. Continuous Integration & Delivery (CI/CD)

- **GitHub Actions**: Automated CI/CD pipeline using GitHub Actions for quick feedback and deployment.

## Future Recommendations

### Security

- **Distroless Images**: Move to bitnami or Google distroless base images to reduce attack surface.
- **Non-Root User**: Configure Dockerfiles to run as non-root for production.
- **Secrets Management**: Use environment variables for sensitive data. Later a secret management system should be implemented.

### CI/CD & DevOps

- **GHCR Integration**: Automate image pushing to GitHub Container Registry/Dockerhub.
- **Helm Charts**: Prepare for K8s deployment using Helm for environment modeling.

### Monitoring

- **Prometheus/Grafana**: Export metrics via OpenTelemetry.
- **ELK Stack**: Centralize logs for cross-service tracing (Correlation IDs).

### Workflow Details

- **Branch Strategy**: Feature-based branching with mandatory PR reviews.
- **Cleanup**: Housekeeping via periodic branch pruning.

## Key Architectural Decisions

The following ADRs document critical technical decisions and their rationale:

1. **[Monorepo Strategy](docs/adr/0001-monorepo-strategy.md)**: Why we chose a single repository over multiple repositories

   - Enables atomic commits for cross-service schema changes
   - Centralized dependency management via Directory.Packages.props
   - Simplified CI/CD configuration

2. **[Kafka for Event Streaming](docs/adr/0002-event-streaming-protocol.md)**: Why Kafka over RabbitMQ
   - Event durability and replayability for audit trails
   - High-throughput streaming for future telemetry features
   - Strong ordering guarantees within partitions

For the complete list of architectural decisions, see the [ADR Directory](docs/adr/README.md).

## AI-Assisted Development

This project was developed utilizing AI as an advanced documentation and research tool. Full architectural control and logical decision-making remained under human oversight, with AI serving as a catalyst for infrastructure scaffolding .

### 1. Boilerplate & Infrastructure

- **Template Generation**: GenAI was strategically utilized as a high-efficiency alternative to traditional project templates, generating service skeletons and initial Docker configurations.
- **TDD Verification**: Once the core business logic was architected, AI was used to assist in the generation of unit and integration test cases (like researching boundary conditions), which were subsequently reviewed and refined to ensure full coverage of boundary conditions.

### 2. Technical Validation & Research

- **Decision Support**: The AI agent (**Gemini**) functioned as an enhanced "StackOverflow + Professor" to contest architectural decisions, validate compliance with DDD principles, and research alternative messaging strategies (e.g., Kafka vs. RabbitMQ).
- **Architecture as Code (PlantUML)**: Diagrams were generated to visualize the system design and are version-controlled alongside the code to maintain a single source of truth and prevent "documentation drift." All generated diagrams were manually verified for architectural accuracy.
