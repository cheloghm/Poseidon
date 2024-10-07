# Stage 1: Base image setup for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build image with SDK and dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project files for API and Tests
COPY ./Poseidon/Poseidon.csproj ./Poseidon/
COPY ./Poseidon.Tests/Poseidon.Tests.csproj ./Poseidon.Tests/

# Restore dependencies for both projects
RUN dotnet restore "./Poseidon/Poseidon.csproj"
RUN dotnet restore "./Poseidon.Tests/Poseidon.Tests.csproj"

# Copy all remaining source code for both projects
COPY ./Poseidon/ ./Poseidon/
COPY ./Poseidon.Tests/ ./Poseidon.Tests/

# Build the main API project
WORKDIR /src/Poseidon
RUN dotnet build "Poseidon.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Build the test project
WORKDIR /src/Poseidon.Tests
RUN dotnet build "Poseidon.Tests.csproj" -c $BUILD_CONFIGURATION

# Stage 3: Run tests and publish results
WORKDIR /src/Poseidon.Tests

# Ensure the /test-results directory exists
RUN mkdir -p /test-results

# Skip integration tests during build, run only unit tests
RUN dotnet test "Poseidon.Tests.csproj" --filter Category!=Integration --logger trx --results-directory /test-results

# Stage 4: Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/Poseidon
RUN dotnet publish "Poseidon.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 5: Final runtime image for deployment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poseidon.dll"]
