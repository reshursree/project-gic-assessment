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

This project was developed with AI serving **strictly as a research assistant and documentation tool**. All architectural decisions, design patterns, and implementation logic were **human-driven and fully understood** by the developer. AI was leveraged to accelerate repetitive tasks and validate technical approaches, similar to using Stack Overflow or technical documentation.

### 1. Boilerplate & Infrastructure Assistance

- **Template Generation**: AI was used to accelerate the creation of initial service skeletons and Docker configurations, similar to using `dotnet new` templates. All generated code was reviewed, understood, and modified as needed.
- **TDD Test Scaffolding**: After designing the test strategy and business logic, AI assisted in generating test case structures and researching edge cases. All tests were manually reviewed, validated, and refactored to ensure correctness.

### 2. Technical Research & Validation

- **Decision Support**: AI (**Gemini**) was used as a research tool to explore architectural trade-offs, validate DDD principles, and investigate messaging patterns (e.g., Kafka vs. RabbitMQ). All final decisions were made by the developer based on project requirements.
- **Architecture Diagrams (PlantUML)**: AI assisted in generating initial PlantUML syntax for system diagrams. All diagrams were manually verified for architectural accuracy, modified to reflect the actual implementation, and version-controlled as living documentation.

### Development Approach

- **Human-Led Architecture**: All architectural decisions documented in ADRs were made through independent analysis and research.
- **Code Understanding**: Every line of code in this repository is fully understood and can be explained in technical interviews.
- **AI as Accelerator**: AI was used to speed up repetitive tasks (boilerplate, documentation formatting) and as a sounding board for technical validation, not as a substitute for software engineering expertise.
