FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY ["IpScanner.csproj", "/src/IpScanner/"]
RUN dotnet restore "/src/IpScanner/IpScanner.csproj"

COPY . /src/IpScanner
RUN dotnet build "/src/IpScanner/IpScanner.csproj" -c Release

FROM build AS publish
RUN ls -alh /src
RUN ls -alh /src/IpScanner
RUN dotnet publish "/src/IpScanner/IpScanner.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IpScanner.dll"]
