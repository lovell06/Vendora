FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code/

COPY src/BuildingBlocks/Results/Vendora.BuildingBlocks.Results.csproj src/BuildingBlocks/Results/
COPY src/BuildingBlocks/Cqrs/Vendora.BuildingBlocks.Cqrs.csproj src/BuildingBlocks/Cqrs/
COPY src/BuildingBlocks/Messaging/Vendora.BuildingBlocks.Messaging.csproj src/BuildingBlocks/Messaging/
COPY src/Services/Identity/Domain/Vendora.Services.Identity.Domain.csproj src/Services/Identity/Domain/
COPY src/Services/Identity/Application/Vendora.Services.Identity.Application.csproj src/Services/Identity/Application/
COPY src/Services/Identity/Infrastructure/Vendora.Services.Identity.Infrastructure.csproj src/Services/Identity/Infrastructure/
COPY src/Services/Identity/Worker/Vendora.Services.Identity.Worker.csproj src/Services/Identity/Worker/

RUN dotnet restore src/Services/Identity/Worker/

COPY . .

RUN dotnet publish src/Services/Identity/Worker \
    -c Release \
    --no-restore \
    -o /app/publish/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app/

COPY --from=build /app/publish .

ENTRYPOINT [ "./Vendora.Services.Identity.Worker" ]