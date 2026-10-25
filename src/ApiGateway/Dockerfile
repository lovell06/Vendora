# Prepare stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS prepare

RUN apt update
RUN apt install curl -y

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /project/

COPY src/ApiGateway/*.csproj src/ApiGateway/

RUN dotnet restore src/ApiGateway/

COPY --parents src/ApiGateway/* .

RUN dotnet publish src/ApiGateway/ \
    -c Release \
    --no-restore \
    -o ./publish/ 

# Run stage
FROM prepare AS runtime

WORKDIR /app/

COPY --from=build project/publish/ .

ENTRYPOINT [ "./Vendora.ApiGateway" ]