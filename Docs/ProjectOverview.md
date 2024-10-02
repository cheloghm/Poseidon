# Poseidon API - Project Overview

## Table of Contents
1. [Introduction](#introduction)
2. [Project Objective](#project-objective)
3. [Core Features](#core-features)
4. [Architecture Overview](#architecture-overview)
5. [Project Structure](#project-structure)
6. [Technologies Used](#technologies-used)
7. [APIs and Endpoints](#apis-and-endpoints)
8. [Authentication and Authorization](#authentication-and-authorization)
9. [Deployment Options](#deployment-options)

---

## 1. Introduction

The **Poseidon API** is a backend service designed to manage, process, and analyze passenger data, including survivors and other key details from the Titanic dataset. The service offers a robust RESTful API for handling CRUD operations on passengers, user authentication, and role-based access control. Additionally, it includes MongoDB as the primary data store for high performance and scalability.

This project is developed with scalability, security, and best practices in mind, making it suitable for a wide range of applications in both development and production environments.

---

## 2. Project Objective

The primary goal of the Poseidon API is to:

1. Manage a dataset of Titanic passengers.
2. Provide API endpoints for retrieving, updating, and analyzing passenger data.
3. Implement secure authentication and authorization using JWT tokens.
4. Serve as a base for more complex analytics and data services in the future.
5. Showcase best practices in backend development using .NET, Docker, Kubernetes, and MongoDB.

---

## 3. Core Features

The Poseidon API includes the following core features:

- **CRUD Operations for Passengers**: Retrieve, create, update, and delete passenger records from the Titanic dataset.
- **Data Analytics**: Calculate survival rates and other statistics based on passenger data.
- **Authentication & Authorization**: JWT-based authentication with role-based access control (Admin and User roles).
- **MongoDB Seeding**: Pre-seed the MongoDB database with Titanic passenger data using a custom seed script.
- **RESTful API**: Follows REST principles with a well-structured set of endpoints.
- **Pagination**: Paginate results for better performance on large datasets.
- **Logging and Monitoring**: Includes logging and monitoring support, suitable for production deployments.

---

## 4. Architecture Overview

The architecture of the Poseidon API project is structured around a clean separation of concerns. The key layers and components are:

1. **Controllers**: Responsible for handling HTTP requests and routing them to appropriate services.
2. **Services**: Implements the business logic of the application, coordinating between repositories and other utilities.
3. **Repositories**: Provides a data access layer, specifically interacting with MongoDB to handle CRUD operations.
4. **DTOs (Data Transfer Objects)**: Used to transfer data between layers of the application.
5. **Utilities**: Includes helper classes for tasks like JWT generation and password hashing.
6. **Event Handlers**: Implements a simple event-driven architecture for handling actions like passenger creation and updates.
7. **Middlewares**: Custom middlewares for exception handling and rate-limiting.

### Diagram (optional)

You may add an architecture diagram for clarity, showing the flow between controllers, services, repositories, and the database.

---

## 5. Project Structure

Below is an outline of the project structure for the Poseidon API:

```
Poseidon/
│
├── Mongo-seed/          # Directory for MongoDB seeding project
│   ├── Dockerfile
│   ├── init-mongo.sh    # MongoDB initialization and seeding script
│   └── titanic.csv      # Titanic dataset used for MongoDB seeding
│
├── Poseidon/            # Main Poseidon API project
│   ├── Controllers/     # API Controllers
│   ├── Services/        # Business logic and service layer
│   ├── Models/          # Data models
│   ├── Repositories/    # Data access layer (MongoDB interaction)
│   ├── DTOs/            # Data Transfer Objects
│   ├── EventHandlers/   # Event handling for business processes
│   └── ...              # Other directories (Config, Filters, Extensions, etc.)
│
├── Docs/                # Project documentation
│   ├── ProjectOverview.md
│   ├── InstallationGuide.md
│   ├── APIDocumentation.md
│   ├── CodeStyleGuide.md
│   └── DeploymentGuide.md
│
├── docker-compose.yml   # Docker Compose file for local development
├── Dockerfile           # Dockerfile for building Poseidon API image
└── Poseidon.sln         # Solution file for the project
```

---

## 6. Technologies Used

The Poseidon API leverages a variety of modern technologies to deliver a robust, scalable, and secure service. Below are the key technologies and frameworks used:

- **.NET 8.0**: The core framework used for building the API.
- **MongoDB**: NoSQL database for managing passenger data.
- **JWT (JSON Web Tokens)**: For secure authentication and role-based authorization.
- **Docker**: Containerization technology to package and deploy the application in any environment.
- **Kubernetes (Minikube)**: Orchestration platform to manage the containerized deployments.
- **Swagger**: API documentation and testing tool.
- **Serilog**: Logging framework for structured logs.
- **AutoMapper**: A library to map between DTOs and models.

---

## 7. APIs and Endpoints

The Poseidon API exposes a set of RESTful endpoints, which are organized around the following resources:

### **Passenger Endpoints:**
- `GET /api/Passenger/all`: Retrieve a list of passengers (paginated).
- `GET /api/Passenger/survivors`: Get a list of passengers who survived.
- `GET /api/Passenger/class/{classNumber}`: Get passengers by their class (1st, 2nd, 3rd).
- `GET /api/Passenger/gender/{sex}`: Get passengers by gender.
- `GET /api/Passenger/age-range`: Retrieve passengers within an age range.
- `GET /api/Passenger/fare-range`: Retrieve passengers based on ticket fare.
- `GET /api/Passenger/survival-rate`: Get the survival rate of all passengers.
- `POST /api/Passenger`: Create a new passenger (Admin only).
- `PUT /api/Passenger/{id}`: Update passenger details (Admin only).
- `DELETE /api/Passenger/{id}`: Delete a passenger record (Admin only).

### **User Authentication Endpoints:**
- `POST /api/User/register`: Register a new user with a role (Admin/User).
- `POST /api/User/login`: Login to receive a JWT token.
- `GET /api/User/{id}`: Get user details (Admin only).
- `PUT /api/User/{id}`: Update user details (Admin only).
- `DELETE /api/User/{id}`: Delete a user (Admin only).

For more detailed API usage, see the [API Documentation](./APIDocumentation.md).

---

## 8. Authentication and Authorization

The Poseidon API implements **JWT-based authentication** and role-based authorization. Upon successful login, users receive a JWT token, which must be included in the `Authorization` header for subsequent requests.

### Roles:
- **Admin**: Full access to all endpoints, including creating, updating, and deleting users and passengers.
- **User**: Read-only access to passenger data (can’t modify any records).

The JWT token is validated on every request, ensuring that only authenticated users with valid roles can access restricted endpoints.

---

## 9. Deployment Options

The Poseidon API can be deployed using several methods, including:

### **Local Development**:
Run the API directly in your IDE (e.g., Visual Studio) or via the .NET CLI. This mode is used for development and testing purposes.

### **Docker Deployment**:
Containerized deployment using Docker. You can build and run Docker containers locally or push them to a Docker registry (e.g., Docker Hub).

### **Kubernetes Deployment**:
For production-scale environments, the Poseidon API can be deployed in a Kubernetes cluster, with separate pods for the API and MongoDB database.

See the [Deployment Guide](./DeploymentGuide.md) for step-by-step instructions on how to deploy the Poseidon API.

---

## Conclusion

The Poseidon API is designed to be a flexible, scalable solution for handling passenger data from the Titanic dataset, with secure user authentication and easy-to-use APIs. Whether you're developing locally, deploying in a containerized environment, or managing the application on Kubernetes, this project serves as a strong foundation for further development and enhancements.

For more detailed instructions on setup, see the [Installation Guide](./InstallationGuide.md).
