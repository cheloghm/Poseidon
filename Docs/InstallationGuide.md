# Poseidon API - Detailed Installation Guide

Welcome to the Poseidon API installation guide. This guide will take you through every step to set up and run the Poseidon project locally, using Docker, or in a Kubernetes environment. It is designed for developers of all experience levels.

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

---

## 1. Prerequisites

Before you begin, ensure that you have the following tools installed on your system:

- **Git**: [Download Git](https://git-scm.com/downloads)
- **.NET 8.0 SDK**: [Download .NET](https://dotnet.microsoft.com/download/dotnet)
- **Docker**: [Download Docker](https://www.docker.com/products/docker-desktop)
- **Kubernetes** (optional for K8s deployment):
  - [Minikube](https://minikube.sigs.k8s.io/docs/start/)
  - [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
- **MongoDB** (if running locally): [MongoDB Community Edition](https://www.mongodb.com/try/download/community)

---

## 2. Cloning the Repository

To get started, you need to clone the Poseidon project repository to your local machine:

### Using HTTPS:
```bash
git clone https://github.com/cheloghm/Poseidon.git
```

### Using SSH:
```bash
git clone git@github.com:cheloghm/Poseidon.git
```

Then navigate into the project folder:
```bash
cd Poseidon
```

---

## 3. Environment Setup

The Poseidon project uses `.env` files to store sensitive configuration values such as database credentials and JWT keys. You will need to create two `.env` files as outlined below.

### Root `.env` File for Docker/Kubernetes

Create a `.env` file in the **root** directory of the project with the following content. This file is used when running the project in Docker or Kubernetes.

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

### API Project `.env` File for Local Development

Create another `.env` file inside the `Poseidon/` directory for local development (when running the project in Visual Studio or another IDE).

**Poseidon `.env` file (for local development):**

```plaintext
# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=rootUser
MONGO_INITDB_ROOT_PASSWORD=strongPassword
MONGO_DB_NAME=PoseidonDB

# Local Development MongoDB Connection String (used when running locally via Visual Studio)
MONGO_CONNECTION_STRING_LOCAL=mongodb://rootUser:strongPassword@localhost:27017

# JWT Configuration
JWT_KEY=ThisIsASecretKeyWithAtLeast32Characters
JWT_ISSUER=PoseidonAPI
JWT_AUDIENCE=PoseidonClient
```

Make sure the `.env` files are properly placed:
1. The **root** `.env` file for Docker and Kubernetes.
2. The `.env` file inside the `Poseidon/` folder for local development.

---

## 4. Running the Project Locally (IDE like Visual Studio)

Once you have the environment files set up, follow these steps to run the Poseidon API locally.

1. **Open the Project in Visual Studio** (or any preferred IDE).
2. **Restore Dependencies**: In the terminal or from the IDE's built-in terminal, run:
   ```bash
   dotnet restore
   ```

3. **Run the Project**: You can either:
   - Click the **Run** button in Visual Studio, or
   - Run the project via the command line:
     ```bash
     dotnet run --project Poseidon/Poseidon.csproj
     ```

4. The API will be available at: `http://localhost:8080/index.html`

---

## 5. Running the Project with Docker

To run the Poseidon API and MongoDB in Docker containers, follow these steps:

1. **Build the Docker Images**: This will build the images for both the API and the MongoDB seed.
   ```bash
   docker-compose build
   ```

2. **Start the Services**:
   ```bash
   docker-compose up
   ```

This will start both the Poseidon API and MongoDB container. You can access the API at:
```bash
http://localhost:8080
```

To stop the containers, press `Ctrl + C` or run:
```bash
docker-compose down
```

---

## 6. Running the Project in Kubernetes

To deploy the Poseidon API and MongoDB in a Kubernetes cluster:

1. **Ensure Kubernetes is Running**: Start your Minikube cluster (if using Minikube):
   ```bash
   minikube start
   ```

2. **Apply Kubernetes Configurations**: Apply the necessary deployment and service YAML files.

   For MongoDB deployment and service:
   ```bash
   kubectl apply -f Poseidon/k8s/Deployments/mongodb-deployment.yml
   kubectl apply -f Poseidon/k8s/Services/mongodb-service.yml
   ```

   For Poseidon API deployment and service:
   ```bash
   kubectl apply -f Poseidon/k8s/Deployments/poseidon-deployment.yaml
   kubectl apply -f Poseidon/k8s/Services/poseidon-service.yaml
   ```

3. **Check the Status of Pods**:
   ```bash
   kubectl get pods
   ```

4. **Access the Poseidon API**: Use Minikube to access the service:
   ```bash
   minikube service poseidon-service --url
   ```

The URL will be provided, and you can access the API from there.

---

## 7. Building and Pushing the MongoDB Docker Image

If you want to create and push your MongoDB seed image to Docker Hub (or use mine), follow these steps:

1. **Navigate to the Mongo-seed Directory**:
   ```bash
   cd Mongo-seed
   ```

2. **Build the MongoDB Seed Image**:
   ```bash
   docker build -t your-dockerhub-username/poseidon-mongo-seed:latest .
   ```

3. **Login to Docker Hub**:
   ```bash
   docker login
   ```

4. **Push the Image to Docker Hub**:
   ```bash
   docker push your-dockerhub-username/poseidon-mongo-seed:latest
   ```

5. **Use the MongoDB Seed Image in Kubernetes**:
   - If you want to use the MongoDB seed image, update the deployment file `Poseidon/k8s/Deployments/mongodb-deployment.yml` to include your image:
   ```yaml
   containers:
     - name: mongodb
       image: your-dockerhub-username/poseidon-mongo-seed:latest
       ports:
         - containerPort: 27017
   ```

If you do not want to create your own image, you can use the pre-built image:
```bash
docker pull cheloghm/poseidon-mongo-seed:latest
```

---

## 8. Running Unit Tests

Unit tests are provided in the `Poseidon.Tests` project. To run the tests:

1. Navigate to the root of the project:
   ```bash
   cd Poseidon
   ```

2. Run the tests using the .NET CLI:
   ```bash
   dotnet test
   ```

This will execute all the unit tests and display the results in the terminal.

---

## Conclusion

You now have the Poseidon API running locally, with Docker, or in a Kubernetes environment. You can also customize the MongoDB seed image as needed. For further API usage instructions and documentation, refer to the [API Documentation](./Docs/APIDocumentation.md).

Feel free to raise any issues on the GitHub repository if you encounter any problems!
