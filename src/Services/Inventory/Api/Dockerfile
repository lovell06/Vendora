FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code

COPY src/BuildingBlocks/Vendora.BuildingBlocks.csproj src/BuildingBlocks/
COPY src/Services/Inventory/Domain/Vendora.Services.Inventory.Domain.csproj /code/src/Services/Inventory/Domain/
COPY src/Services/Inventory/Application/Vendora.Services.Inventory.Application.csproj /code/src/Services/Inventory/Application/
COPY src/Services/Inventory/Infrastructure/Vendora.Services.Inventory.Infrastructure.csproj /code/src/Services/Inventory/Infrastructure/
COPY src/Services/Inventory/Api/Vendora.Services.Inventory.Api.csproj /code/src/Services/Inventory/Api/

RUN dotnet restore src/Services/Inventory/Api

COPY . .

RUN dotnet publish \
    --no-restore \
    -c Release \
    -o /app/publish/ \
    src/Services/Inventory/Api/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "./Vendora.Services.Inventory.Api" ] 