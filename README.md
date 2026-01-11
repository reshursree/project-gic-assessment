# Assessment: Event-Driven Microservices Platform

This project demonstrates a resilient, event-driven microservices architecture built with .NET 9, Kafka, and Docker. It showcases industry-grade patterns for cross-service communication, distributed resilience, and automated CI/CD.

## 🏗️ Architecture Overview

The system follows a **Modular Monolith/Microservices** hybrid approach within a monorepo structure to maximize developer agility while maintaining clear bounded contexts.

- **Event-Driven**: Services communicate asynchronously via Apache Kafka.
- **Shared Foundation**: A `Shared.Messaging` library centralizes resilient Kafka logic (using Polly).
- **Resilience**: Implements exponential backoff, circuit-breaker patterns (logic ready), and idempotent processing.

### Services

- **User Service (Port 5001)**: Handles user registration and emits `UserCreatedEvent`.
- **Order Service (Port 5002)**: Consumes user events via a Background Service and manages the order lifecycle.
- **Shared.Messaging**: Shared plumbing for resilient Kafka Producers and Consumers.

---

## 🚀 Getting Started

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

---

## 🛠️ Engineering Standards

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

---

## 📈 Future Recommendations

### Security

- **Distroless Images**: Move to bitnami or Google distroless base images to reduce attack surface.
- **Non-Root User**: Configure Dockerfiles to run as non-root for production.

### CI/CD & DevOps

- **GHCR Integration**: Automate image pushing to GitHub Container Registry/Dockerhub.
- **Helm Charts**: Prepare for K8s deployment using Helm for environment modeling.

### Monitoring

- **Prometheus/Grafana**: Export metrics via OpenTelemetry.
- **ELK Stack**: Centralize logs for cross-service tracing (Correlation IDs).
