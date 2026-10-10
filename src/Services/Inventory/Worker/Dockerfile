FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code/

COPY src/BuildingBlocks/Results/Vendora.BuildingBlocks.Results.csproj src/BuildingBlocks/Results/
COPY src/BuildingBlocks/Cqrs/Vendora.BuildingBlocks.Cqrs.csproj src/BuildingBlocks/Cqrs/
COPY src/BuildingBlocks/Messaging/Vendora.BuildingBlocks.Messaging.csproj src/BuildingBlocks/Messaging/
COPY src/Services/Inventory/Domain/Vendora.Services.Inventory.Domain.csproj src/Services/Inventory/Domain/
COPY src/Services/Inventory/Application/Vendora.Services.Inventory.Application.csproj src/Services/Inventory/Application/
COPY src/Services/Inventory/Infrastructure/Vendora.Services.Inventory.Infrastructure.csproj src/Services/Inventory/Infrastructure/
COPY src/Services/Inventory/Worker/Vendora.Services.Inventory.Worker.csproj src/Services/Inventory/Worker/

RUN dotnet restore src/Services/Inventory/Worker/

COPY . .

RUN dotnet publish src/Services/Inventory/Worker \ 
    -c Release \
    --no-restore \
    -o /app/publish/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app/

COPY --from=build /app/publish/ .

ENTRYPOINT [ "./Vendora.Services.Inventory.Worker" ]