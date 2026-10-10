FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code/

COPY src/BuildingBlocks/Results/Vendora.BuildingBlocks.Results.csproj src/BuildingBlocks/Results/
COPY src/BuildingBlocks/Cqrs/Vendora.BuildingBlocks.Cqrs.csproj src/BuildingBlocks/Cqrs/
COPY src/BuildingBlocks/Messaging/Vendora.BuildingBlocks.Messaging.csproj src/BuildingBlocks/Messaging/
COPY src/Services/Cart/Domain/Vendora.Services.Cart.Domain.csproj src/Services/Cart/Domain/
COPY src/Services/Cart/Application/Vendora.Services.Cart.Application.csproj src/Services/Cart/Application/
COPY src/Services/Cart/Infrastructure/Vendora.Services.Cart.Infrastructure.csproj src/Services/Cart/Infrastructure/
COPY src/Services/Cart/Worker/Vendora.Services.Cart.Worker.csproj src/Services/Cart/Worker/

RUN dotnet restore src/Services/Cart/Worker/

COPY . .

RUN dotnet publish src/Services/Cart/Worker/ \
    -c Release \
    --no-restore \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app/

COPY --from=build /app/publish .

ENTRYPOINT [ "./Vendora.Services.Cart.Worker" ]