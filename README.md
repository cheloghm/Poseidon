```markdown
# Poseidon API Orchestrator

![Poseidon Logo](https://your-logo-url.com/logo.png) <!-- Replace with your actual logo URL -->

**Poseidon API Orchestrator** is a robust RESTful web API built with **.NET 8** and **MongoDB**. Designed to handle complex backend engineering tasks, this API facilitates user authentication, data management, and seamless deployment in both local and cloud environments using **Docker**, **Kubernetes**, and **AWS**.

---
  
## Table of Contents

1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Project Structure](#project-structure)
4. [Prerequisites](#prerequisites)
5. [Getting Started](#getting-started)
    - [Clone the Repository](#clone-the-repository)
    - [Set Up Environment Variables](#set-up-environment-variables)
    - [Install Dependencies](#install-dependencies)
    - [Build and Run Locally](#build-and-run-locally)
6. [Environment Variables Customization](#environment-variables-customization)
7. [Docker Image Creation and Deployment](#docker-image-creation-and-deployment)
    - [Building the Docker Image](#building-the-docker-image)
    - [Pushing the Docker Image to Docker Hub](#pushing-the-docker-image-to-docker-hub)
8. [Running the Project](#running-the-project)
    - [Using Docker Compose](#using-docker-compose)
    - [Using Kubernetes](#using-kubernetes)
    - [Deploying to AWS](#deploying-to-aws)
9. [API Endpoints](#api-endpoints)
10. [Documentation](#documentation)
11. [CI/CD and Security](#cicd-and-security)
12. [Contributing](#contributing)
13. [License](#license)
14. [Next Steps](#next-steps)
15. [Acknowledgements](#acknowledgements)
16. [Important Notice](#important-notice)

---

## Project Overview

**Poseidon API Orchestrator** serves as a showcase of best practices in building scalable, secure, and maintainable APIs. It leverages modern technologies and methodologies to ensure seamless interaction with MongoDB, adherence to software development best practices, and effortless deployment within both local and cloud (AWS) environments.

---

## Features

- **User Registration and Authentication**: Securely register and authenticate users using JWT-based tokens.
- **Passenger Data Management**: Perform CRUD operations on the Passenger dataset (Titanic data).
- **Swagger Documentation**: Comprehensive API documentation accessible at `/swagger`.
- **Database Seeding**: Seed MongoDB with sample data (Titanic dataset) using Docker.
- **Deployment**: Docker Compose for local deployments and Terraform with Terragrunt for AWS deployments.
- **CI/CD**: Automated Continuous Integration and Deployment pipelines using GitHub Actions.
- **DevSecOps**: Integrated with **Trivy** for vulnerability scanning of Docker images.
- **Health Checks**: Implemented readiness and liveness probes for Kubernetes.

---

## Project Structure

```plaintext
Poseidon/
│
├── .github/                      # GitHub workflows and configurations
├── AWS/                          # AWS deployment configurations and scripts
│   ├── helm-charts/              # Helm charts for deploying applications
│   │   └── poseidon-app/
│   │       ├── Chart.yaml
│   │       ├── values.yaml
│   │       └── templates/
│   │           ├── api-deployment.yaml
│   │           ├── api-service.yaml
│   │           ├── frontend-deployment.yaml
│   │           ├── frontend-service.yaml
│   │           ├── ingress.yaml
│   │           ├── mongodb-deployment.yaml
│   │           ├── mongodb-service.yaml
│   │           └── logstash/
│   │               ├── logstash-deployment.yaml
│   │               └── logstash-service.yaml
│   ├── k8s/                      # Kubernetes manifests and configurations
│   │   ├── configmaps/
│   │   │   ├── poseidon-config.yaml
│   │   │   └── poseidon-logstash-config.yaml
│   │   ├── cronjobs/
│   │   │   └── mongodb-backup-cronjob.yaml
│   │   ├── deployments/
│   │   │   ├── poseidon-api-deployment.yaml
│   │   │   ├── poseidon-frontend-deployment.yaml
│   │   │   └── poseidon-mongodb-deployment.yaml
│   │   ├── ingress/
│   │   │   └── poseidon-ingress.yaml
│   │   ├── persistentvolumes/
│   │   ├── persistenvolumeclaims/
│   │   │   └── mongo-pvc.yaml
│   │   ├── secrets/
│   │   │   ├── poseidon-external-secret.yaml
│   │   │   └── poseidon-secrets.yaml
│   │   ├── services/
│   │   │   ├── poseidon-api-service.yaml
│   │   │   ├── poseidon-frontend-service.yaml
│   │   │   └── poseidon-mongodb-service.yaml
│   │   └── storageclasses/
│   │       └── aws-ebs-sc.yaml
│   ├── scripts/
│   │   ├── deploy.sh
│   │   └── setup.sh
│   ├── terraform/
│   │   ├── environments/
│   │   │   └── prod/
│   │   │       ├── eks/
│   │   │       │   └── terragrunt.hcl
│   │   │       └── networking/
│   │   │           └── terragrunt.hcl
│   │   ├── modules/
│   │   │   ├── eks/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── kinesis/
│   │   │   │   ├── main.tf
│   │   │   │   ├── variables.tf
│   │   │   │   └── outputs.tf
│   │   │   ├── kms/
│   │   │   │   └── main.tf
│   │   │   ├── networking/
│   │   │   │   └── main.tf
│   │   │   └── vpc/
│   │   │       ├── main.tf
│   │   │       ├── variables.tf
│   │   │       └── outputs.tf
│   │   └── terragrunt.hcl
│   └── README.md                  # AWS Deployment Guide
│
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
│   ├── BackgroundTasks/            # Background tasks (e.g., token cleanup)
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

