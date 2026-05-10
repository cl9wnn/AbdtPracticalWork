FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY nuget.config .
COPY PracticalWork.Shared/nugetStorage /app/PracticalWork.Shared/nugetStorage

COPY PracticalWork.Reports/src/PracticalWork.Reports/PracticalWork.Reports.csproj src/PracticalWork.Reports/
COPY PracticalWork.Reports/src/PracticalWork.Reports.Web/PracticalWork.Reports.Web.csproj src/PracticalWork.Reports.Web/
COPY PracticalWork.Reports/src/PracticalWork.Reports.Controllers/PracticalWork.Reports.Controllers.csproj src/PracticalWork.Reports.Controllers/
COPY PracticalWork.Reports/src/PracticalWork.Reports.Contracts/PracticalWork.Reports.Contracts.csproj src/PracticalWork.Reports.Contracts/
COPY PracticalWork.Reports/src/PracticalWork.Reports.Cache.Redis/PracticalWork.Reports.Cache.Redis.csproj src/PracticalWork.Reports.Cache.Redis/
COPY PracticalWork.Reports/src/PracticalWork.Reports.Data.PostgreSql/PracticalWork.Reports.Data.PostgreSql.csproj src/PracticalWork.Reports.Data.PostgreSql/
COPY PracticalWork.Reports/src/PracticalWork.Reports.MessageBroker.Kafka/PracticalWork.Reports.MessageBroker.Kafka.csproj src/PracticalWork.Reports.MessageBroker.Kafka/
COPY PracticalWork.Reports/src/PracticalWork.Reports.Data.Minio/PracticalWork.Reports.Data.Minio.csproj src/PracticalWork.Reports.Data.Minio/
COPY PracticalWork.Reports/utils/PracticalWork.Reports.Data.PostgreSql.Migrator/PracticalWork.Reports.Data.PostgreSql.Migrator.csproj utils/PracticalWork.Reports.Data.PostgreSql.Migrator/

COPY PracticalWork.Reports/PracticalWork.Reports.slnx .
COPY PracticalWork.Reports/Directory.Packages.props .
COPY PracticalWork.Reports/Directory.Build.props .

RUN dotnet restore PracticalWork.Reports.slnx

COPY . .

WORKDIR /app/PracticalWork.Reports/src/PracticalWork.Reports.Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PracticalWork.Reports.Web.dll"]
