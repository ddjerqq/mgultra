FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY ["MgUltra.csproj", "/src/MgUltra/"]
RUN dotnet restore "/src/MgUltra/MgUltra.csproj"

COPY . /src/MgUltra
RUN dotnet build "/src/MgUltra/MgUltra.csproj" -c Release

FROM build AS publish
RUN ls -alh /src
RUN ls -alh /src/MgUltra
RUN dotnet publish "/src/MgUltra/MgUltra.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MgUltra.dll"]
