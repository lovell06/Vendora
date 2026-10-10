FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code/

COPY src/BuildingBlocks/Results/Vendora.BuildingBlocks.Results.csproj src/BuildingBlocks/Results/
COPY src/BuildingBlocks/Cqrs/Vendora.BuildingBlocks.Cqrs.csproj src/BuildingBlocks/Cqrs/
COPY src/BuildingBlocks/Messaging/Vendora.BuildingBlocks.Messaging.csproj src/BuildingBlocks/Messaging/
COPY src/Services/Catalog/Domain/Vendora.Services.Catalog.Domain.csproj src/Services/Catalog/Domain/
COPY src/Services/Catalog/Application/Vendora.Services.Catalog.Application.csproj src/Services/Catalog/Application/
COPY src/Services/Catalog/Infrastructure/Vendora.Services.Catalog.Infrastructure.csproj src/Services/Catalog/Infrastructure/
COPY src/Services/Catalog/Worker/Vendora.Services.Catalog.Worker.csproj src/Services/Catalog/Worker/

RUN dotnet restore src/Services/Catalog/Worker/

COPY . .

RUN dotnet publish src/Services/Catalog/Worker/ \
    -c Release \
    --no-restore \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app/

COPY --from=build /app/publish/ .

ENTRYPOINT [ "./Vendora.Services.Catalog.Worker" ]