## Prerequisites

Ensure you have the following installed on your machine:

- **.NET 8 SDK**: [Download Here](https://dotnet.microsoft.com/download)
- **Docker**: [Download Here](https://www.docker.com/get-started)
- **Kubernetes** with **Minikube** (for local testing): [Minikube Installation Guide](https://minikube.sigs.k8s.io/docs/start/)
- **MongoDB**: Either local installation or via Docker
- **Kubectl**: [Install kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
- **AWS Account**: [Sign Up](https://aws.amazon.com/) if you don't have one.
- **Terraform & Terragrunt**: Installed via the setup scripts.
- **Helm**: [Install Helm](https://helm.sh/docs/intro/install/)
- **Git**: [Install Git](https://git-scm.com/downloads)

---

## Getting Started

Follow the steps below to set up and run the **Poseidon API Orchestrator** on your local machine or within a Kubernetes/AWS environment.

### Clone the Repository

To get started, first clone the repository to your local machine:

```bash
git clone https://github.com/yourusername/Poseidon.git
cd Poseidon
```

### Set Up Environment Variables

Create a `.env` file in the root directory based on the provided `.env.example` (if available). Ensure all necessary environment variables are defined.

**Sample `.env` File:**

```env
# .env

# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=admin
MONGO_INITDB_ROOT_PASSWORD=yourpassword
MONGO_DB_NAME=PoseidonDB

# Database Configuration
DatabaseConfig__ConnectionStringDocker=mongodb://admin:yourpassword@mongodb:27017/PoseidonDB
DatabaseConfig__DatabaseName=PoseidonDB

# JWT Configuration
Jwt__Key=YourSuperSecretKey
Jwt__Issuer=PoseidonAPI
Jwt__Audience=PoseidonClients
```

**Note:** Replace placeholder values (`yourpassword`, `YourSuperSecretKey`, etc.) with your actual configuration details.

### Install Dependencies

Navigate to the main project directory and restore the necessary packages:

```bash
cd Poseidon
dotnet restore
```

### Build and Run Locally

To run the API locally using .NET CLI:

```bash
cd Poseidon
dotnet run
```

The API will start on the configured port (e.g., [http://localhost:8080](http://localhost:8080)).

---

## Environment Variables Customization

To allow flexibility in different environments (e.g., Docker Compose, Kubernetes), you can customize the service name and port used by the Poseidon API.

### Kubernetes Deployment Configuration

The Poseidon API uses environment variables to determine its service configurations. These variables are defined in the Kubernetes deployment files.

#### Changing Service Name and Port in Kubernetes

1. **Open Kubernetes Deployment File:**

   **File Path:** `k8s/Deployments/poseidon-deployment.yml`

2. **Update Container Port:**

   ```yaml
   ports:
     - containerPort: 8080
   ```

3. **Update Service Name and Port:**

   Ensure the Kubernetes Service (`k8s/Services/poseidon-service.yml`) has the desired service name and port.

   **Example: Changing Service to `poseidon-api` on port `8080`**

   **Deployment File (`poseidon-deployment.yml`):**

   ```yaml
   ports:
     - containerPort: 8080
   ```

   **Service File (`poseidon-service.yml`):**

   ```yaml
   ports:
     - protocol: TCP
       port: 80
       targetPort: 8080
       name: http
     - protocol: TCP
       port: 443
       targetPort: 8443
       name: https
   selector:
     app: poseidon
   ```

   **Note:** Ensure consistency between the Deployment and Service port configurations.

### Docker Compose Configuration

1. **Open Docker Compose File:**

   **File Path:** `docker-compose.yml`

2. **Update Service Name and Port:**

   ```yaml
   services:
     poseidon-api:
       build:
         context: .
         dockerfile: Dockerfile
       container_name: poseidon_api
       ports:
         - "9090:8080"  # Host port 9090 maps to container port 8080
       environment:
         MONGO_INITDB_ROOT_USERNAME: ${MONGO_INITDB_ROOT_USERNAME}
         MONGO_INITDB_ROOT_PASSWORD: ${MONGO_INITDB_ROOT_PASSWORD}
         MONGO_DB_NAME: ${MONGO_DB_NAME}
         DatabaseConfig__ConnectionStringDocker: ${DatabaseConfig__ConnectionStringDocker}
         DatabaseConfig__DatabaseName: ${DatabaseConfig__DatabaseName}
         Jwt__Key: ${Jwt__Key}
         Jwt__Issuer: ${Jwt__Issuer}
         Jwt__Audience: ${Jwt__Audience}
         API_SERVICE: poseidon-api   # Change to your API service name
         API_PORT: "80"              # Change to your API service port
       depends_on:
         - mongodb
       networks:
         - poseidon-net
   ```

3. **Frontend Configuration (If Applicable):**

   Ensure that the frontend service references the updated `API_SERVICE` and `API_PORT` accordingly.

---

## Docker Image Creation and Deployment

To deploy the Poseidon API Orchestrator using Docker, you'll need to build a Docker image and push it to a container registry (e.g., Docker Hub). Below are the steps to create and deploy the Docker image.

### Building the Docker Image

1. **Navigate to the Project Directory:**

   ```bash
   cd Poseidon
   ```

2. **Build the Docker Image:**

   ```bash
   docker build -t your-dockerhub-username/poseidon-api:latest .
   ```

   - **Replace `your-dockerhub-username`** with your actual Docker Hub username.
   - **`:latest`** is the tag for the image version. You can use other tags as needed.

3. **Verify the Image:**

   ```bash
   docker images
   ```

   You should see `your-dockerhub-username/poseidon-api` listed among the images.

### Pushing the Docker Image to Docker Hub

1. **Log in to Docker Hub:**

   ```bash
   docker login
   ```

   Enter your Docker Hub credentials when prompted.

2. **Push the Image:**

   ```bash
   docker push your-dockerhub-username/poseidon-api:latest
   ```

   - **Replace `your-dockerhub-username`** with your actual Docker Hub username.

3. **Verify the Image on Docker Hub:**

   Navigate to [Docker Hub](https://hub.docker.com/) and check your repositories to ensure the image has been pushed successfully.

---

## Running the Project

You can run the **Poseidon API Orchestrator** using **Docker Compose**, **Kubernetes**, or deploy it to **AWS**. Below are the instructions for each method.

### Using Docker Compose

**Prerequisites:**

- Docker and Docker Compose installed on your machine.
- The frontend services (if any) are running or accessible at the configured `API_BASE_URL`.

**Steps:**

1. **Navigate to the Project Root Directory:**

   ```bash
   cd Poseidon
   ```

2. **Ensure Docker Compose Configuration:**

   Verify that the `docker-compose.yml` is correctly configured with all necessary services (Poseidon API, MongoDB, Mongo-seed).

3. **Start the Services:**

   ```bash
   docker-compose up -d
   ```

   - The `-d` flag runs the containers in detached mode.

4. **Seed MongoDB with Titanic Dataset:**

   The `Mongo-seed` service is responsible for seeding the database. It should run automatically as part of Docker Compose.

   **Check Seed Logs:**

   ```bash
   docker logs mongo-seeder
   ```

   Ensure that the seeding process completed successfully.

5. **Access the API:**

   Open your browser and navigate to [http://localhost:9090/swagger](http://localhost:9090/swagger) to view the API documentation.

   **Access the Frontend (If Applicable):**

   If a frontend service is available, it will be accessible at [http://localhost:3000](http://localhost:3000).

**Notes:**

- Ensure that the `API_SERVICE` and `API_PORT` environment variables in `docker-compose.yml` match your backend service configurations.
- If deploying frontend and backend in separate Docker Compose files, ensure they share the same Docker network for inter-service communication.

### Using Kubernetes

**Prerequisites:**

- A Kubernetes cluster set up (e.g., Minikube, GKE, EKS).
- `kubectl` configured to interact with your cluster.
- The Docker image for Poseidon API pushed to a container registry accessible by your Kubernetes cluster.

**Steps:**

1. **Navigate to the Kubernetes Manifests Directory:**

   ```bash
   cd Poseidon/k8s
   ```

2. **Apply Kubernetes Manifests:**

   Deploy MongoDB, Poseidon API, and other necessary services to Kubernetes.

   ```bash
   kubectl apply -f Deployments/mongodb-deployment.yml
   kubectl apply -f Services/mongodb-service.yml
   kubectl apply -f Deployments/poseidon-deployment.yml
   kubectl apply -f Services/poseidon-service.yml
   ```

3. **Set Up ConfigMaps and Secrets:**

   Apply ConfigMaps and Secrets for environment variables and sensitive data.

   ```bash
   kubectl apply -f k8s-configs/configmap.yml
   kubectl apply -f k8s-configs/secret.yml
   ```

4. **Run CronJobs (If Applicable):**

   ```bash
   kubectl apply -f CronJob/mongodb-image-backup-cronjob.yml
   ```

5. **Verify Deployments and Services:**

   ```bash
   kubectl get pods
   kubectl get services
   ```

6. **Access the API:**

   - **Port Forwarding:**

     ```bash
     kubectl port-forward service/poseidon-service 8080:80
     ```

     Access the API documentation at [http://localhost:8080/swagger](http://localhost:8080/swagger).

   - **Using LoadBalancer (if supported):**

     ```bash
     kubectl get svc poseidon-service
     ```

     Note the external IP and navigate to it in your browser.

7. **Monitor Health Checks:**

   Ensure that readiness and liveness probes are passing.

   ```bash
   kubectl describe pod <poseidon-pod-name>
   ```

**Notes:**

- Ensure that the Poseidon API image in the deployment manifest points to the correct Docker image repository and tag.
- Update the `API_SERVICE` and `API_PORT` environment variables in the Kubernetes Deployment files if you have customized them.

### Deploying to AWS

Deploying the **Poseidon API Orchestrator** to AWS leverages **Terraform**, **Terragrunt**, and **Helm** for infrastructure provisioning and application deployment. Follow the steps below to deploy to AWS.

#### Prerequisites

- **AWS Account**: [Sign Up](https://aws.amazon.com/) if you don't have one.
- **AWS CLI Configured**: Ensure your AWS credentials are set up. You can set them using the AWS CLI:
  
  ```bash
  aws configure
  ```

- **Terraform & Terragrunt**: Installed via the setup scripts.
- **Kubectl & Helm**: Installed via the setup scripts.
- **Permissions**: Ensure your AWS IAM user has the necessary permissions for Terraform to create resources.

#### Steps

1. **Navigate to the AWS Directory:**

   ```bash
   cd Poseidon/AWS
   ```

2. **Run the Setup Script:**

   The `setup.sh` script installs necessary tools such as AWS CLI, Terraform, Terragrunt, kubectl, and Helm.

   ```bash
   chmod +x scripts/setup.sh
   ./scripts/setup.sh
   ```

   **What the Setup Script Does:**

   - **Installs AWS CLI:** Command-line tool for interacting with AWS services.
   - **Installs Terraform:** Infrastructure as Code (IaC) tool for provisioning AWS resources.
   - **Installs Terragrunt:** A thin wrapper for Terraform that provides extra features like DRY configurations and remote state management.
   - **Installs kubectl:** Kubernetes command-line tool for interacting with the cluster.
   - **Installs Helm:** Kubernetes package manager for deploying applications.
   - **Installs kubectx and kubens (optional):** Tools for switching between Kubernetes contexts and namespaces.

   *Ensure you have administrative privileges to execute the script.*

3. **Initialize and Deploy Infrastructure:**

   Deploy the AWS infrastructure using Terragrunt.

   ```bash
   cd terraform/environments/prod
   terragrunt init
   terragrunt apply -auto-approve
   ```

   **What This Step Does:**

   - **Provisions AWS Resources:** VPC, subnets, EKS cluster, IAM roles, security groups, and Kinesis streams.
   - **Manages Remote State:** Uses S3 bucket and DynamoDB table for storing Terraform state and managing locks.

   **Note:** Ensure that the S3 bucket (`poseidon-terraform-state`) and DynamoDB table (`poseidon-terraform-lock`) exist. If not, create them via the AWS Console or using Terraform scripts.

4. **Deploy Applications to Kubernetes on AWS:**

   After the infrastructure is up, deploy the Kubernetes applications using the deploy script.

   ```bash
   cd ../../scripts
   chmod +x deploy.sh
   ./deploy.sh
   ```

   **What the Deploy Script Does:**

   1. **Applies Kubernetes Manifests:** Deployments, Services, ConfigMaps, Secrets, PersistentVolumeClaims.
   2. **Installs NGINX Ingress Controller:** Manages external access to services.
   3. **Deploys CronJobs:** Sets up scheduled backups for MongoDB.
   4. **Deploys ELK Stack and DataDog:** For logging and monitoring.
   5. **Deploys Poseidon Application via Helm:** Manages API, Frontend, MongoDB, and Logstash.

   **Important:** Replace `<YOUR_DATADOG_API_KEY>` and `<YOUR_DATADog_APP_KEY>` in the `deploy.sh` script with your actual DataDog credentials before executing.

5. **Verify Deployments:**

   Check the status of your deployments to ensure everything is running correctly.

   ```bash
   kubectl get pods -A
   ```

   You should see pods for the API, MongoDB, Frontend, Logstash, Ingress Controller, Elasticsearch, Kibana, DataDog agents, etc., all in the `Running` state.

6. **Access the Application:**

   After successful deployment, access your application via the Load Balancer URL associated with your Ingress. You can find the Load Balancer DNS in the `poseidon-ingress` service.

   ```bash
   kubectl get ingress poseidon-ingress
   ```

   **Example Output:**

   ```
   NAME               CLASS    HOSTS              ADDRESS                                         PORTS   AGE
   poseidon-ingress   <none>   your-domain.com     abcdef1234567890.elb.amazonaws.com             80      10m
   ```

   Navigate to `http://your-domain.com` in your browser to access the frontend and `http://your-domain.com/api` for the API.

---

## API Endpoints

The Poseidon API Orchestrator provides a comprehensive set of endpoints for user management and passenger data operations. Below are the main endpoints:

### **User Endpoints**

- **POST** `/api/User/register`: Register a new user.

  **Request Body:**

  ```json
  {
    "id": "unique-user-id",
    "username": "johndoe",
    "email": "johndoe@example.com",
    "password": "SecurePassword123!",
    "role": "User" // or "Admin"
  }
  ```

- **POST** `/api/User/login`: Authenticate a user and retrieve a JWT token.

  **Request Body:**

  ```json
  {
    "username": "johndoe",
    "password": "SecurePassword123!"
  }
  ```

- **GET** `/api/User/me`: Fetch details of the authenticated user.

  **Headers:**

  ```
  Authorization: Bearer <JWT_TOKEN>
  ```

### **Passenger Endpoints**

- **GET** `/api/Passenger/all`: Retrieve all passengers.

- **GET** `/api/Passenger/survivors`: Retrieve all survivors.

- **GET** `/api/Passenger/class/{classNumber}`: Retrieve passengers by class.

- **GET** `/api/Passenger/gender/{sex}`: Retrieve passengers by gender.

- **POST** `/api/Passenger/create`: Create a new passenger.

  **Request Body:**

  ```json
  {
    "name": "John Doe",
    "age": 30,
    "ticketNumber": "A12345",
    "cabin": "C123",
    "fare": 72.50,
    "survived": true
  }
  ```

- **PUT** `/api/Passenger/update/{id}`: Update passenger details (Admin only).

  **Request Body:**

  ```json
  {
    "name": "John Doe",
    "age": 31,
    "ticketNumber": "A12345",
    "cabin": "C124",
    "fare": 75.00,
    "survived": true
  }
  ```

- **DELETE** `/api/Passenger/delete/{id}`: Delete a passenger (Admin only).

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

## Acknowledgements

- **.NET Community**: For providing a robust framework for building APIs.
- **MongoDB**: For offering a flexible and scalable NoSQL database solution.
- **Docker & Kubernetes**: For enabling seamless containerization and orchestration.
- **GitHub Actions**: For facilitating efficient CI/CD workflows.
- **Trivy**: For ensuring Docker images are secure through vulnerability scanning.
- **Contributors**: Special thanks to all the contributors who have helped make this project possible.

---

## Important Notice

The AWS deployment configurations and guidelines located in the AWS directory are currently unverified and have not undergone comprehensive testing. While I have meticulously designed these instructions and setup scripts to facilitate a seamless deployment process, I advise users to follow the guidelines with careful attention to detail. Should you encounter any challenges or have suggestions for enhancements, your feedback is highly appreciated. Please consider opening an issue or contributing directly to help me refine and validate the AWS deployment workflow.

---
