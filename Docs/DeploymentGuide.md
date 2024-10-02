# Poseidon API - Deployment Guide

This deployment guide will walk you through the process of deploying the Poseidon API in various environments, including local deployment (for development), Docker containers, and Kubernetes. The guide assumes you already have the project set up by following the [Installation Guide](./InstallationGuide.md).

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

---

## 1. Prerequisites

Before starting deployment, make sure you have the following installed:

- **Git**: [Install Git](https://git-scm.com/downloads)
- **Docker**: [Install Docker](https://www.docker.com/products/docker-desktop)
- **.NET SDK**: [Install .NET SDK](https://dotnet.microsoft.com/download/dotnet)
- **Kubernetes**:
  - [Minikube](https://minikube.sigs.k8s.io/docs/start/)
  - [Kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)

Ensure that you have cloned the repository by running:
```bash
git clone https://github.com/cheloghm/Poseidon.git
```

---

## 2. Local Deployment

### Step-by-Step Local Deployment

For local development, you can run the Poseidon API directly in your IDE (like Visual Studio). Here's how you can do that:

1. **Open the project** in Visual Studio or your favorite IDE.
   
2. **Restore dependencies** by running:
   ```bash
   dotnet restore
   ```

3. **Run the API**:
   - In Visual Studio, click the **Run** button, or
   - Via terminal/command prompt:
     ```bash
     dotnet run --project Poseidon/Poseidon.csproj
     ```

4. The API will be available at:
   ```bash
   http://localhost:8080/index.html
   ```

Make sure to create the necessary `.env` file inside the `Poseidon` directory as explained in the [Installation Guide](./InstallationGuide.md).

---

## 3. Docker Deployment

### Build and Run Docker Containers

You can deploy the Poseidon API using Docker for containerized environments. Follow these steps:

1. **Build Docker Images**:
   ```bash
   docker-compose build
   ```

2. **Start the Services**:
   ```bash
   docker-compose up
   ```

3. The API will be running at:
   ```bash
   http://localhost:8080
   ```

To stop the containers, use:
```bash
docker-compose down
```

### Push Docker Images to Docker Hub

If you want to share or deploy the Docker images from Docker Hub, follow these steps to push the images to your Docker Hub account.

1. **Login to Docker Hub**:
   ```bash
   docker login
   ```

2. **Tag the Images**:
   ```bash
   docker tag poseidon-api your-dockerhub-username/poseidon-api:latest
   docker tag poseidon-mongo-seed your-dockerhub-username/poseidon-mongo-seed:latest
   ```

3. **Push the Images**:
   ```bash
   docker push your-dockerhub-username/poseidon-api:latest
   docker push your-dockerhub-username/poseidon-mongo-seed:latest
   ```

These images can now be used in your Kubernetes deployment or shared with others.

---

## 4. Kubernetes Deployment

### Configure and Apply Kubernetes Resources

To deploy Poseidon API and MongoDB using Kubernetes, follow these steps:

1. **Ensure Minikube is Running**:
   If using Minikube, start it by running:
   ```bash
   minikube start
   ```

2. **Apply MongoDB Deployment and Service**:
   First, deploy the MongoDB pod:
   ```bash
   kubectl apply -f Poseidon/k8s/Deployments/mongodb-deployment.yml
   kubectl apply -f Poseidon/k8s/Services/mongodb-service.yml
   ```

3. **Apply Poseidon API Deployment and Service**:
   Then, deploy the Poseidon API:
   ```bash
   kubectl apply -f Poseidon/k8s/Deployments/poseidon-deployment.yaml
   kubectl apply -f Poseidon/k8s/Services/poseidon-service.yaml
   ```

4. **Check the Status of the Pods**:
   Ensure that both services are running:
   ```bash
   kubectl get pods
   ```

5. **Expose the Poseidon Service**:
   If using Minikube, you can expose the service to access it:
   ```bash
   minikube service poseidon-service --url
   ```

   Use the URL provided to access the API.

---

### Accessing the Application

Once the deployment is successful, you can access the Poseidon API using the service URL provided by Minikube (for local Kubernetes deployments). For example:

```bash
http://<your-minikube-url>:8080/index.html
```

If you're using a cloud-based Kubernetes cluster (like AWS EKS or Google GKE), make sure you use the **LoadBalancer** service to expose the application externally.

---

## 5. Environment Variables

The Poseidon project uses two `.env` files for configuration. These contain sensitive data such as MongoDB credentials and JWT keys. You will need to create these `.env` files before deploying.

### Root `.env` File for Docker/Kubernetes

Create a `.env` file in the **root** directory (for Docker and Kubernetes):

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

### Poseidon API `.env` File for Local Development

Create a `.env` file inside the `Poseidon/` directory (for local development):

```plaintext
# MongoDB Configuration
MONGO_INITDB_ROOT_USERNAME=rootUser
MONGO_INITDB_ROOT_PASSWORD=strongPassword
MONGO_DB_NAME=PoseidonDB

# Local Development MongoDB Connection String
MONGO_CONNECTION_STRING_LOCAL=mongodb://rootUser:strongPassword@localhost:27017

# JWT Configuration
JWT_KEY=ThisIsASecretKeyWithAtLeast32Characters
JWT_ISSUER=PoseidonAPI
JWT_AUDIENCE=PoseidonClient
```

### Setting Up Environment Files for Kubernetes

When deploying to Kubernetes, the environment variables will be automatically picked up from the `.env` files and from Kubernetes ConfigMaps and Secrets.

---

## Conclusion

Now that you've followed the steps in this deployment guide, you should have the Poseidon API successfully deployed in your desired environment, whether locally, via Docker, or in Kubernetes.

If you have any issues or questions, feel free to reach out or open an issue in the GitHub repository.

For more details about the API endpoints and usage, check the [API Documentation](./Docs/APIDocumentation.md).
