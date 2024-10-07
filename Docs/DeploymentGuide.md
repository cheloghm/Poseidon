# Poseidon API Orchestrator - Deployment Guide

**Welcome to the Deployment Guide for the Poseidon API Orchestrator!** This guide will walk you through deploying the Poseidon API in various environments, including local development, Docker containers, and Kubernetes clusters. Whether you're a beginner or an experienced developer, this guide is designed to help you deploy seamlessly.

*Before proceeding, ensure you have completed the [Installation Guide](./Docs/InstallationGuide.md) to set up the project.*

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Local Deployment](#local-deployment)
3. [Docker Deployment](#docker-deployment)
    - [Build and Run Docker Containers](#build-and-run-docker-containers)
    - [Push Docker Images to Docker Hub](#push-docker-images-to-docker-hub)
4. [Kubernetes Deployment](#kubernetes-deployment)
    - [Configure and Apply Kubernetes Resources](#configure-and-apply-kubernetes-resources)
    - [Accessing the Application](#accessing-the-application)
5. [Environment Variables](#environment-variables)
6. [Troubleshooting](#troubleshooting)
7. [Additional Resources](#additional-resources)

---

## 1. Prerequisites

Before you begin deploying the Poseidon API Orchestrator, ensure you have the following tools and services installed and configured on your machine:

- **Git**: Version control system.
  - [Download Git](https://git-scm.com/downloads)

- **Docker**: Platform for developing, shipping, and running applications in containers.
  - [Download Docker](https://www.docker.com/products/docker-desktop)

- **.NET 8 SDK**: Required for building and running the Poseidon API.
  - [Download .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

- **Kubernetes**: Container orchestration system.
  - **Minikube**: Local Kubernetes cluster.
    - [Minikube Installation Guide](https://minikube.sigs.k8s.io/docs/start/)
  - **Kubectl**: Command-line tool for interacting with Kubernetes clusters.
    - [Kubectl Installation Guide](https://kubernetes.io/docs/tasks/tools/install-kubectl/)

- **MongoDB**: NoSQL database for data storage.
  - Can be run locally or via Docker.

- **GitHub Account**: For accessing GitHub Actions (optional but recommended for CI/CD).

---

## 2. Local Deployment

Local deployment is ideal for development and testing purposes. Running the Poseidon API directly on your machine allows you to interact with the API without containerization.

### Step-by-Step Local Deployment

1. **Clone the Repository**

   Begin by cloning the Poseidon repository to your local machine:

   ```bash
   git clone https://github.com/cheloghm/Poseidon.git
   cd Poseidon
   ```

2. **Navigate to the Poseidon API Project**

   ```bash
   cd Poseidon
   ```

3. **Restore Dependencies**

   Restore the necessary .NET dependencies:

   ```bash
   dotnet restore
   ```

4. **Set Up Environment Variables**

   Ensure the `.env` file is correctly set up with the required environment variables. Refer to the [Environment Variables](#environment-variables) section for details.

5. **Run the API**

   - **Using Visual Studio or Your Preferred IDE:**
     - Open the `Poseidon.sln` solution file.
     - Select the `Poseidon` project.
     - Press **F5** or click the **Run** button to start the application.

   - **Using Terminal/Command Prompt:**
     ```bash
     dotnet run --project Poseidon/Poseidon.csproj
     ```

6. **Access the API**

   Once the application is running, you can access the Swagger UI for API documentation at:

   ```
   http://localhost:8080/swagger
   ```

   Alternatively, access the main page:

   ```
   http://localhost:8080/index.html
   ```

---

## 3. Docker Deployment

Deploying Poseidon API using Docker ensures consistency across different environments and simplifies the deployment process.

### Build and Run Docker Containers

1. **Navigate to the Project Root Directory**

   ```bash
   cd C:\Users\chelo\Poseidon
   ```

2. **Build Docker Images**

   Use Docker Compose to build the required Docker images for Poseidon API and MongoDB seeding.

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

---

### Push Docker Images to Docker Hub

Pushing Docker images to Docker Hub allows you to share your images or use them in other environments.

1. **Login to Docker Hub**

   Authenticate your Docker CLI with Docker Hub:

   ```bash
   docker login
   ```

   **Explanation:**
   - Enter your Docker Hub username and password when prompted.

2. **Tag the Docker Images**

   Assign your Docker Hub username to the images:

   ```bash
   docker tag cheloghm/poseidon-api:latest your-dockerhub-username/poseidon-api:latest
   docker tag cheloghm/mongo-seed:latest your-dockerhub-username/mongo-seed:latest
   ```

   **Replace `your-dockerhub-username`** with your actual Docker Hub username.

3. **Push the Images to Docker Hub**

   Upload the tagged images to your Docker Hub repository:

   ```bash
   docker push your-dockerhub-username/poseidon-api:latest
   docker push your-dockerhub-username/mongo-seed:latest
   ```

   **Explanation:**
   - These commands push the images to the specified repositories on Docker Hub.

4. **Verify the Push**

   Log in to your Docker Hub account via the web interface to ensure the images are available.

---

## 4. Kubernetes Deployment

Deploying Poseidon API to a Kubernetes cluster ensures scalability, reliability, and ease of management. This guide assumes you are using **Minikube** for local Kubernetes deployments.

### Configure and Apply Kubernetes Resources

1. **Ensure Minikube is Running**

   Start Minikube if it's not already running:

   ```bash
   minikube start
   ```

   **Explanation:**
   - This command initializes a local Kubernetes cluster using Minikube.

2. **Apply Kubernetes Configurations**

   Deploy MongoDB and Poseidon API to Kubernetes by applying the necessary YAML files.

   - **Deploy MongoDB**

     ```bash
     kubectl apply -f k8s/Deployments/mongodb-deployment.yml
     kubectl apply -f k8s/Services/mongodb-service.yml
     ```

   - **Deploy Poseidon API**

     ```bash
     kubectl apply -f k8s/Deployments/poseidon-deployment.yml
     kubectl apply -f k8s/Services/poseidon-service.yml
     ```

   **Explanation:**
   - **Deployments**: Define the desired state for your applications (MongoDB and Poseidon API).
   - **Services**: Expose the deployments internally or externally.

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
   - This command retrieves the external URL for the `poseidon-service`.
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

---

### Accessing the Application

Once deployed, you can interact with the Poseidon API through the exposed service URL. Here's how:

1. **Swagger UI**

   Access the Swagger UI to explore and test API endpoints:

   ```
   http://<minikube-ip>:<port>/swagger
   ```

   **Example:**

   ```
   http://192.168.49.2:31344/swagger
   ```

2. **Main Application Page**

   Access the main application interface:

   ```
   http://<minikube-ip>:<port>/index.html
   ```

   **Example:**

   ```
   http://192.168.49.2:31344/index.html
   ```

3. **API Endpoints**

   Use tools like `curl`, Postman, or your preferred HTTP client to interact with the API.

   - **Register a User**

     ```bash
     curl -X 'POST' \
       'http://192.168.49.2:31344/api/User/register' \
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
       'http://192.168.49.2:31344/api/User/login' \
       -H 'accept: */*' \
       -H 'Content-Type: application/json' \
       -d '{
       "username": "johndoe",
       "password": "SecurePassword123!"
     }'
     ```

   - **List All Passengers**

     ```bash
     curl -X 'GET' \
       'http://192.168.49.2:31344/api/Passenger/all' \
       -H 'accept: */*'
     ```

   - **Create a New Passenger**

     ```bash
     curl -X 'POST' \
       'http://192.168.49.2:31344/api/Passenger/create' \
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

---

## 5. Environment Variables

Environment variables are crucial for configuring the Poseidon API, especially for sensitive information like database credentials and JWT keys. Properly setting up environment variables ensures secure and flexible deployments across different environments.

### Environment Variable Setup

The Poseidon project utilizes two primary `.env` files:

1. **Root `.env` File**: Used for Docker and Kubernetes deployments.
2. **Poseidon API `.env` File**: Used for local development.

### Root `.env` File for Docker/Kubernetes

Create a `.env` file in the **root** directory (`C:\Users\chelo\Poseidon\`) with the following content:

```plaintext
# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=rootUser
MONGO_INITDB_ROOT_PASSWORD=strongPassword
MONGO_DB_NAME=PoseidonDB

# Kubernetes MongoDB Connection String
DATABASE_CONFIG_CONNECTIONSTRING=mongodb://rootUser:strongPassword@poseidon-mongodb.default.svc.cluster.local:27017/PoseidonDB?authSource=admin

# JWT Configuration
Jwt__Key=ThisIsASecretKeyWithAtLeast32Characters
Jwt__Issuer=PoseidonAPI
Jwt__Audience=PoseidonClient
```

**Explanation:**

- **MongoDB Configuration:**
  - `MONGO_INITDB_ROOT_USERNAME`: MongoDB root username.
  - `MONGO_INITDB_ROOT_PASSWORD`: MongoDB root password.
  - `MONGO_DB_NAME`: Name of the MongoDB database.

- **Kubernetes MongoDB Connection String:**
  - `DATABASE_CONFIG_CONNECTIONSTRING`: Connection string used by the Poseidon API to connect to MongoDB within Kubernetes.

- **JWT Configuration:**
  - `Jwt__Key`: Secret key for JWT token generation.
  - `Jwt__Issuer`: JWT token issuer.
  - `Jwt__Audience`: JWT token audience.

### Poseidon API `.env` File for Local Development

Create a `.env` file inside the `Poseidon/` directory (`C:\Users\chelo\Poseidon\Poseidon\`) with the following content:

```plaintext
# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=rootUser
MONGO_INITDB_ROOT_PASSWORD=strongPassword
MONGO_DB_NAME=PoseidonDB

# Local Development MongoDB Connection String
DatabaseConfig__ConnectionString=mongodb://rootUser:strongPassword@localhost:27017/PoseidonDB?authSource=admin

# JWT Configuration
Jwt__Key=ThisIsASecretKeyWithAtLeast32Characters
Jwt__Issuer=PoseidonAPI
Jwt__Audience=PoseidonClient
```

**Explanation:**

- **MongoDB Configuration:** Same as the root `.env` file.
- **Local Development MongoDB Connection String:**
  - `DatabaseConfig__ConnectionString`: Connection string for Poseidon API to connect to a locally running MongoDB instance.
  
- **JWT Configuration:** Same as the root `.env` file.

### Setting Up Environment Files for Kubernetes

When deploying to Kubernetes, environment variables are managed via **ConfigMaps** and **Secrets**. The provided Kubernetes YAML files (`k8s/k8s-configs/configmap.yml` and `k8s/k8s-configs/secret.yml`) handle this configuration.

**Note:** Ensure that the `secret.yml` contains all sensitive environment variables, while `configmap.yml` contains non-sensitive configurations.

---

## 6. Troubleshooting

Deployment can sometimes present challenges. Below are common issues and their resolutions:

### Common Issues and Solutions

1. **Pod Crashing or Not Starting**

   - **Symptom:** The Poseidon API pod is in a `CrashLoopBackOff` or `Error` state.
   
   - **Solution:**
     - **Check Logs:** Retrieve pod logs to identify errors.
       ```bash
       kubectl logs -f <pod-name>
       ```
     - **Environment Variables:** Ensure all required environment variables are correctly set via ConfigMaps and Secrets.
     - **Database Connectivity:** Verify that MongoDB is running and accessible from the Poseidon API pod.
     - **Image Pull Issues:** Ensure that the Docker images are correctly tagged and accessible.

2. **Cannot Connect to MongoDB**

   - **Symptom:** Application logs indicate inability to connect to MongoDB.
   
   - **Solution:**
     - **Connection String:** Verify that the `DATABASE_CONFIG_CONNECTIONSTRING` is correctly set and points to the MongoDB service.
     - **MongoDB Service:** Ensure that the MongoDB service is running and accessible within the Kubernetes cluster.
     - **Network Policies:** Check if any network policies are preventing communication between pods.

3. **Port Conflicts**

   - **Symptom:** Services fail to start due to port conflicts, especially when running Docker Compose alongside Kubernetes.
   
   - **Solution:**
     - **Terminate Conflicting Services:** Stop Docker Compose services if they are not needed.
       ```bash
       docker-compose down
       ```
     - **Modify Host Port Mappings:** Change host ports in `docker-compose.yml` to avoid conflicts.

4. **Health Probes Failing**

   - **Symptom:** Kubernetes readiness and liveness probes are failing, causing pods to restart or remain unready.
   
   - **Solution:**
     - **Adjust Probe Configurations:** Modify `initialDelaySeconds`, `timeoutSeconds`, and `periodSeconds` in `poseidon-deployment.yml` based on application startup time.
     - **Endpoint Availability:** Ensure that the health check endpoints (`/health/live` and `/health/ready`) are correctly implemented and accessible.

5. **Unauthorized Access Errors**

   - **Symptom:** API endpoints return `401 Unauthorized` or `403 Forbidden`.
   
   - **Solution:**
     - **JWT Configuration:** Ensure that `Jwt__Key`, `Jwt__Issuer`, and `Jwt__Audience` are correctly set and match between the API and client requests.
     - **Token Validity:** Verify that JWT tokens are being correctly generated and included in request headers.

### Steps to Diagnose Issues

1. **Check Pod Status**

   ```bash
   kubectl get pods
   ```

2. **Inspect Pod Logs**

   ```bash
   kubectl logs -f <pod-name>
   ```

3. **Describe Pods for Detailed Information**

   ```bash
   kubectl describe pod <pod-name>
   ```

4. **Verify Services and Endpoints**

   ```bash
   kubectl get services
   kubectl get endpoints
   ```

5. **Check ConfigMaps and Secrets**

   - **ConfigMaps:**
     ```bash
     kubectl get configmap
     kubectl describe configmap <configmap-name>
     ```
   
   - **Secrets:**
     ```bash
     kubectl get secrets
     kubectl describe secret <secret-name>
     ```

6. **Validate Kubernetes Resources**

   Use `kubectl` to validate the applied YAML files and ensure they are correctly configured.

   ```bash
   kubectl apply --dry-run=client -f k8s/Deployments/poseidon-deployment.yml
   ```

---

## 7. Additional Resources

For further assistance and detailed information, refer to the following resources:

- **[Installation Guide](./Docs/InstallationGuide.md)**: Comprehensive instructions to set up and run the project locally and via Docker.
- **[API Documentation](./Docs/APIDocumentation.md)**: Detailed information about API endpoints, request/response structures, and usage examples.
- **[Database Schema](./Docs/DatabaseSchema.md)**: Overview of MongoDB collections, data models, and relationships.
- **[Security Guide](./Docs/SecurityGuide.md)**: Best practices and security measures implemented in the project.
- **[Testing Guide](./Docs/TestingGuide.md)**: Strategies and instructions for testing the Poseidon API.
- **[Contribution Guide](./Docs/ContributionGuide.md)**: Guidelines for contributing to the project, including coding standards and pull request processes.
- **[Deployment Guide](#)**: (This current document)
- **[Poseidon GitHub Repository](https://github.com/cheloghm/Poseidon)**: Access the source code, open issues, and contribute to the project.

---

# Conclusion

Deploying the **Poseidon API Orchestrator** across different environments enhances its scalability, reliability, and maintainability. Whether you're running it locally for development, containerizing it with Docker for consistency, or orchestrating it with Kubernetes for production-grade deployments, this guide provides the necessary steps to achieve a successful deployment.

*If you encounter any issues not covered in this guide, please refer to the [Troubleshooting](#troubleshooting) section or reach out through the project's [GitHub Issues](https://github.com/cheloghm/Poseidon/issues).*

---

# License

This project is licensed under the [MIT License](LICENSE.txt). You are free to use, modify, and distribute this project as per the license terms.

---

# Acknowledgements

- **.NET Community**: For providing a robust framework for building APIs.
- **MongoDB**: For offering a flexible and scalable NoSQL database solution.
- **Docker & Kubernetes**: For enabling seamless containerization and orchestration.
- **GitHub Actions**: For facilitating efficient CI/CD workflows.
- **Trivy**: For ensuring Docker images are secure through vulnerability scanning.
- **Contributors**: Special thanks to all the contributors who have helped make this project possible.

---

# Additional Steps and Notes

1. **Ensure Accurate File Paths**

   All directory paths in this guide have been updated to match your current project structure. Double-check the paths in your Kubernetes YAML files and Docker Compose configurations to ensure they align with the file locations.

2. **Replace Placeholder Values**

   - **Docker Hub Username**: Replace `your-dockerhub-username` with your actual Docker Hub username in the Docker tagging and pushing commands.
   - **Minikube IP and Ports**: When accessing services via Minikube, replace `<minikube-ip>` and `<port>` with the actual values obtained from the `minikube service` command.

3. **Secure Your Environment Variables**

   Avoid committing sensitive information like passwords and secret keys to version control. Use Kubernetes Secrets and GitHub Secrets for secure storage.

4. **Maintain Documentation**

   As your project evolves, keep the **Docs** folder updated with the latest information to ensure all guides and documentation remain accurate and helpful.

5. **Test Deployment Processes**

   Regularly test your deployment processes in different environments to identify and resolve potential issues early.

6. **Monitor and Log**

   Implement monitoring and logging solutions to gain insights into the application's performance and quickly identify issues.

---
