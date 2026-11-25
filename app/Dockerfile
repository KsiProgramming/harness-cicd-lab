FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/HarnessLab.Api/HarnessLab.Api.csproj src/HarnessLab.Api/
RUN dotnet restore src/HarnessLab.Api/HarnessLab.Api.csproj

COPY src/ src/
RUN dotnet publish src/HarnessLab.Api/HarnessLab.Api.csproj \
      -c Release -o /out --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ARG APP_VERSION=dev
ENV APP_VERSION=${APP_VERSION} \
    ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /out .

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "HarnessLab.Api.dll"]
