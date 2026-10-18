# Vendora

A commerce backend built with .NET, organized into independent services with clear domain boundaries.

## Services

- **Identity** — Accounts, authentication, and email verification.
- **Catalog** — Products, categories, and pricing.
- **Cart** — Shopping carts and item selection.
- **Inventory** — Stock management and reservations.
- **Ordering** — Order placement and lifecycle.

## Architecture

Each service separates API, application logic, domain models, and infrastructure.

Services communicate through HTTP and RabbitMQ. Transactional outboxes persist events alongside business data for asynchronous delivery.

## Stack

.NET 10 · ASP.NET Core · Entity Framework Core · PostgreSQL · Redis · RabbitMQ · Docker Compose

## Development

The project is under active development and is not yet complete. Features and service integrations are still being implemented and refined.

Docker Compose configuration is organized under `composes/`, with separate API and worker definitions where needed. Environment files and secrets must be provided locally before starting the stack.

```sh
docker compose up --build
```
