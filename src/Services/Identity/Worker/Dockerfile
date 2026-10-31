# Prepare stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS prepare

RUN apt update
RUN apt install curl -y

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /project/

COPY --parents src/BuildingBlocks/**/*.csproj .
COPY --parents src/Services/Identity/**/*.csproj .

RUN dotnet restore src/Services/Identity/Worker/

COPY --parents src/BuildingBlocks/ .
COPY --parents src/Services/Identity/ .

RUN dotnet publish src/Services/Identity/Worker \
    -c Release \
    --no-restore \
    -o /project/publish/

# Runtime stage
FROM prepare AS runtime

WORKDIR /app/

COPY --from=build /project/publish .

ENTRYPOINT [ "./Vendora.Services.Identity.Worker" ]