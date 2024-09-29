# Stage 1: Base image setup
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build and restore dependencies for the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy main API project and restore dependencies
COPY Poseidon/Poseidon.csproj Poseidon/
RUN dotnet restore "Poseidon/Poseidon.csproj"

# Copy the rest of the application code
COPY Poseidon/ Poseidon/

# Build the main API project
WORKDIR "/src/Poseidon"
RUN dotnet build "Poseidon.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Poseidon.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poseidon.dll"]
