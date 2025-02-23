FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /
COPY ["src/MgUltra/MgUltra.csproj", "/src/MgUltra/"]
RUN dotnet restore "src/MgUltra/MgUltra.csproj"

COPY . .
WORKDIR /
RUN dotnet build -c Release --no-restore "/src/MgUltra/MgUltra.csproj"

FROM build AS publish
RUN dotnet publish -c Release --no-restore --no-build -o /app/publish "/src/MgUltra/MgUltra.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MgUltra.dll"]
