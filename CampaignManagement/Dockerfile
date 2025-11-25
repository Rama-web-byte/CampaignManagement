# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#ENV DOTNET_RUNNING_IN_CONTAINER=true
#ENV DOTNET_USE_POLLING_FILE_WATCHER=1
#ENV DOTNET_NOLOGO=true
#ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
#ENV ASPNETCORE_ENVIRONMENT=Production

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies first (caching layer)
COPY ["CampaignManagement.csproj", "./"]
RUN dotnet restore "./CampaignManagement.csproj"

# Copy remaining files and build
COPY . .
RUN dotnet build "./CampaignManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CampaignManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampaignManagement.dll"]
