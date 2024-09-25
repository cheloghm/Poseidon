## Poseidon API Orchestrator

**Poseidon API Orchestrator** is a RESTful web API built using **.NET 8** and **MongoDB**. The API serves as a showcase for handling complex backend engineering tasks, including user authentication, data storage, and deployment in a containerized environment using **Docker** and **Kubernetes**.

This project includes the following key components:
- **RESTful API** with **.NET 8**
- **MongoDB** for data storage
- **JWT-based Authentication**
- **Swagger Documentation**
- **Seeding MongoDB** with Titanic dataset using **Docker**
- **CI/CD Pipelines** using **GitHub Actions**
- **Deployment** in **Kubernetes** using **Minikube**
- **DevSecOps** practices integrated with **Trivy** for security scanning

### **Goal of the Project**
The main goal of this project is to demonstrate expertise in building a scalable, secure, and maintainable API that interacts with MongoDB, follows best practices for software development, and can be easily deployed in a Kubernetes environment. The project also serves as a foundation for building more advanced features and services for future applications.

---

## **Features**

- **User Registration and Authentication**: Register and authenticate users using JWT-based tokens.
- **Passenger Data Management**: CRUD operations on the Passenger dataset (Titanic data).
- **Swagger Documentation**: API endpoints documented using Swagger, accessible at `/swagger`.
- **Database Seeding**: MongoDB seeding with sample data (Titanic dataset).
- **Deployment**: Kubernetes deployment files included for seamless containerization and orchestration.
- **CI/CD**: Continuous Integration and Deployment pipelines using GitHub Actions.
- **DevSecOps**: Integrated with Trivy for vulnerability scanning of Docker images.

---

## **Project Structure**

```plaintext
Poseidon/
│
├── Mongo-seed/                  # Directory for MongoDB seeding project
│   ├── Dockerfile               # Dockerfile for building MongoDB seeding image
│   ├── init-mongo.sh            # MongoDB initialization and seeding script
│   └── titanic.csv              # Titanic dataset used for MongoDB seeding
│
├── Poseidon/                    # Main Poseidon API project
│   ├── Controllers/             # API Controllers (handle incoming HTTP requests)
│   │   └── UserController.cs    # Controller for user-related endpoints
│   │   └── PassengerController.cs  # Controller for passenger-related endpoints
│   │   └── ...                  # Additional controllers as needed
│   │
│   ├── Services/                # Business logic and service layer
│   │   └── UserService.cs       # Service for user-related business logic
│   │   └── PassengerService.cs  # Service for passenger-related business logic
│   │   └── ...                  # Other services as necessary
│   │
│   ├── Models/                  # Data models (representing MongoDB documents)
│   │   └── User.cs              # Model for User entity
│   │   └── Passenger.cs         # Model for Passenger entity
│   │   └── ...                  # Other models as needed
│   │
│   ├── Repositories/            # Data access layer (interacting with MongoDB)
│   │   └── UserRepository.cs    # Repository for User-related database operations
│   │   └── PassengerRepository.cs  # Repository for Passenger-related database operations
│   │   └── ...                  # Additional repositories as needed
│   │
│   ├── DTOs/                    # Data Transfer Objects (used to transfer data)
│   │   └── CreateUserDTO.cs     # DTO for user creation requests
│   │   └── PassengerDTO.cs      # DTO for passenger data transfer
│   │   └── ...                  # Other DTOs as necessary
│   │
│   ├── Config/                  # Configuration files
│   │   └── JwtConfig.cs         # Configuration for JWT authentication
│   │   └── DatabaseConfig.cs    # Configuration for MongoDB connection
│   │   └── ...                  # Other configuration files
│   │
│   ├── Filters/                 # Filters for request processing (e.g., logging, validation)
│   │   └── LoggingFilter.cs     # Example filter for logging requests
│   │
│   ├── Utilities/               # Helper functions and utilities
│   │   └── JwtUtility.cs        # Utility for handling JWT operations
│   │   └── ...                  # Other utility functions
│   │
│   ├── k8s/                     # Kubernetes manifests for deploying to K8s cluster
│   │   └── poseidon-deployment.yml  # Deployment file for Poseidon API
│   │   └── mongodb-deployment.yml   # Deployment file for MongoDB
│   │   └── poseidon-service.yml     # Service file for Poseidon API
│   │   └── mongodb-service.yml      # Service file for MongoDB
│   │
│   ├── k8s-configs/             # Kubernetes ConfigMaps and Secrets
│   │   └── configmap.yml        # ConfigMap for non-sensitive environment variables
│   │   └── secret.yml           # Secret for sensitive environment variables
│   │
│   ├── CICD/                    # CI/CD pipeline configurations
│   │   └── ci-cd-pipeline.yml   # GitHub Actions pipeline for CI/CD
│   │
│   ├── BackgroudTasks/          # Background tasks (e.g., token cleanup)
│   │   └── CleanupExpiredTokensTask.cs  # Background task for cleaning up expired tokens
│   │
│   ├── Data/                    # MongoDB context and data-related classes
│   │   └── PoseidonContext.cs   # MongoDB context for the application
│   │
│   ├── Middlewares/             # Custom middlewares for request handling
│   │   └── RateLimitingMiddleware.cs  # Middleware for rate limiting
│   │
│   ├── Security/                # Security configurations and tools
│   │   └── trivy-config.yaml    # Trivy security scanner configuration
│   │
│   └── Program.cs               # Main entry point for the application
│   └── appsettings.json         # Application settings file
│   └── appsettings.Development.json  # Development environment settings
│   └── Poseidon.csproj          # Project file for the Poseidon API project
│
├── Docs/                        # Project documentation
│   ├── ProjectOverview.md       # Overview of the project
│   ├── InstallationGuide.md     # Installation instructions for setting up the project
│   ├── APIDocumentation.md      # Detailed API documentation (endpoints, methods, etc.)
│   ├── DatabaseSchema.md        # Description of MongoDB schema and collections
│   ├── DeploymentGuide.md       # Instructions for deploying the project with Kubernetes
│   ├── CONTRIBUTING.md          # Contribution guidelines for other developers
│   ├── TestingGuide.md          # Information about testing strategies
│   ├── SecurityGuide.md         # Details about security measures in the project
│
├── .env                         # Environment variables (for local development)
├── docker-compose.yml           # Docker Compose file for local development (API and MongoDB)
├── Dockerfile                   # Dockerfile for building Poseidon API image
└── Poseidon.sln                 # Solution file for the Poseidon project
```

