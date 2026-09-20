FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code

COPY src/BuildingBlocks/Vendora.BuildingBlocks.csproj /code/src/BuildingBlocks/
COPY src/Services/Catalog/Api/Vendora.Services.Catalog.Api.csproj /code/src/Services/Catalog/Api/
COPY src/Services/Catalog/Infrastructure/Vendora.Services.Catalog.Infrastructure.csproj /code/src/Services/Catalog/Infrastructure/
COPY src/Services/Catalog/Application/Vendora.Services.Catalog.Application.csproj /code/src/Services/Catalog/Application/
COPY src/Services/Catalog/Domain/Vendora.Services.Catalog.Domain.csproj /code/src/Services/Catalog/Domain/

RUN dotnet restore src/Services/Catalog/Api/

COPY . .

RUN dotnet publish \
    -c Release \
    --no-restore \
    -o /app/publish \
    src/Services/Catalog/Api/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "./Vendora.Services.Catalog.Api" ]