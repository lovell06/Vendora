# Backend and Docker Compose

A guide for developers running Vendora's backend locally. The stack includes PostgreSQL, Redis, RabbitMQ, five APIs, five workers, and the API Gateway.

[Back to the project overview](../README.md). Commands below use a POSIX shell such as Bash/Zsh and run from the **repository root**, not from `composes/`.

## Prerequisites

- A running Docker Engine or Docker Desktop.
- The `docker compose` plugin with `include` support and Docker BuildKit with support for the `COPY --parents` instructions used by the Dockerfiles.
- Available ports `8080`, `5432`, `6379`, `5672`, and `15672`.
- .NET SDK 10 if you build or run source code outside containers; Dockerfiles supply the SDK for container builds.

## 1. Prepare local configuration

The `environments/` and `secrets/` directories are ignored by Git. A fresh clone needs the files referenced by [compose.yaml](../compose.yaml) and its included files. The repository does not yet provide `.env.example` files; the table below lists required files and configuration keys without real credentials.

For .NET configuration, `__` in an environment variable corresponds to `:` in a configuration key.

Choose PostgreSQL and RabbitMQ credentials, then use matching values in the corresponding application settings. Obtain SMTP credentials from your mail server/provider; the SMTP client currently enables SSL. Replace placeholders with real connection settings before use.

| File under `composes/` | Required contents |
| --- | --- |
| `infra/environments/postgres.env` | `POSTGRES_USER`, `POSTGRES_DB`, `POSTGRES_PASSWORD_FILE=/run/secrets/postgres_password` |
| `infra/secrets/postgres-password.txt` | PostgreSQL password matching the connection strings |
| `infra/environments/rabbitmq.env` | `RABBITMQ_DEFAULT_USER`, `RABBITMQ_DEFAULT_PASS`; use a dedicated application account |
| `shared/environments/aspnetcore.env` | `ASPNETCORE_ENVIRONMENT=Development` for the existing local Gateway routes, OpenAPI, and migrations |
| `shared/environments/netcore.env` | `DOTNET_ENVIRONMENT=Development` for workers |
| `shared/environments/rabbitmq.env` | `RabbitMq__Host=rabbitmq`, `RabbitMq__Port=5672`, `RabbitMq__VirtualHost=/`, `RabbitMq__UserName`, `RabbitMq__Password`, `RabbitMq__Exchange` |
| `shared/secrets/jwt-private-key.pem` | RSA private key for Identity |
| `shared/secrets/jwt-public-key.pem` | Matching RSA public key for the other APIs |
| `<service>/secrets/connection-string.txt` | PostgreSQL connection string for each service: `identity`, `catalog`, `inventory`, `cart`, `ordering` |
| `identity/environments/jwt.env` | `Jwt__Issuer`, `Jwt__Audience`, `Jwt__ExpireMinutes`, `Jwt__PrivateKeyPath=/run/secrets/jwt_private_key` |
| `<service>/environments/jwt.env` for the other four services | `Jwt__Issuer`, `Jwt__Audience`, `Jwt__PublicKeyPath=/run/secrets/jwt_public_key`; issuer/audience must match Identity |
| `identity/environments/caching.env` | `ConnectionStrings__Redis=redis:6379` |
| `identity/environments/admin.account.env` | `AdminSeedingEnable`; when enabled, supply `AdminAccount__Email`, `AdminAccount__Password`, `AdminAccount__FullName`, `AdminAccount__PhoneNumber` |
| `identity/environments/smtp.env` | `Smtp__Host`, `Smtp__Port`, `Smtp__UserName`, `Smtp__Password`, `Smtp__DisplayName` for verification emails |
| `identity/environments/rabbitmq.consumer.env` | `RabbitMqConsumer__QueueName`, `RabbitMqConsumer__PrefetchCount` |
| `identity/environments/rabbitmq.consumer.keys.env` | `RabbitMqConsumer__BindingKeys__0` |
| `cart/environments/rabbitmq.consumer.env`, `inventory/environments/rabbitmq.consumer.env` | A separate queue name for each worker and `RabbitMqConsumer__PrefetchCount` |
| `cart/environments/rabbitmq.consumer.keys.env`, `inventory/environments/rabbitmq.consumer.keys.env` | `RabbitMqConsumer__BindingKeys__0`, `RabbitMqConsumer__BindingKeys__1` |
| `catalog/environments/services.env` | `Services__Inventory__BaseAddress=http://inventory-api:8080/` |
| `cart/environments/services.env` | `Services__Catalog__BaseAddress=http://catalog-api:8080/` |
| `ordering/environments/services.env` | `Services__Cart__BaseAddress=http://cart-api:8080/`, `Services__Catalog__BaseAddress=http://catalog-api:8080/`, `Services__Inventory__BaseAddress=http://inventory-api:8080/` |

Use the same exchange for event publishers and consumers. Publishing workers use `message.Type` as the routing key. Set `RabbitMqConsumer__BindingKeys__<index>` according to the current registries:

| Worker | Binding keys in index order, starting at 0 |
| --- | --- |
| Identity | `identity.user.registered` |
| Cart | `identity.user.registered`, `ordering.order.placed` |
| Inventory | `catalog.product-variant.added`, `ordering.order.cancelled` |

