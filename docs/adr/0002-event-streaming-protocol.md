# ADR 0002: Event Streaming with Kafka

## Status

Accepted

## Date

2026-01-11

## Context

The system requires an asynchronous communication mechanism to decouple services (e.g., UserService and OrderService). We specifically need a system that supports high throughput for future telematics/telemetry data.

## Decision

We have chosen **Apache Kafka** as the primary message broker.

## Consequences

- **Pros**:
  - **Durability & Replayability**: Events are persisted to disk, allowing "Event Sourcing" patterns and re-calculating state by replaying logs.
  - **Scalability**: Designed for high-throughput streaming (millions of events/sec), which is critical for the vehicle telemetry module.
  - **Order Guarantees**: Kafka provides strong ordering within a partition.
- **Cons**:
  - **Operational Overhead**: Kafka is more complex to manage than RabbitMQ (requires Zookeeper/KRaft).
  - **Learning Curve**: The consumer group and offset management concepts are more complex than simple amqp queues.

## Alternatives Considered

- **RabbitMQ**: Highly considered for its simplicity and excellent routing capabilities (Exchange headers). However, it lacks the native log-based storage and high-throughput streaming capabilities required for the telemetry-heavy future of the project.
