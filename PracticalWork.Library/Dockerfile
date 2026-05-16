FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY nuget.config .
COPY PracticalWork.Shared/nugetStorage /app/PracticalWork.Shared/nugetStorage

COPY PracticalWork.Library/src/PracticalWork.Library/PracticalWork.Library.csproj src/PracticalWork.Library/
COPY PracticalWork.Library/src/PracticalWork.Library.Web/PracticalWork.Library.Web.csproj src/PracticalWork.Library.Web/
COPY PracticalWork.Library/src/PracticalWork.Library.Controllers/PracticalWork.Library.Controllers.csproj src/PracticalWork.Library.Controllers/
COPY PracticalWork.Library/src/PracticalWork.Library.Contracts/PracticalWork.Library.Contracts.csproj src/PracticalWork.Library.Contracts/
COPY PracticalWork.Library/src/PracticalWork.Library.Cache.Redis/PracticalWork.Library.Cache.Redis.csproj src/PracticalWork.Library.Cache.Redis/
COPY PracticalWork.Library/src/PracticalWork.Library.Data.PostgreSql/PracticalWork.Library.Data.PostgreSql.csproj src/PracticalWork.Library.Data.PostgreSql/
COPY PracticalWork.Library/src/PracticalWork.Library.MessageBroker.Kafka/PracticalWork.Library.MessageBroker.Kafka.csproj src/PracticalWork.Library.MessageBroker.Kafka/
COPY PracticalWork.Library/src/PracticalWork.Library.Data.Minio/PracticalWork.Library.Data.Minio.csproj src/PracticalWork.Library.Data.Minio/
COPY PracticalWork.Library/src/PracticalWork.Library.BackgroundJobs.Quartz/PracticalWork.Library.BackgroundJobs.Quartz.csproj src/PracticalWork.Library.BackgroundJobs.Quartz/
COPY PracticalWork.Library/src/PracticalWork.Library.Notification.Email/PracticalWork.Library.Notification.Email.csproj src/PracticalWork.Library.Notification.Email/
COPY PracticalWork.Library/utils/PracticalWork.Library.Data.PostgreSql.Migrator/PracticalWork.Library.Data.PostgreSql.Migrator.csproj utils/PracticalWork.Library.Data.PostgreSql.Migrator/
COPY PracticalWork.Library/src/PracticalWork.Library.Clients.Http/PracticalWork.Library.Clients.Http.csproj src/PracticalWork.Library.Clients.Http/

COPY PracticalWork.Library/PracticalWork.Library.slnx .
COPY PracticalWork.Library/Directory.Packages.props .
COPY PracticalWork.Library/Directory.Build.props .

RUN dotnet restore PracticalWork.Library.slnx

COPY . .

WORKDIR /app/PracticalWork.Library/src/PracticalWork.Library.Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PracticalWork.Library.Web.dll"]