### **Poseidon API Orchestrator** - README.md

---

## Poseidon API Orchestrator

**Poseidon API Orchestrator** is a RESTful web API built using **.NET 8** and **MongoDB**. The API demonstrates handling complex backend engineering tasks, including user authentication, data storage, and deployment in a containerized environment using **Docker** and **Kubernetes**.

### **Key Features**

- **RESTful API**: Built using **.NET 8**.
- **MongoDB**: Data storage using a NoSQL database.
- **JWT-based Authentication**: For securing user data.
- **Swagger Documentation**: Accessible at `/swagger`.
- **MongoDB Seeding**: Titanic dataset is seeded via a custom Docker solution.
- **CI/CD Pipelines**: Implemented using **GitHub Actions**.
- **Kubernetes Deployment**: Container orchestration with **Minikube** for local testing.
- **Security**: Integrated **Trivy** security scanning for Docker images.
  
---

## **Project Structure**

```plaintext
Poseidon/
│
├── Mongo-seed/                  # MongoDB seeding project
│   ├── Dockerfile               # Dockerfile for MongoDB seeding
│   ├── init-mongo.sh            # MongoDB initialization and seeding script
│   └── titanic.csv              # Titanic dataset used for seeding
│
├── Poseidon/                    # Main Poseidon API project
│   ├── Controllers/             # API controllers (e.g., UserController, PassengerController)
│   ├── Services/                # Business logic for handling requests
│   ├── Models/                  # Data models for MongoDB
│   ├── Repositories/            # Data access layer for MongoDB
│   ├── DTOs/                    # Data transfer objects for incoming/outgoing data
│   ├── Config/                  # Configuration files for JWT, MongoDB, etc.
│   ├── Filters/                 # Request processing filters (e.g., logging, validation)
│   ├── Utilities/               # Helper classes (e.g., JWT utilities)
│   ├── k8s/                     # Kubernetes manifests for deploying the API and MongoDB
│   ├── k8s-configs/             # Kubernetes ConfigMaps and Secrets
│   ├── CICD/                    # CI/CD pipeline configurations
│   ├── BackgroudTasks/          # Background tasks (e.g., token cleanup)
│   ├── Middlewares/             # Custom middleware (e.g., rate limiting)
│   ├── Security/                # Security configurations (e.g., Trivy config)
│   └── Program.cs               # Entry point of the application
│
├── Docs/                        # Documentation files
│   ├── ProjectOverview.md       # Overview of the project
│   ├── InstallationGuide.md     # Instructions to set up the project
│   ├── APIDocumentation.md      # API documentation (endpoints, methods, etc.)
│   ├── DatabaseSchema.md        # MongoDB collections schema
│   ├── DeploymentGuide.md       # Kubernetes deployment guide
│   ├── TestingGuide.md          # Information about testing strategies
│   ├── SecurityGuide.md         # Security configurations and best practices
│
├── docker-compose.yml           # Docker Compose file for local development
├── Dockerfile                   # Dockerfile for Poseidon API
├── Poseidon.sln                 # Solution file for the Poseidon project
└── .env                         # Environment variables for local development
```

---

## **Getting Started**

Follow the instructions below to set up and run the Poseidon API Orchestrator on your local machine using **Docker** or **Kubernetes (Minikube)**.

---

### **Running with Docker Compose**

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/cheloghm/Poseidon.git
   cd Poseidon
   ```

2. **Create `.env` Files**:
   - **Root `.env` (used for Docker/Kubernetes)**:
     ```plaintext
     # MongoDB Configuration
     MONGO_INITDB_ROOT_USERNAME=rootUser
     MONGO_INITDB_ROOT_PASSWORD=strongPassword
     MONGO_DB_NAME=PoseidonDB

     # Docker/Kubernetes MongoDB Connection
     MONGO_CONNECTION_STRING_K8S=mongodb://rootUser:strongPassword@poseidon-mongodb.default.svc.cluster.local:27017

     # JWT Configuration
     JWT_KEY=ThisIsASecretKeyWithAtLeast32Characters
     JWT_ISSUER=PoseidonAPI
     JWT_AUDIENCE=PoseidonClient
     ```

   - **Poseidon API `.env` (for IDE/local development)**:
     ```plaintext
     # Local MongoDB Connection
     MONGO_CONNECTION_STRING_LOCAL=mongodb://rootUser:strongPassword@localhost:27017
     
     # JWT Configuration
     JWT_KEY=ThisIsASecretKeyWithAtLeast32Characters
     JWT_ISSUER=PoseidonAPI
     JWT_AUDIENCE=PoseidonClient
     ```

3. **Run Docker Compose**:
   ```bash
   docker-compose up --build
   ```

4. **Access the API**:
   - Swagger UI: [http://localhost:8080/swagger](http://localhost:8080/swagger)
   - MongoDB: Accessible at `mongodb://localhost:27017` (default credentials: rootUser/strongPassword).

---

### **Running with Kubernetes (Minikube)**

1. **Set Up Minikube**:
   Ensure that **Minikube** is installed and running. If not, you can install it [here](https://minikube.sigs.k8s.io/docs/start/).

   Start Minikube:
   ```bash
   minikube start
   ```

2. **Deploy MongoDB and Poseidon API to Kubernetes**:

   - Apply MongoDB Deployment and Service:
     ```bash
     kubectl apply -f Poseidon/k8s/Deployments/mongodb-deployment.yml
     kubectl apply -f Poseidon/k8s/Services/mongodb-service.yml
     ```

   - Apply Poseidon API Deployment and Service:
     ```bash
     kubectl apply -f Poseidon/k8s/Deployments/poseidon-deployment.yml
     kubectl apply -f Poseidon/k8s/Services/poseidon-service.yml
     ```

3. **Access Poseidon API**:
   Get the Poseidon service URL:
   ```bash
   minikube service poseidon-service --url
   ```

   Navigate to the provided URL and access the Swagger UI.

---

## **API Usage**

### **User Authentication**

- **Register a User**: `POST /api/User/register`
- **Login**: `POST /api/User/login`

### **Passenger Data Management**

- **Get All Passengers**: `GET /api/Passenger/all`
- **Get Passengers by Class**: `GET /api/Passenger/class/{classNumber}`
  
For more API details, visit the [API Documentation](Docs/APIDocumentation.md).

---

## **Contributing**

Contributions are welcome! Please refer to the [Contribution Guide](Docs/CONTRIBUTING.md) for guidelines.

---

## **License**

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) for details.
