# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files
COPY Backend.sln ./
COPY AlphaAPI/AlphaAPI.csproj AlphaAPI/
COPY BusinessObjects/BusinessObjects.csproj BusinessObjects/
COPY DataAccessObjects/DataAccessObjects.csproj DataAccessObjects/
COPY Repositories/Repositories.csproj Repositories/
COPY Services/Services.csproj Services/

# Restore dependencies
RUN dotnet restore Backend.sln

# Copy all source code
COPY . .

# Publish AlphaAPI
RUN dotnet publish AlphaAPI/AlphaAPI.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Copy published output
COPY --from=build /app/publish .

# Expose port (default ASP.NET port)
EXPOSE 8080

# Start AlphaAPI
ENTRYPOINT ["dotnet", "AlphaAPI.dll"]