# syntax=docker/dockerfile:1
# Multi-stage build: the SDK image compiles, the slim ASP.NET runtime image runs.
# Build context = app/   (docker build -t harness-lab-api app/)
# Tests are NOT run here. The CI pipeline runs them in a dedicated step so results show
# in Harness' Tests tab and a failing test stops the pipeline before any image is pushed.

# ---- build stage ------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first, using only the project files, so this layer stays cached until dependencies change.
COPY src/HarnessLab.Api/HarnessLab.Api.csproj src/HarnessLab.Api/
RUN dotnet restore src/HarnessLab.Api/HarnessLab.Api.csproj

COPY src/ src/
RUN dotnet publish src/HarnessLab.Api/HarnessLab.Api.csproj \
      -c Release -o /out --no-restore /p:UseAppHost=false

# ---- runtime stage ----------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Injected by CI (build-arg) so the running container reports which build it is.
ARG APP_VERSION=dev
ENV APP_VERSION=${APP_VERSION} \
    ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /out .

# Built-in non-root user of the official .NET images (UID 1654).
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "HarnessLab.Api.dll"]
