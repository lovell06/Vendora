FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code

COPY src/BuildingBlocks/Vendora.BuildingBlocks.csproj \
    src/BuildingBlocks/

COPY src/Services/Identity/Api/Vendora.Services.Identity.Api.csproj \
    src/Services/Identity/Api/

COPY src/Services/Identity/Infrastructure/Vendora.Services.Identity.Infrastructure.csproj \
    src/Services/Identity/Infrastructure/

COPY src/Services/Identity/Application/Vendora.Services.Identity.Application.csproj \
    src/Services/Identity/Application/

COPY src/Services/Identity/Domain/Vendora.Services.Identity.Domain.csproj \
    src/Services/Identity/Domain/

RUN dotnet restore src/Services/Identity/Api/Vendora.Services.Identity.Api.csproj

COPY . .

RUN dotnet publish \
    src/Services/Identity/Api/Vendora.Services.Identity.Api.csproj \
    --no-restore \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "./Vendora.Services.Identity.Api" ]