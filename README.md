# Event-Driven Microservices Platform

This repository implements a resilient microservices architecture utilizing **.NET 9**, **Apache Kafka**, and **Docker**. The system is designed to demonstrate professional engineering standards in distributed systems, specifically focusing on asynchronous communication, fault tolerance, and domain-driven design principles.

## Architecture & Design Principles

The project utilizes a **Monorepo** strategy to balance service decoupling with operational efficiency.

- **Asynchronous Communication**: Services interact via an event-driven model using Kafka. Resilience is enforced through **Polly** policies to handle transient failures gracefully ([ADR-0002](docs/adr/0002-event-streaming-protocol.md)).
- **Maintainable Scaling**: The monorepo structure facilitates centralized dependency management and atomic updates across service boundaries ([ADR-0001](docs/adr/0001-monorepo-strategy.md)).
- **Technical Documentation**: Architectural rationale is preserved through [Architecture Decision Records (ADR)](docs/adr/README.md) and version-controlled [Visual Architecture](#visual-architecture).

### Visual Architecture

Detailed system design is captured across several architectural perspectives:

- **[System Context](docs/architecture/diagrams/context-diagram-simple.puml)**: High-level system boundaries and external interactions.
- **[Component Architecture](docs/architecture/diagrams/component-diagram.puml)**: Internal service structure and boundary definitions.
- **[Event Flow](docs/architecture/diagrams/event-flow-diagram.puml)**: Message propagation through Kafka topics.
- **[Sequence Interactions](docs/architecture/diagrams/sequence-diagram.puml)**: Detailed request-response and event-driven workflows.
- **[Data Model](docs/architecture/diagrams/erd.puml)**: Entity-relation diagrams for service databases.
- **[Deployment](docs/architecture/diagrams/deployment-diagram.puml)**: Container orchestration and network topology.

### Service Inventory

- **User Service (Port 5001)**: Manages user lifecycle, publishes registration events, and subscribes to downstream order notifications.
- **Order Service (Port 5002)**: Orchestrates order processing, consumes user events, and manages the order state machine.
- **Shared.Messaging**: A central library for resilient producer/consumer logic and standardized event schemas.

## Infrastructure & Setup

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (for local development and testing)

### Deployment

The full stack—including message brokers and databases—is orchestrated via Docker Compose:

```bash
docker compose up --build
```

**Service Endpoints:**

- **User Service OpenAPI**: `http://localhost:5001/swagger`
- **Order Service OpenAPI**: `http://localhost:5002/swagger`
- **Health Monitoring**: `/health` endpoints are available for all runtime services.

## Engineering Standards

### 1. Test-Driven Development (TDD)

Logic implementation follows a **Red-Green-Refactor** methodology. Integration testing is handled via `WebApplicationFactory` to ensure high-fidelity verification of service interactions.

- **Command**: `dotnet test TakeHomeTest.sln`

### 2. Observability & Monitoring

- **Structured Logging**: Implemented via **Serilog** with JSON formatting for log aggregation readiness.
- **Health Verification**: Native ASP.NET Core health checks integrated with Docker orchestration for automated recovery.

### 3. Distributed Resilience

- **Failure Handling**: Kafka producers implement exponential backoff and retry patterns via **Polly**.
- **Data Integrity**: Manual offset management and server-side idempotency ensure at-most-once processing and state consistency.
- **Schema Governance**: Integration events are strictly versioned within the `Shared.Messaging` library to prevent cross-service breaking changes.

### 4. Containerization

Industry-standard `Dockerfiles` ensure environment parity across the development lifecycle, from local builds to CI/CD pipelines.

### 5. Automated Delivery

- **GitHub Actions**: Configured to provide continuous integration feedback and automated build verification.

### 6. Documentation as Code

- **Interactive Specifications**: Swagger/OpenAPI provides real-time contract verification.
- **Rationale Tracking**: [ADRs](docs/adr/README.md) document the "why" behind critical architectural choices.
- **Living Diagrams**: PlantUML source files are version-controlled alongside the code to prevent documentation drift.

## Critical Architectural Decisions

Key decisions that define the system's character:

1. **[Monorepo Strategy](docs/adr/0001-monorepo-strategy.md)**: Selected to enable atomic commits and simplify dependency management during initial scale-up.
2. **[Kafka Implementation](docs/adr/0002-event-streaming-protocol.md)**: Chosen over traditional brokers for its durability, replayability, and high-throughput capabilities.

---

## AI Usage Methodology

This project utilized AI tools (Gemini) as a **technical research and documentation assistant**. Professional oversight was maintained throughout the development process, with all architectural logic and code implementation driven by human decision-making.

### Applied Use Cases:

- **Project Scaffolding**: Leveraged for efficient generation of initial service boilerplate and Docker configurations.
- **Technical Research**: Used as an advanced research tool to validate DDD principles and compare messaging protocols.
- **Test Strategy Validation**: Utilized as a sounding board to identify edge cases for the TDD suites.
- **Documentation Refinement**: Assisted in converting architectural concepts into PlantUML syntax and Markdown formatting.

Note:
All architectural decisions and code implementations reflect the developer's direct knowledge and engineering judgment. All components are fully understood and review-ready for technical discussion.
