# Assessment: Event-Driven Microservices Platform

This project features a resilient, event-driven microservices architecture built with .NET 9, Kafka, and Docker. It showcases decoupled DDD patterns for cross-service communication, distributed resilience, and automated CI/CD coupled with TDD and containerization.

## Architecture Overview

The system follows a **Modular Monolith/Microservices** hybrid approach within a monorepo structure to maximize developer agility while maintaining clear bounded contexts.

- **Event-Driven**: Services communicate asynchronously via Apache Kafka.
- **Shared Foundation**: A `Shared.Messaging` library centralizes resilient Kafka logic (using Polly).
- **Resilience**: Implements exponential backoff, circuit-breaker patterns (logic ready), and idempotent processing.
- **Decision Tracking**: Technical rationale is captured in [Architecture Decision Records (ADRs)](docs/adr/README.md).

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

### 4. Containerization & Orchestration

The development workflow is entirely **Docker-based**. We use **Docker Compose** to orchestrate the full stack, including Kafka and Zookeeper. This ensures high local fidelity and rapid onboarding, as the entire environment can be spun up with a single command.

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

## AI-Assisted Development

This project was developed utilizing AI as an advanced documentation and research tool. Full architectural control and logical decision-making remained under human oversight, with AI serving as a catalyst for infrastructure scaffolding .

### 1. Boilerplate & Infrastructure

- **Template Generation**: GenAI was strategically utilized as a high-efficiency alternative to traditional project templates, generating service skeletons and initial Docker configurations.
- **TDD Verification**: Once the core business logic was architected, AI was used to assist in the generation of unit and integration test cases (like researching boundary conditions), which were subsequently reviewed and refined to ensure full coverage of boundary conditions.

### 2. Technical Validation & Research

- **Decision Support**: The AI agent (**Gemini**) functioned as an enhanced "StackOverflow + Professor" to contest architectural decisions, validate compliance with DDD principles, and research alternative messaging strategies (e.g., Kafka vs. RabbitMQ).
- **Architecture as Code (PlantUML)**: Diagrams were generated to visualize the system design and are version-controlled alongside the code to maintain a single source of truth and prevent "documentation drift." All generated diagrams were manually verified for architectural accuracy.
