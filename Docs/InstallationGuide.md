# Poseidon API Orchestrator - Detailed Installation Guide

Welcome to the **Poseidon API Orchestrator** installation guide. This document will walk you through every step necessary to set up and run the Poseidon project locally, using Docker, or within a Kubernetes environment. Whether you're a seasoned developer or just getting started, this guide is designed to help you deploy the project seamlessly.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Cloning the Repository](#cloning-the-repository)
3. [Environment Setup](#environment-setup)
    - [Root `.env` File for Docker/Kubernetes](#root-env-file-for-dockerkubernetes)
    - [API Project `.env` File for Local Development](#api-project-env-file-for-local-development)
4. [Running the Project Locally (IDE like Visual Studio)](#running-the-project-locally-ide-like-visual-studio)
5. [Running the Project with Docker](#running-the-project-with-docker)
6. [Running the Project in Kubernetes](#running-the-project-in-kubernetes)
7. [Building and Pushing the MongoDB Docker Image](#building-and-pushing-the-mongodb-docker-image)
8. [Running Unit Tests](#running-unit-tests)
9. [Additional Configuration](#additional-configuration)
10. [Troubleshooting](#troubleshooting)

---

## 1. Prerequisites

Before you begin, ensure that you have the following tools installed on your system:

- **Git**: Version control system.
  - [Download Git](https://git-scm.com/downloads)

- **.NET 8.0 SDK**: Required for building and running the Poseidon API.
  - [Download .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

- **Docker**: Platform for developing, shipping, and running applications in containers.
  - [Download Docker](https://www.docker.com/products/docker-desktop)

- **Kubernetes** (optional for Kubernetes deployment):
  - **Minikube**: Local Kubernetes cluster.
    - [Minikube Installation Guide](https://minikube.sigs.k8s.io/docs/start/)
  - **Kubectl**: Command-line tool for interacting with Kubernetes clusters.
    - [Kubectl Installation Guide](https://kubernetes.io/docs/tasks/tools/install-kubectl/)

- **MongoDB** (if running locally):
  - [MongoDB Community Edition](https://www.mongodb.com/try/download/community)

- **IDE (Optional)**: Visual Studio, Visual Studio Code, or any preferred Integrated Development Environment for .NET development.
  - [Download Visual Studio](https://visualstudio.microsoft.com/downloads/)
  - [Download Visual Studio Code](https://code.visualstudio.com/Download)

---

## 2. Cloning the Repository

To get started, clone the Poseidon project repository to your local machine. You can use either HTTPS or SSH based on your preference and GitHub setup.

### Using HTTPS:

```bash
git clone https://github.com/cheloghm/Poseidon.git
```

### Using SSH:

```bash
git clone git@github.com:cheloghm/Poseidon.git
```

After cloning, navigate into the project directory:

```bash
cd Poseidon
```

---

## 3. Environment Setup

The Poseidon project utilizes `.env` files to manage sensitive configuration values such as database credentials and JWT keys. You will need to create two separate `.env` files:

1. **Root `.env` File**: Used for Docker and Kubernetes deployments.
2. **API Project `.env` File**: Used for local development (when running the project in an IDE).

### 3.1 Root `.env` File for Docker/Kubernetes

Create a `.env` file in the **root** directory of the project (`C:\Users\chelo\Poseidon\`) with the following content. This file is essential when deploying the project using Docker or Kubernetes.

**Root `.env` file:**

```plaintext
# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=rootUser
MONGO_INITDB_ROOT_PASSWORD=strongPassword
MONGO_DB_NAME=PoseidonDB

# Docker/Kubernetes MongoDB Connection String
MONGO_CONNECTION_STRING_K8S=mongodb://rootUser:strongPassword@poseidon-mongodb.default.svc.cluster.local:27017

# JWT Configuration
JWT_KEY=ThisIsASecretKeyWithAtLeast32Characters
JWT_ISSUER=PoseidonAPI
JWT_AUDIENCE=PoseidonClient
```

**Important Notes:**

- **Security**: Ensure that this `.env` file is **never** committed to version control. It should be included in `.gitignore` to prevent accidental exposure of sensitive information.
- **Customization**: Replace `rootUser`, `strongPassword`, and `ThisIsASecretKeyWithAtLeast32Characters` with your own secure credentials and secret keys.

### 3.2 API Project `.env` File for Local Development

Create another `.env` file inside the `Poseidon/` directory (`C:\Users\chelo\Poseidon\Poseidon\`) for local development. This configuration is used when running the project directly through an IDE like Visual Studio.

**Poseidon `.env` file (for local development):**

```plaintext
# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=rootUser
MONGO_INITDB_ROOT_PASSWORD=strongPassword
MONGO_DB_NAME=PoseidonDB

# Local Development MongoDB Connection String
MONGO_CONNECTION_STRING_LOCAL=mongodb://rootUser:strongPassword@localhost:27017/PoseidonDB?authSource=admin

# JWT Configuration
JWT_KEY=ThisIsASecretKeyWithAtLeast32Characters
JWT_ISSUER=PoseidonAPI
JWT_AUDIENCE=PoseidonClient
```

**Important Notes:**

- **Security**: Similarly, ensure that this `.env` file is **excluded** from version control.
- **Customization**: Adjust the connection string if your local MongoDB instance uses different credentials or runs on a different host/port.

---

## 4. Running the Project Locally (IDE like Visual Studio)

Running the Poseidon API locally through an IDE allows for easy debugging and development. Follow these steps to set up and run the project in Visual Studio or your preferred IDE.

### Step-by-Step Instructions

1. **Open the Project in Your IDE**

   - Launch Visual Studio or your chosen IDE.
   - Open the solution file located at `Poseidon.sln`.

2. **Restore Dependencies**

   - In the terminal integrated within your IDE or using the command prompt, navigate to the project root directory and run:
   
     ```bash
     dotnet restore
     ```

   - This command restores all necessary NuGet packages required by the project.

3. **Build the Project**

   - Ensure that the project builds without errors. You can do this by selecting **Build > Build Solution** in Visual Studio or using the command line:
   
     ```bash
     dotnet build
     ```

4. **Set Up Environment Variables**

   - Ensure that the `.env` file inside the `Poseidon/` directory is correctly configured as outlined in [Environment Setup](#environment-setup).

5. **Run the Project**

   - **Using Visual Studio:**
     - Press **F5** or click the **Run** button to start the application in debug mode.
   
   - **Using Terminal/Command Prompt:**
     ```bash
     dotnet run --project Poseidon/Poseidon.csproj
     ```

6. **Access the API**

   - Once the application is running, you can access the API documentation via Swagger UI at:
   
     ```
     http://localhost:8080/swagger
     ```

   - Alternatively, access the main application page at:
   
     ```
     http://localhost:8080/index.html
     ```

**Troubleshooting Tips:**

- **Port Conflicts**: If port `8080` is already in use, you may need to change the port in the `Program.cs` or relevant configuration files.
- **Environment Variables**: Ensure that all necessary environment variables are correctly set and loaded by the application.

---

## 5. Running the Project with Docker

Using Docker to run the Poseidon API ensures consistency across different environments and simplifies deployment. Follow these steps to containerize and run the project using Docker.

### Step-by-Step Instructions

1. **Navigate to the Project Root Directory**

   Ensure you are in the project root directory (`C:\Users\chelo\Poseidon\`).

   ```bash
   cd C:\Users\chelo\Poseidon
   ```

2. **Build Docker Images**

   Use Docker Compose to build the required Docker images for both the Poseidon API and MongoDB seeding.

   ```bash
   docker-compose build
   ```

   **Explanation:**
   - **Docker Compose** reads the `docker-compose.yml` file in the root directory to build images as defined.

3. **Start the Services**

   Launch the Poseidon API and MongoDB services using Docker Compose.

   ```bash
   docker-compose up -d
   ```

   **Explanation:**
   - The `-d` flag runs the containers in detached mode.

4. **Seed MongoDB with Titanic Dataset**

   To populate MongoDB with the initial Titanic dataset, execute the MongoDB seeding container.

   ```bash
   docker run --rm cheloghm/mongo-seed:latest
   ```

   **Explanation:**
   - This command runs the `mongo-seed` Docker image, which executes the `init-mongo.sh` script to seed the database.
   - The `--rm` flag removes the container after it exits.

5. **Verify Running Containers**

   Ensure that both Poseidon API and MongoDB containers are running:

   ```bash
   docker ps
   ```

   **Expected Output:**

   ```
   CONTAINER ID   IMAGE                          COMMAND                  CREATED          STATUS          PORTS                    NAMES
   abcdef123456   cheloghm/poseidon-api:latest   "dotnet Poseidon.Api…"   10 seconds ago   Up 9 seconds    0.0.0.0:8080->8080/tcp   poseidon_api_1
   fedcba654321   mongo:latest                   "docker-entrypoint.s…"   10 seconds ago   Up 9 seconds    27017/tcp                poseidon_mongodb_1
   ```

6. **Access the API**

   With Docker containers running, access the API at:

   ```
   http://localhost:8080/swagger
   ```

   Or the main application page:

   ```
   http://localhost:8080/index.html
   ```

7. **Stop the Containers**

   When you're done, stop and remove the containers:

   ```bash
   docker-compose down
   ```

   **Explanation:**
   - This command stops and removes all containers, networks, volumes, and images created by `docker-compose up`.

**Troubleshooting Tips:**

- **Docker Daemon**: Ensure that the Docker daemon is running. If not, start Docker Desktop or the Docker service on your machine.
- **Image Availability**: If you encounter issues building or running images, verify that the Dockerfiles and `docker-compose.yml` are correctly configured.

---

## 6. Running the Project in Kubernetes

Deploying the Poseidon API and MongoDB in a Kubernetes cluster provides scalability, reliability, and ease of management. This section guides you through deploying the project using Minikube for local Kubernetes testing.

### Step-by-Step Instructions

1. **Ensure Kubernetes is Running**

   - **Using Minikube:**
   
     Start your Minikube cluster if it's not already running:
     
     ```bash
     minikube start
     ```

     **Explanation:**
     - Initializes a local Kubernetes cluster using Minikube.

2. **Apply Kubernetes Configurations**

   Deploy MongoDB and Poseidon API to Kubernetes by applying the necessary YAML files.

   - **Deploy MongoDB:**
   
     ```bash
     kubectl apply -f k8s/Deployments/mongodb-deployment.yml
     kubectl apply -f k8s/Services/mongodb-service.yml
     ```

   - **Deploy Poseidon API:**
   
     ```bash
     kubectl apply -f k8s/Deployments/poseidon-deployment.yml
     kubectl apply -f k8s/Services/poseidon-service.yml
     ```

   **Explanation:**
   - **Deployments**: Define the desired state for your applications (MongoDB and Poseidon API).
   - **Services**: Expose the deployments internally or externally within the Kubernetes cluster.

3. **Verify Deployments and Services**

   Check the status of your pods and services to ensure they are running correctly.

   ```bash
   kubectl get pods
   kubectl get services
   ```

   **Expected Output:**

   ```
   NAME                                READY   STATUS    RESTARTS   AGE
   poseidon-api-5ddb4b6664-kmmgt       1/1     Running   0          2m
   poseidon-mongodb-7d9f75b77d-spbvc   1/1     Running   0          35h

   NAME               TYPE           CLUSTER-IP      EXTERNAL-IP   PORT(S)                      AGE
   kubernetes         ClusterIP      10.96.0.1       <none>        443/TCP                      2d22h
   poseidon-mongodb   ClusterIP      10.107.72.233   <none>        27017/TCP                    2d21h
   poseidon-service   LoadBalancer   10.101.22.203   <pending>     80:31344/TCP,443:31375/TCP   2d21h
   ```

4. **Expose the Poseidon Service**

   To access the Poseidon API externally, use Minikube's service tunneling feature:

   ```bash
   minikube service poseidon-service --url
   ```

   **Explanation:**
   - Retrieves the external URL for the `poseidon-service`.
   - You can access the API using the provided URL.

   **Example Output:**

   ```
   http://192.168.49.2:31344
   ```

   Access the API documentation at:

   ```
   http://192.168.49.2:31344/swagger
   ```

   Or the main application page:

   ```
   http://192.168.49.2:31344/index.html
   ```

5. **Monitor Pod Logs**

   To ensure the API is running correctly, monitor the logs of the Poseidon API pod:

   ```bash
   kubectl logs -f poseidon-api-5ddb4b6664-kmmgt
   ```

   **Explanation:**
   - The `-f` flag streams the logs in real-time.

**Troubleshooting Tips:**

- **Pod Crashing**: If pods are in a `CrashLoopBackOff` or `Error` state, check logs for errors:
  
  ```bash
  kubectl logs <pod-name>
  ```

- **Service Exposure Issues**: Ensure that the `poseidon-service.yml` correctly defines a `LoadBalancer` type service. If using Minikube, use the `minikube service` command as shown above.

---

## 7. Building and Pushing the MongoDB Docker Image

If you wish to create and push your own MongoDB seed image to Docker Hub (or another container registry), follow these steps. This allows for customization and sharing of the seed image.

### Step-by-Step Instructions

1. **Navigate to the Mongo-seed Directory**

   ```bash
   cd Mongo-seed
   ```

2. **Build the MongoDB Seed Image**

   Replace `your-dockerhub-username` with your actual Docker Hub username.

   ```bash
   docker build -t your-dockerhub-username/poseidon-mongo-seed:latest .
   ```

   **Explanation:**
   - The `-t` flag tags the image with the specified name and tag.
   - The `.` indicates the current directory contains the `Dockerfile`.

3. **Login to Docker Hub**

   Authenticate your Docker CLI with Docker Hub:

   ```bash
   docker login
   ```

   **Explanation:**
   - Enter your Docker Hub username and password when prompted.

4. **Push the Image to Docker Hub**

   ```bash
   docker push your-dockerhub-username/poseidon-mongo-seed:latest
   ```

   **Explanation:**
   - Uploads the tagged image to your Docker Hub repository.

5. **Use the MongoDB Seed Image in Kubernetes**

   - **Update Deployment File:**
     
     Open `k8s/Deployments/mongodb-deployment.yml` and update the image to use your Docker Hub image:

     ```yaml
     containers:
       - name: mongodb
         image: your-dockerhub-username/poseidon-mongo-seed:latest
         ports:
           - containerPort: 27017
     ```

   - **Apply the Updated Deployment:**

     ```bash
     kubectl apply -f k8s/Deployments/mongodb-deployment.yml
     ```

6. **Alternative: Use Pre-built Image**

   If you prefer not to build your own image, you can use the pre-built image:

   ```bash
   docker pull cheloghm/poseidon-mongo-seed:latest
   ```

   **Explanation:**
   - Downloads the pre-built MongoDB seed image from Docker Hub.

**Important Notes:**

- **Image Naming**: Ensure that your image names are unique to avoid conflicts in Docker Hub.
- **Security**: Never push images containing sensitive information or secrets. Always use environment variables and Kubernetes Secrets for managing sensitive data.

---

## 8. Running Unit Tests

Unit tests are crucial for ensuring the reliability and correctness of your application. The Poseidon API includes a test project to validate its functionality. Follow these steps to run the unit tests.

### Step-by-Step Instructions

1. **Navigate to the Project Root Directory**

   ```bash
   cd C:\Users\chelo\Poseidon
   ```

2. **Run the Tests Using the .NET CLI**

   Execute the following command to run all unit tests:

   ```bash
   dotnet test
   ```

   **Explanation:**
   - The `dotnet test` command builds the test project and runs all tests, providing a summary of the results.

3. **Review Test Results**

   After running the tests, review the output to ensure all tests pass. The output will indicate the number of tests passed, failed, or skipped.

   **Example Output:**

   ```
   Build started, please wait...
   Build completed.

   Test run for C:\Users\chelo\Poseidon\Poseidon.Tests\bin\Debug\net8.0\Poseidon.Tests.dll (.NETCoreApp,Version=v8.0)
   Microsoft (R) Test Execution Command Line Tool Version 17.5.0
   Copyright (c) Microsoft Corporation.  All rights reserved.

   Starting test execution, please wait...

   Passed!  - Failed: 0, Passed: 25, Skipped: 0, Total: 25
   ```

**Troubleshooting Tips:**

- **Test Failures**: If any tests fail, review the test output to identify the cause. Common issues include missing dependencies, incorrect configurations, or code errors.
- **Environment Variables**: Ensure that the `.env` files are correctly set up and that the test environment has access to necessary configurations.
- **Dependencies**: Verify that all project dependencies are restored and up-to-date by running `dotnet restore` before testing.

---

## 9. Additional Configuration

Beyond the basic setup and deployment steps, there are additional configurations and optimizations you can implement to enhance the Poseidon API Orchestrator's functionality and performance.

### 9.1 Configuring Kubernetes Secrets and ConfigMaps

For secure handling of sensitive data in Kubernetes, use **Secrets** and **ConfigMaps**.

- **ConfigMaps**: Store non-sensitive configuration data.
- **Secrets**: Store sensitive information like passwords and API keys.

**Example: Creating a Secret for JWT Key**

```bash
kubectl create secret generic jwt-secret \
  --from-literal=Jwt__Key=ThisIsASecretKeyWithAtLeast32Characters
```

**Explanation:**
- Creates a Kubernetes Secret named `jwt-secret` containing the JWT key.

**Refer to the [Deployment Guide](#deployment-guide) for detailed steps on configuring and applying Kubernetes resources.**

### 9.2 Setting Up Health Checks

Implementing health checks ensures that your application is running smoothly and can recover from failures.

**Example: Adding Health Probes in Deployment**

```yaml
readinessProbe:
  httpGet:
    path: /health/ready
    port: 8080
  initialDelaySeconds: 10
  periodSeconds: 10

livenessProbe:
  httpGet:
    path: /health/live
    port: 8080
  initialDelaySeconds: 15
  periodSeconds: 20
```

**Explanation:**
- **Readiness Probe**: Checks if the application is ready to accept traffic.
- **Liveness Probe**: Checks if the application is alive and running.

### 9.3 Implementing Logging and Monitoring

Integrate logging and monitoring tools to gain insights into application performance and troubleshoot issues effectively.

- **Logging**: Use tools like **Serilog** or **NLog** for structured logging.
- **Monitoring**: Integrate with **Prometheus** and **Grafana** for real-time monitoring and alerting.

**Example: Integrating Serilog for Logging**

1. **Install Serilog Packages**

   ```bash
   dotnet add package Serilog.AspNetCore
   dotnet add package Serilog.Sinks.Console
   dotnet add package Serilog.Sinks.File
   ```

2. **Configure Serilog in `Program.cs`**

   ```csharp
   using Serilog;

   var builder = WebApplication.CreateBuilder(args);

   // Configure Serilog
   Log.Logger = new LoggerConfiguration()
       .WriteTo.Console()
       .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
       .CreateLogger();

   builder.Host.UseSerilog();

   // ... rest of the setup
   ```

**Explanation:**
- Logs are written to both the console and rolling log files.

---

## 10. Troubleshooting

Deployment and setup processes can sometimes present challenges. Below are common issues and their resolutions to help you navigate and resolve potential problems effectively.

### 10.1 Common Issues and Solutions

1. **Port Conflicts**

   - **Symptom**: Unable to start the application or Docker containers due to port `8080` being in use.
   
   - **Solution**:
     - Identify the process using the port:
       
       ```bash
       # On Windows
       netstat -ano | findstr :8080
       
       # On macOS/Linux
       lsof -i :8080
       ```
     
     - Terminate the conflicting process or change the port in the application's configuration.

2. **Database Connection Failures**

   - **Symptom**: The application cannot connect to MongoDB.
   
   - **Solution**:
     - Ensure MongoDB is running.
     - Verify the connection string in the `.env` files is correct.
     - Check network configurations and firewall settings.
     - For Kubernetes deployments, ensure that the MongoDB service is correctly defined and accessible.

3. **Docker Build Failures**

   - **Symptom**: Errors occur while building Docker images.
   
   - **Solution**:
     - Check the `Dockerfile` for syntax errors.
     - Ensure all necessary files are present in the build context.
     - Verify that Docker has sufficient resources allocated.

4. **Kubernetes Deployment Issues**

   - **Symptom**: Pods are not starting or are crashing.
   
   - **Solution**:
     - Inspect pod logs for error messages:
       
       ```bash
       kubectl logs -f <pod-name>
       ```
     
     - Ensure that all environment variables are correctly set via ConfigMaps and Secrets.
     - Verify that the Docker images are correctly tagged and accessible.
     - Check resource allocations and adjust if necessary.

5. **Unit Test Failures**

   - **Symptom**: Some or all unit tests are failing.
   
   - **Solution**:
     - Review the test output to identify failing tests.
     - Ensure that the test environment is correctly configured with necessary dependencies.
     - Verify that recent code changes have not introduced breaking changes.

### 10.2 Steps to Diagnose Issues

1. **Check Service Status**

   ```bash
   # For Docker
   docker ps
   
   # For Kubernetes
   kubectl get pods
   kubectl get services
   ```

2. **Inspect Logs**

   - **Docker Logs:**
     
     ```bash
     docker logs <container-id>
     ```
   
   - **Kubernetes Pod Logs:**
     
     ```bash
     kubectl logs -f <pod-name>
     ```

3. **Describe Kubernetes Resources**

   ```bash
   kubectl describe pod <pod-name>
   kubectl describe service <service-name>
   ```

4. **Validate Configuration Files**

   - Ensure that all YAML files for Kubernetes are correctly formatted and reference the correct images and configurations.

   ```bash
   kubectl apply --dry-run=client -f k8s/Deployments/poseidon-deployment.yml
   ```

5. **Network Connectivity**

   - Test connectivity between services, especially between the Poseidon API and MongoDB.

   ```bash
   # From within the Poseidon API pod
   kubectl exec -it <poseidon-pod-name> -- bash
   # Then inside the pod
   ping poseidon-mongodb
   ```

---

## Conclusion

By following this **Detailed Installation Guide**, you should be able to set up and run the **Poseidon API Orchestrator** locally, within Docker containers, or in a Kubernetes environment with ease. This comprehensive guide ensures that developers of all experience levels can deploy the project effectively, troubleshoot common issues, and understand the necessary configurations.

For further assistance, refer to the following resources:

- **[API Documentation](./Docs/APIDocumentation.md)**: Detailed information about API endpoints, request/response structures, and usage examples.
- **[Database Schema](./Docs/DatabaseSchema.md)**: Overview of MongoDB collections, data models, and relationships.
- **[Deployment Guide](./Docs/DeploymentGuide.md)**: Instructions on deploying the API using Kubernetes, including setting up Minikube.
- **[Poseidon GitHub Repository](https://github.com/cheloghm/Poseidon)**: Access the source code, open issues, and contribute to the project.

Feel free to raise any issues on the GitHub repository if you encounter any problems during installation or deployment.

---
