FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code

COPY src/BuildingBlocks/Vendora.BuildingBlocks.csproj src/BuildingBlocks/
COPY src/Services/Cart/Domain/Vendora.Services.Cart.Domain.csproj src/Services/Cart/Domain/
COPY src/Services/Cart/Application/Vendora.Services.Cart.Application.csproj src/Services/Cart/Application/
COPY src/Services/Cart/Infrastructure/Vendora.Services.Cart.Infrastructure.csproj src/Services/Cart/Infrastructure/
COPY src/Services/Cart/Api/Vendora.Services.Cart.Api.csproj src/Services/Cart/Api/

RUN dotnet restore src/Services/Cart/Api/

COPY . .

RUN dotnet publish \
    -c Release \
    --no-restore \
    -o /app/publish/ \
    src/Services/Cart/Api/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish/ .

ENTRYPOINT [ "./Vendora.Services.Cart.Api" ]