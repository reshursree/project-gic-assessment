# Assessment

## Running the App

To run the app, you can use the following command:

```bash
docker compose up --build
```

## Features

### 1. Structured Logging

- We can use structured logging to make it easier to search and analyze logs.

### 2. Health Checks

- We can use health checks to make it easier to monitor the health of the container.

### 3. Metrics

- We can use metrics to make it easier to monitor the performance of the container.

### 4. Configuration

- We can use environment variables to make it easier to configure the container.

## Recommendations

### 1. Docker Image Security

- We can use distroless images to reduce the attack surface of the container.
- We can use multi-stage builds to reduce the size of the container.
- We can use a non-root user to run the application.

### 2. Deployment to Container Registry and CD

- We can use GitHub Actions to build and push the container image to GitHub Container Registry.
- We can use GitHub Actions to deploy the container image to a container registry.

### 3. Monitoring and Logging

- We can use Prometheus and Grafana to monitor the container.
- We can use ELK Stack to log the container.
