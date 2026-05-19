FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RinhaDeBackend2026/RinhaDeBackend2026.csproj", "RinhaDeBackend2026/"]
RUN dotnet restore "RinhaDeBackend2026/RinhaDeBackend2026.csproj"
COPY . .
WORKDIR "/src/RinhaDeBackend2026"
RUN dotnet build "./RinhaDeBackend2026.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RinhaDeBackend2026.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RinhaDeBackend2026.dll"]