**Worker configuration gap:** two workers register HTTP clients, but Compose does not load their corresponding service-address files:

| Worker | File not loaded | Keys needed when using the clients |
| --- | --- | --- |
| Cart | `cart/environments/services.env` | `Services__Catalog__BaseAddress` |
| Ordering | `ordering/environments/services.env` | `Services__Cart__BaseAddress`, `Services__Catalog__BaseAddress`, `Services__Inventory__BaseAddress` |

For workflows using these clients, provide the keys through an environment file already loaded by the worker or adjust its `env_file` configuration. This requires configuration changes beyond starting the existing files; running containers alone does not establish that all business workflows work.

Example container connection string; replace values in `<...>` before use:

```text
Host=postgres;Port=5432;Database=<service_database>;Username=<POSTGRES_USER>;Password=<password>
```

Compose mounts this file as `/run/secrets/ConnectionStrings__Postgres`; applications read it through `AddKeyPerFile`. The hosts `postgres`, `redis`, `rabbitmq`, and `*-api` are Docker network service names. When running applications directly on the host, use host-accessible addresses/ports and local RSA key paths.

PostgreSQL initializes the database named by `POSTGRES_DB`. If you use a separate database per service, ensure those databases exist or that the connection account can create them. APIs **automatically run EF Core migrations at startup only in Development**. In other environments, apply migrations separately before using the APIs.

If you do not have an RSA key pair, install OpenSSL to run the following commands. Generate keys only when none exist; these commands write to the destination paths:

```sh
mkdir -p composes/shared/secrets
openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:2048 -out composes/shared/secrets/jwt-private-key.pem
openssl pkey -in composes/shared/secrets/jwt-private-key.pem -pubout -out composes/shared/secrets/jwt-public-key.pem
```

Keep private keys, passwords, and connection strings local; do not put real values in documentation or Git.

## 2. Validate and start

After preparing all required files, run from the repository root:

```sh
docker compose config --quiet
docker compose up --build -d
docker compose ps
```

Compose starts PostgreSQL, Redis, RabbitMQ, five APIs, five workers, and the API Gateway. **Web is not included in Compose**; run it separately using the [Web guide](../src/Web/README.md).

## 3. Access and inspect

| Service | Address from the development host |
| --- | --- |
| API Gateway | http://localhost:8080 |
| Gateway health | http://localhost:8080/health |
| Scalar API Reference in Development | http://localhost:8080/scalar |
| RabbitMQ Management | http://localhost:15672 |
| PostgreSQL | `localhost:5432` |
| Redis | `localhost:6379` |
| RabbitMQ AMQP | `localhost:5672` |

Gateway route groups are `/api/auth`, `/api/users`, `/api/products`, `/api/categories`, `/api/inventory-items`, `/api/carts`, and `/api/orders`. Service OpenAPI documents are exposed through `/openapi/identity.json`, `/openapi/catalog.json`, `/openapi/inventory.json`, `/openapi/cart.json`, and `/openapi/ordering.json`.

Internal APIs listen on container port `8080`; their individual host port mappings are currently commented out in Compose. Gateway `/health` checks the Gateway itself; inspect `docker compose ps` and individual service logs to check the backend.

```sh
curl -i http://localhost:8080/health
docker compose logs -f api-gateway identity-api identity-worker
```

After startup, the Gateway health request should return HTTP 200. Open Scalar to check that API documents load through the Gateway. This is an additional connectivity check, not verification of data-changing operations or event processing.

Stop the stack with `docker compose down`. PostgreSQL and RabbitMQ data live in named volumes and are retained after this command.

## Troubleshooting

| Symptom | What to check |
| --- | --- |
| Compose reports missing env/secret files | Check paths in `composes/**/*.yaml`; these local files are absent from a fresh clone |
| Docker build fails at `COPY --parents` | Check Docker/BuildKit support for the Dockerfile syntax in use |
| API is unhealthy or reports missing tables | Check connection strings, database permissions, migration logs, and `ASPNETCORE_ENVIRONMENT=Development` |
| Gateway returns 404 or 502 | Routes currently live in `src/ApiGateway/appsettings.Development.json`; check the environment, API status, and internal Docker hostnames |
| Gateway is healthy but business operations fail | Check relevant API/worker health and logs; Gateway health does not check every dependency |
| API rejects JWTs | Check issuer/audience, token expiry, and matching RSA keys |
| Email or event synchronization does not work | Check the Identity worker, SMTP, RabbitMQ credentials, exchange, queues, and binding keys |
| Gateway is configured but Web cannot establish login | Web does not handle sessions yet; see [authentication contract limitations](../src/Web/Features/Authentication/README.md#current-contracts-and-limitations) |

## Verification limits

Compose, Dockerfiles, configuration, and startup code were inspected. `docker compose config --quiet` passed on Docker Compose 5.7.1 with the existing local environment files/secrets; this does not validate image builds, database connectivity, or complete business contracts. Container builds, migrations, and cross-service workflows were not run because the documentation session permits writes only to README files. See also [Web authentication configuration](../src/Web/Features/Authentication/README.md).
