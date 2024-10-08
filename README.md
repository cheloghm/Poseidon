# Poseidon API Orchestrator

![Poseidon Logo](https://your-logo-url.com/logo.png) <!-- Replace with your actual logo URL -->

**Poseidon API Orchestrator** is a robust RESTful web API built with **.NET 8** and **MongoDB**. Designed to handle complex backend engineering tasks, this API facilitates user authentication, data management, and seamless deployment in a containerized environment using **Docker** and **Kubernetes**.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Project Structure](#project-structure)
4. [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
5. [Usage](#usage)
    - [User Authentication](#user-authentication)
    - [Passenger Data Management](#passenger-data-management)
6. [Documentation](#documentation)
7. [Deployment](#deployment)
8. [CI/CD and Security](#cicd-and-security)
9. [Contributing](#contributing)
10. [License](#license)
11. [Next Steps](#next-steps)
12. [Acknowledgements](#acknowledgements)

---

## Project Overview

**Poseidon API Orchestrator** serves as a showcase of best practices in building scalable, secure, and maintainable APIs. It leverages modern technologies and methodologies to ensure seamless interaction with MongoDB, adherence to software development best practices, and effortless deployment within Kubernetes environments.

---

## Features

- **User Registration and Authentication**: Securely register and authenticate users using JWT-based tokens.
- **Passenger Data Management**: Perform CRUD operations on the Passenger dataset (Titanic data).
- **Swagger Documentation**: Comprehensive API documentation accessible at `/swagger`.
- **Database Seeding**: Seed MongoDB with sample data (Titanic dataset) using Docker.
- **Deployment**: Kubernetes deployment files for containerization and orchestration.
- **CI/CD**: Automated Continuous Integration and Deployment pipelines using GitHub Actions.
- **DevSecOps**: Integrated with **Trivy** for vulnerability scanning of Docker images.
- **Health Checks**: Implemented readiness and liveness probes for Kubernetes.

---

## Project Structure

```plaintext
Poseidon/
│
├── .github/                      # GitHub workflows and configurations
├── data/                         # Data-related files and scripts
├── Docs/                         # Comprehensive project documentation
│   ├── APIDocumentation.md       # Detailed API endpoints and usage
│   ├── ContributionGuide.md      # Guidelines for contributing to the project
│   ├── DatabaseSchema.md         # MongoDB schema and collection details
│   ├── DeploymentGuide.md        # Steps to deploy the project using Kubernetes
│   ├── InstallationGuide.md      # Instructions to set up and run the project locally
│   ├── ProjectOverview.md        # High-level overview of the project
│   ├── SecurityGuide.md          # Security measures and best practices implemented
│   └── TestingGuide.md           # Testing strategies and procedures
│
├── k8s/                          # Kubernetes manifests and configurations
│   ├── CronJob/
│   │   └── mongodb-image-backup-cronjob.yml    # CronJob for MongoDB backups
│   ├── Deployments/
│   │   ├── mongodb-deployment.yml    # Deployment for MongoDB
│   │   └── poseidon-deployment.yml   # Deployment for Poseidon API
│   ├── k8s-configs/
│   │   ├── configmap.yml             # ConfigMaps for non-sensitive configurations
│   │   └── secret.yml                # Secrets for sensitive data
│   ├── Security/
│   │   └── trivy-config.yml          # Trivy security scanning configurations
│   ├── Services/
│   │   ├── mongodb-service.yml       # Service for MongoDB
│   │   └── poseidon-service.yml      # Service for Poseidon API
│   └── Volumes/
│       ├── PersistentVolume.yml      # Persistent Volume definitions
│       └── PersistentVolumeClaim.yml # Persistent Volume Claims
│
├── Mongo-seed/                    # MongoDB seeding project
│   ├── .dockerignore               # Docker ignore file
│   ├── Dockerfile                   # Dockerfile for MongoDB seeding image
│   ├── init-mongo.sh                # MongoDB initialization and seeding script
│   └── titanic.csv                  # Titanic dataset used for MongoDB seeding
│
├── Poseidon/                      # Main Poseidon API project
│   ├── BackgroudTasks/            # Background tasks (e.g., token cleanup)
│   ├── bin/                        # Compiled binaries
│   ├── Config/                     # Configuration files (JWT, MongoDB, etc.)
│   ├── Controllers/                # API Controllers handling HTTP requests
│   ├── Data/                       # MongoDB context and data-related classes
│   ├── DTOs/                       # Data Transfer Objects for data exchange
│   ├── Enums/                      # Enumerations used in the project
│   ├── EventHandlers/              # Event handling mechanisms
│   ├── Events/                     # Event definitions
│   ├── Extensions/                 # Extension methods and utilities
│   ├── Filters/                    # Request processing filters (e.g., logging, validation)
│   ├── Helpers/                    # Helper functions and utilities
│   ├── Interfaces/                 # Interface definitions
│   ├── logs/                       # Logging configurations and files
│   ├── Mappers/                    # Object mappers (e.g., AutoMapper profiles)
│   ├── Middlewares/                # Custom middlewares for request handling
│   ├── Models/                     # Data models representing MongoDB documents
│   ├── obj/                        # Object files
│   ├── Properties/                 # Project properties and settings
│   ├── Repositories/               # Data access layer interacting with MongoDB
│   ├── Services/                   # Business logic and service layer
│   ├── Utilities/                  # Utility functions and helper classes
│   ├── Validators/                 # Data validation logic
│   ├── ViewModels/                 # View models for data representation
│   ├── Poseidon.csproj              # Project file
│   ├── Poseidon.csproj.user         # User-specific project settings
│   ├── Poseidon.http                # HTTP request samples
│   └── Program.cs                   # Main entry point for the application
│
├── Poseidon.Tests/                # Test project for Poseidon API
├── .dockerignore                   # Docker ignore file for the main project
├── .env                            # Environment variables for local development
├── .gitattributes                  # Git attributes file
├── .gitignore                      # Git ignore file
├── bfg-1.14.0.jar                  # BFG Repo-Cleaner tool
├── docker-compose.yml              # Docker Compose file for local development
├── Dockerfile                      # Dockerfile for building Poseidon API image
├── LICENSE.txt                     # License information
├── Poseidon.sln                    # Solution file for the Poseidon project
└── README.md                       # This README file
```

---

## Getting Started

Follow the steps below to set up and run the **Poseidon API Orchestrator** on your local machine or within a Kubernetes environment.

### Prerequisites

Ensure you have the following installed on your machine:

- **.NET 8 SDK**: [Download Here](https://dotnet.microsoft.com/download)
- **Docker**: [Download Here](https://www.docker.com/get-started)
- **Kubernetes** with **Minikube** (for local testing): [Minikube Installation Guide](https://minikube.sigs.k8s.io/docs/start/)
- **MongoDB**: Either local installation or via Docker

### Installation

For detailed installation instructions, refer to the [Installation Guide](Docs/InstallationGuide.md) in the `Docs` folder. Below is a high-level overview:

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/Poseidon.git
   cd Poseidon
   ```

2. **Set Up Environment Variables**

   Create a `.env` file in the root directory based on the provided `.env.example` (if available). Ensure all necessary environment variables are defined.

3. **Run MongoDB and Poseidon API Locally with Docker Compose**

   ```bash
   docker-compose up -d
   ```

4. **Seed MongoDB with Titanic Dataset**

   Navigate to the `Mongo-seed` directory and run the seeding script.

   ```bash
   cd Mongo-seed
   docker build -t cheloghm/mongo-seed:latest .
   docker run --rm cheloghm/mongo-seed:latest
   ```

5. **Access the API**

   Once Docker Compose is up and MongoDB is seeded, access the API documentation at [http://localhost:8080/swagger](http://localhost:8080/swagger).

---

## Usage

### User Authentication

The API facilitates user registration and authentication using JWT tokens. Users can have roles such as **Admin** or **User**.

- **Register a User**

  ```bash
  curl -X 'POST' \
    'http://localhost:8080/api/User/register' \
    -H 'accept: */*' \
    -H 'Content-Type: application/json' \
    -d '{
    "id": "unique-user-id",
    "username": "johndoe",
    "email": "johndoe@example.com",
    "password": "SecurePassword123!",
    "role": "User"
  }'
  ```

- **Authenticate a User**

  ```bash
  curl -X 'POST' \
    'http://localhost:8080/api/User/login' \
    -H 'accept: */*' \
    -H 'Content-Type: application/json' \
    -d '{
    "username": "johndoe",
    "password": "SecurePassword123!"
  }'
  ```

### Passenger Data Management

Manage passenger data from the Titanic dataset with CRUD operations.

- **List All Passengers**

  ```bash
  curl -X 'GET' \
    'http://localhost:8080/api/Passenger/all' \
    -H 'accept: */*'
  ```

- **Get Passenger by ID**

  ```bash
  curl -X 'GET' \
    'http://localhost:8080/api/Passenger/{id}' \
    -H 'accept: */*'
  ```

- **Create a New Passenger**

  ```bash
  curl -X 'POST' \
    'http://localhost:8080/api/Passenger/create' \
    -H 'accept: */*' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "John Doe",
    "age": 30,
    "ticketNumber": "A12345",
    "cabin": "C123",
    "fare": 72.50,
    "survived": true
  }'
  ```

- **Update a Passenger**

  ```bash
  curl -X 'PUT' \
    'http://localhost:8080/api/Passenger/update/{id}' \
    -H 'accept: */*' \
    -H 'Content-Type: application/json' \
    -d '{
    "name": "John Doe",
    "age": 31,
    "ticketNumber": "A12345",
    "cabin": "C124",
    "fare": 75.00,
    "survived": true
  }'
  ```

- **Delete a Passenger**

  ```bash
  curl -X 'DELETE' \
    'http://localhost:8080/api/Passenger/delete/{id}' \
    -H 'accept: */*'
  ```

For a complete list of endpoints and their usage, refer to the [API Documentation](Docs/APIDocumentation.md).

---

## Documentation

For detailed information about various aspects of the project, refer to the comprehensive documentation available in the `Docs` folder:

1. [**Project Overview**](Docs/ProjectOverview.md): High-level overview of the project's purpose and features.
2. [**Installation Guide**](Docs/InstallationGuide.md): Step-by-step instructions to set up and run the project locally, run Docker containers, and deploy using Kubernetes.
3. [**API Documentation**](Docs/APIDocumentation.md): Detailed API endpoints, methods, request/response schemas, and usage examples.
4. [**Database Schema**](Docs/DatabaseSchema.md): Structure of MongoDB collections, data models, and relationships.
5. [**Deployment Guide**](Docs/DeploymentGuide.md): Instructions on deploying the API using Kubernetes, including setting up Minikube.
6. [**Testing Guide**](Docs/TestingGuide.md): Information about testing strategies, running tests, and interpreting results.
7. [**Security Guide**](Docs/SecurityGuide.md): Security measures implemented, including JWT authentication and vulnerability scanning with Trivy.
8. [**Contribution Guide**](Docs/ContributionGuide.md): Guidelines for contributing to the project, including coding standards and pull request procedures.

---

## Deployment

The project is containerized using Docker and can be deployed to a Kubernetes cluster. Below are the high-level steps for deploying the **Poseidon API Orchestrator** locally using **Minikube**:

1. **Ensure Minikube is Running**

   Start Minikube if it's not already running:

   ```bash
   minikube start
   ```

2. **Apply Kubernetes Manifests**

   Deploy MongoDB and the Poseidon API to Kubernetes:

   ```bash
   kubectl apply -f k8s/Deployments/mongodb-deployment.yml
   kubectl apply -f k8s/Deployments/poseidon-deployment.yml
   ```

3. **Expose Services**

   Ensure that services are correctly exposed. The `poseidon-service.yml` should already define a `LoadBalancer` type service. To access the API, you might need to set up port forwarding or use Minikube's service tunneling:

   ```bash
   minikube service poseidon-service
   ```

   This command will open the Poseidon API in your default browser.

4. **Verify Deployment**

   Check the status of your pods and services:

   ```bash
   kubectl get pods
   kubectl get services
   ```

For more detailed deployment instructions, refer to the [Deployment Guide](Docs/DeploymentGuide.md).

---

## CI/CD and Security

### Continuous Integration and Deployment

The project utilizes **GitHub Actions** for automating the build, test, and deployment processes. The CI/CD pipeline ensures that code changes are automatically tested and deployed, maintaining code quality and facilitating rapid development.

- **Workflow Configuration**: Located in the `.github/` directory.
- **Key Actions**:
  - **Build**: Compiles the application.
  - **Test**: Runs unit and integration tests.
  - **Docker Build and Push**: Builds Docker images and pushes them to a container registry.
  - **Deployment**: Applies Kubernetes manifests to deploy the latest build.

### DevSecOps

Security is a priority in the Poseidon API Orchestrator project. The integration with **Trivy** ensures that Docker images are scanned for vulnerabilities, maintaining a secure deployment pipeline.

- **Trivy Configuration**: Defined in `k8s/Security/trivy-config.yml`.
- **Vulnerability Scanning**: Automated scans are part of the CI/CD pipeline to detect and address security issues promptly.

For more details, refer to the [Security Guide](Docs/SecurityGuide.md).

---

## Contributing

Contributions are highly appreciated! Whether you're fixing bugs, improving documentation, or adding new features, your input helps make this project better.

Please read our [Contribution Guide](Docs/ContributionGuide.md) for detailed instructions on how to contribute effectively.

---

## License

This project is licensed under the [MIT License](LICENSE.txt). You are free to use, modify, and distribute this project as per the license terms.

---

## Next Steps

- **Advanced Features**: Implement role-based access control (RBAC) for more granular permissions.
- **Monitoring and Logging**: Integrate tools like Prometheus and Grafana for enhanced observability.
- **Scalability Enhancements**: Optimize Kubernetes deployments for better scalability and performance.
- **Automated Backups**: Set up automated backups for MongoDB data.
- **Enhanced Security**: Implement HTTPS with TLS certificates and further security hardening.

Stay tuned for upcoming updates and features!

---

## Acknowledgements

- **.NET Community**: For providing a robust framework for building APIs.
- **MongoDB**: For offering a flexible and scalable NoSQL database solution.
- **Docker & Kubernetes**: For enabling seamless containerization and orchestration.
- **GitHub Actions**: For facilitating efficient CI/CD workflows.
- **Trivy**: For ensuring Docker images are secure through vulnerability scanning.
- **Contributors**: Special thanks to all the contributors who have helped make this project possible.

---

Feel free to reach out if you have any questions or need further assistance!

---

# Additional Steps and Notes

1. **Replace Placeholder Links and Images**: 
   - Ensure that the logo URL in the `Project Overview` section is replaced with your actual project logo.
   - Verify that all links to documentation files in the `Docs` folder are correct and that the files exist.

2. **Update Deployment Commands**:
   - The `curl` commands in the **Usage** section assume that the API is accessible at `localhost:8080`. Adjust these if your Kubernetes service is exposed differently (e.g., using port forwarding or a different port).

3. **Ensure Consistent Naming**:
   - Double-check that all file names and paths in the `Project Structure` section match the actual files in your repository.

4. **Maintain Documentation**:
   - As your project evolves, keep the `Docs` folder updated with the latest information to ensure the `README.md` remains a reliable entry point for new users and contributors.

5. **Testing the README**:
   - Preview the `README.md` on GitHub to ensure that all links, images, and formatting render correctly.
