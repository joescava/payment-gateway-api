Payment Gateway API

![.NET CI](https://github.com/joescava/payment-gateway-api/actions/workflows/dotnet.yml/badge.svg)

A .NET 8.0 Web API built using Clean Architecture principles. This project serves as a simplified payment gateway, the best practices for backend structure, API key authentication, middleware usage, and modularity by layers.

# Project Structure

PaymentGateway/
├── Dockerfile
├── docker-compose.yml
├── PaymentGateway.sln
├── src/
│   ├── PaymentGateway.API/
│   ├── PaymentGateway.Application/
│   ├── PaymentGateway.Domain/
│   ├── PaymentGateway.Infrastructure/
│   └── PaymentGateway.Persistence/
└── tests/
    └── PaymentGateway.Tests/

# Getting Started

Prerequisites

- .NET 8 SDK
- Docker (optional but recommended)

# Run with Docker

docker build -t payment-gateway-api .
docker run -d -p 5010:80 --name payment-gateway-api payment-gateway-api

- The API will be available at: http://localhost:5010/swagger
- Default API Key: SuperSecretKey123

# View Docker Logs

- docker logs payment-gateway-api

# Run Locally (Without Docker)

cd src/PaymentGateway.API
dotnet run

- API will run at: http://localhost:5020/swagger/index.html

# API Key Authentication

To access secured endpoints:

- Use header: X-API-KEY: SuperSecretKey123
- In Swagger, click Authorize and enter the same key.

# Unit Testing

cd tests/PaymentGateway.Tests
dotnet test

# Explanation of Core Components

src/PaymentGateway.API

- Program.cs: API startup and middleware configuration.
- Controllers/PaymentController.cs: Handles payment requests (POST and GET).
- Auth/ApiKeyAuthenticationHandler.cs: Custom authentication handler for API key
- Middleware/ErrorHandlingMiddleware.cs: Global exception handling.

src/PaymentGateway.Application

- DTOs: Data transfer objects for requests and responses
- Interfaces: Defines contracts.
- Services/PaymentService.cs: Core business logic: authorization, persistence, response

src/PaymentGateway.Domain

- Entities/Payment.cs: Domain model. Handles card masking internally.

src/PaymentGateway.Infrastructure

- MockAcquirer/IAcquirer.cs: Interface for external acquirer systems.
- MockAcquirer/FakeAcquirer.cs: Simulated third-party payment processor.

src/PaymentGateway.Persistence

- Repositories/InMemoryPaymentRepository.cs: Temporary storage for payments.


# Docker Explained

- Dockerfile

Defines the containerized build and runtime environment. Publishes the app using dotnet publish and exposes port 80.

- docker-compose.yml

(Optional) Can be extended to define multiple services (e.g., database, Redis). Currently not configured.

# Sample Swagger Request

Use Swagger UI at /swagger:

- Click Authorize and enter API Key: SuperSecretKey123
- Test /api/payments endpoint with example payload.

## Sample Request and Response

POST /api/payment
- Request
{
  "cardNumber": "4111111111111111",
  "amount": 100.00,
  "currency": "USD",
  "expiryMonth": "12",
  "expiryYear": "2026",
  "cvv": "123"
}
- Response 
{
  "paymentId": "b5e7f1f0-9a72-4e9f-802c-4f0a88b57b6f",
  "status": "Approved",
  "maskedCardNumber": "************1111"
}

Notes

- Project follows Clean Architecture principles.
- Masking logic ensures card number is never fully stored or returned.
- API Key is stored in plain text in appsettings.json for demo purposes only.
- No real DB is used (in-memory only).
- No expiration or CVV validation – simulated only.
- Ready for future Docker Compose services (DB, Redis).

# Author

 Johan Cante

