# Vendora

Vendora is an e-commerce project for developers building commerce services and a storefront. It uses **.NET 10**, separates business capabilities into five microservices, exposes APIs through a Gateway, and provides a **Blazor Interactive Server** frontend.

> **Under development:** The Web frontend uses sample data. Authentication, cart, and ordering workflows are not yet fully integrated between Web and the backend.

## Start with the demo storefront

Install the **.NET SDK 10**, then run these commands from the repository root:

```sh
dotnet restore src/Web/Vendora.Web.csproj
dotnet run --project src/Web/Vendora.Web.csproj --launch-profile http
```

Open [http://localhost:5111](http://localhost:5111). The homepage displays sample products labeled “Demo catalog.” Search for `headphones`, then open `/products/headphones` to explore variants and the image gallery. This demo does not require the backend. Adding an item to the cart only displays demo feedback.

To run the backend, follow the [Docker Compose guide](composes/README.md): prepare environment files, secrets, and RSA keys, validate the configuration, start the stack, and open Scalar. Web runs separately from Compose.

## Components and current capabilities

| Component | Scope in the codebase |
| --- | --- |
| Identity | Registration, login, logout, email verification, and employee account creation |
| Catalog | Products, variants, categories, and product lifecycle states |
| Inventory | Stock queries, adjustments, availability checks, and reservations |
| Cart | Adding, removing, updating quantities, and querying cart items |
| Ordering | Placing, canceling, preparing, delivering, and completing orders |
| API Gateway | HTTP routing through YARP and aggregated API documentation in Development |
| Web | Searching/filtering sample data, product details, variant selection, image galleries, and authentication forms |

The backend uses ASP.NET Core Minimal APIs, Entity Framework Core 10, and PostgreSQL 18. Redis supports Identity; RabbitMQ carries events between services. API authentication uses JSON Web Tokens (JWTs) signed with RSA keys. Web uses MudBlazor and locally hosted Inter fonts, supports light/dark/system themes, and displays its interface in English.

## Architecture

Each service contains `Api`, `Application`, `Domain`, `Infrastructure`, and `Worker` projects. Api handles HTTP, Application coordinates use cases, Domain holds business rules, and Infrastructure handles persistence and external integrations. Workers publish outbox events or process events received from RabbitMQ.

An outbox stores events in the same transaction as business data before a worker publishes them asynchronously. Services also call each other directly over HTTP; for example, Ordering has clients for Cart, Catalog, and Inventory.

```text
HTTP client → API Gateway → Identity / Catalog / Inventory / Cart / Ordering
Blazor Web → API Gateway (authentication forms when configured)
API → PostgreSQL + outbox → publishing worker → RabbitMQ → consuming worker
Identity → Redis
```

This diagram summarizes groups of connections. The Web catalog and cart do not currently call the Gateway.

```text
Vendora.slnx             # .NET solution
compose.yaml            # Combined backend configuration
composes/               # Infrastructure, APIs, workers, local env files and secrets
src/
  ApiGateway/           # YARP and API documentation
  BuildingBlocks/       # Shared CQRS, messaging, and result types
  Services/             # Identity, Catalog, Inventory, Cart, Ordering
  Web/                  # Blazor, features, UI components, and static assets
```

`BuildingBlocks/Cqrs` provides Command Query Responsibility Segregation (CQRS) infrastructure, separating requests that change data from queries. The `tests/` and `docs/` directories are currently empty.

## Development and verification

Building on the host requires the .NET SDK 10. Run from the repository root:

```sh
dotnet restore Vendora.slnx
dotnet build Vendora.slnx --no-restore
```

The repository has no automated test projects yet. A build verifies compilation; API and cross-service workflows still need testing with workers running. Component READMEs describe suggested manual checks.

Documented commands have been checked against source code and configuration. `docker compose config --quiet` passed with the existing local configuration files. Restore, builds, applications, containers, and browser checks were not run during this documentation update because the session permits writes only to README files.

## Component documentation

| Guide | Contents |
| --- | --- |
| [Backend and Docker Compose](composes/README.md) | Prerequisites, environment files/secrets, migrations, API addresses, and troubleshooting |
| [Web](src/Web/README.md) | Running Blazor, routes, frontend structure, and integration limits |
| [Home](src/Web/Features/Home/README.md) | Sample catalog, search, categories, and navigation |
| [Products](src/Web/Features/Products/README.md) | Details, variants, images, and quantity selection |
| [Authentication](src/Web/Features/Authentication/README.md) | Forms, Gateway configuration, and API contracts |
| [Shared UI](src/Web/Components/UI/README.md) | Components, usage examples, themes, and integration conventions |

## License

The repository does not contain a project-wide license file. The Inter font license is provided separately in [LICENSE.txt](src/Web/wwwroot/fonts/inter/LICENSE.txt) and applies to the font.
