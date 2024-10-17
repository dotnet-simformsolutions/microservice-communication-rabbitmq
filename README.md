# Interservice Communication: Sync and Async Microservices

This repository demonstrates synchronous and asynchronous interservice communication patterns in .NET microservices, designed for reliable, scalable, and decoupled service-to-service communication.

## Features

### Synchronous Communication

- Utilizes HTTP requests for real-time communication between microservices.

### Asynchronous Communication

- Leverages message brokers like RabbitMQ for decoupled, event-driven communication.

## Getting Started

### Prerequisites

Ensure the following are installed:

- .NET SDK
- Docker
- RabbitMQ
- SQL Server

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/poojanpk/microservices-intra-communication-dotnet.git
    cd microservices-intra-communication-dotnet
    ```

2. Set up environment variables:
   - Configure environment variables in `appsettings.json` for database connection strings, message broker settings, and other required configurations.

3. Set up Docker for RabbitMQ:
    ```bash
    docker pull rabbitmq:3-management
    docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management
    ```

## Project Structure

- **Order Service**: Handles order creation and processing.
- **Product Service**: Manages product inventory and stock levels.
- **Authentication API**: Authenticates users for secure access to services.
- **API Gateway**: Provides access to the Order and Product microservices, acting as a single entry point.
