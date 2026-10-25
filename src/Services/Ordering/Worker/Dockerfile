FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /project/

COPY --parents src/BuildingBlocks/**/*.csproj ./
COPY --parents src/Services/Ordering/**/*.csproj ./

RUN dotnet restore src/Services/Ordering/Worker/

COPY . .

RUN dotnet publish src/Services/Ordering/Worker/ \
    -c Release \
    --no-restore \
    -o /project/publish/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app/

COPY --from=build /project/publish/ /app/

ENTRYPOINT [ "./Vendora.Services.Ordering.Worker" ]