---

## **Getting Started**

Follow these steps to set up and run the project on your local machine or within a Kubernetes environment.

### **Prerequisites**
- **.NET 8 SDK**
- **Docker**
- **Kubernetes** with **Minikube** (for local testing)
- **MongoDB** (either local or via Docker)

### **Installation**

Please refer to the [Installation Guide](Docs/InstallationGuide.md) for step-by-step instructions on setting up the project locally, running the API with Docker, and deploying it using Kubernetes.

---

## **Usage**

### **1. User Authentication**
The API allows you to register and authenticate users using JWT tokens. The registration endpoint requires one of two roles: **Admin** or **User**.

- **Register a User:**
  `POST /api/User/register`

- **Login:**
  `POST /api/User/login`

Refer to the [API Documentation](Docs/APIDocumentation.md) for a full list of endpoints and usage examples.

### **2. Passenger Data Management**
The API provides CRUD operations for managing passenger data from the Titanic dataset. You can create, read, update, and delete passenger information.

- **List all passengers:**
  `GET /api/Passenger/all`

For more information, see the [API Documentation](Docs/APIDocumentation.md).

---

## **Documentation**

For more detailed information about the project, visit the following sections in the `Docs` folder:

1. [**Project Overview**](Docs/ProjectOverview.md): High-level overview of the project’s purpose and features.
2. [**Installation Guide**](Docs/InstallationGuide.md): Step-by-step instructions to set up and run the project.
3. [**API Documentation**](Docs/APIDocumentation.md): Detailed API endpoints with examples.
4. [**Database Schema**](Docs/DatabaseSchema.md): Structure of the MongoDB collections.
5. [**Deployment Guide**](Docs/DeploymentGuide.md): Instructions on deploying the API using Kubernetes.
6. [**Contribution Guide**](Docs/CONTRIBUTING.md): Instructions for contributing to the project.

---

## **Deployment**

The project is containerized using Docker and can be deployed to Kubernetes. The following commands will help you deploy it locally using Minikube:

```bash
# Deploy MongoDB and Poseidon API to Kubernetes
kubectl apply -f Poseidon/k8s/mongodb-deployment.yml
kubectl apply -f Poseidon/k8s/poseidon-deployment.yml
```

For more details, check out the [Deployment Guide](Docs/DeploymentGuide.md).

---

## **CI/CD and Security**

- **CI/CD Pipeline**: The project uses GitHub Actions for continuous integration and deployment. Learn more about the setup in the [CI/CD Pipeline Guide](Docs/InstallationGuide.md).
- **Security**: The project integrates **Trivy** to scan Docker images for vulnerabilities. Learn more in the [Security Guide](Docs/SecurityGuide.md).

---

## **Contributing**

Contributions are welcome! Please read the [Contribution Guide](Docs/CONTRIBUTING.md) for more details on how to contribute to this project.

---

## **License**

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for more information.

---

### **Next Steps**
