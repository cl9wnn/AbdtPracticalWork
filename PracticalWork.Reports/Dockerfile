FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY src/PracticalWork.Reports/PracticalWork.Reports.csproj src/PracticalWork.Reports/
COPY src/PracticalWork.Reports.Web/PracticalWork.Reports.Web.csproj src/PracticalWork.Reports.Web/
COPY src/PracticalWork.Reports.Controllers/PracticalWork.Reports.Controllers.csproj src/PracticalWork.Reports.Controllers/
COPY src/PracticalWork.Reports.Contracts/PracticalWork.Reports.Contracts.csproj src/PracticalWork.Reports.Contracts/
COPY src/PracticalWork.Reports.SharedKernel/PracticalWork.Reports.SharedKernel.csproj src/PracticalWork.Reports.SharedKernel/
COPY src/PracticalWork.Reports.Cache.Redis/PracticalWork.Reports.Cache.Redis.csproj src/PracticalWork.Reports.Cache.Redis/
COPY src/PracticalWork.Reports.Data.PostgreSql/PracticalWork.Reports.Data.PostgreSql.csproj src/PracticalWork.Reports.Data.PostgreSql/
COPY src/PracticalWork.Reports.Data.Minio/PracticalWork.Reports.Data.Minio.csproj src/PracticalWork.Reports.Data.Minio/
COPY utils/PracticalWork.Reports.Data.PostgreSql.Migrator/PracticalWork.Reports.Data.PostgreSql.Migrator.csproj utils/PracticalWork.Reports.Data.PostgreSql.Migrator/

COPY PracticalWork.Reports.sln .

RUN dotnet restore PracticalWork.Reports.sln

COPY . .

WORKDIR /app/src/PracticalWork.Reports.Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PracticalWork.Reports.Web.dll"]
