FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /code/

COPY src/BuildingBlocks/Results/Vendora.BuildingBlocks.Results.csproj src/BuildingBlocks/Results/
COPY src/BuildingBlocks/Cqrs/Vendora.BuildingBlocks.Cqrs.csproj src/BuildingBlocks/Cqrs/
COPY src/BuildingBlocks/Messaging/Vendora.BuildingBlocks.Messaging.csproj src/BuildingBlocks/Messaging/
COPY src/Services/Ordering/Domain/Vendora.Services.Ordering.Domain.csproj /code/src/Services/Ordering/Domain/
COPY src/Services/Ordering/Application/Vendora.Services.Ordering.Application.csproj /code/src/Services/Ordering/Application/
COPY src/Services/Ordering/Infrastructure/Vendora.Services.Ordering.Infrastructure.csproj /code/src/Services/Ordering/Infrastructure/
COPY src/Services/Ordering/Api/Vendora.Services.Ordering.Api.csproj /code/src/Services/Ordering/Api/

RUN dotnet restore src/Services/Ordering/Api/

COPY . .

RUN dotnet publish src/Services/Ordering/Api/ \
    -c Release \
    --no-restore \
    -o /app/publish/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app/

COPY --from=build /app/publish/ .

RUN apt update 

RUN apt install curl -y 

ENTRYPOINT [ "./Vendora.Services.Ordering.Api" ]