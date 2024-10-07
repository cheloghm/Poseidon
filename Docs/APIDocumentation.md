# Poseidon API Orchestrator - API Documentation

Welcome to the **Poseidon API Orchestrator** API Documentation! This document provides a detailed overview of all available endpoints, including their HTTP methods, required parameters, request/response formats, and authentication mechanisms. Whether you're a developer integrating with the Poseidon API or a new contributor, this guide will help you navigate and utilize the API effectively.

---

## Table of Contents

- [Authentication](#authentication)
- [Passenger Endpoints](#passenger-endpoints)
  - [Get All Passengers](#get-all-passengers)
  - [Get Survivors](#get-survivors)
  - [Get Passengers by Class](#get-passengers-by-class)
  - [Get Passengers by Gender](#get-passengers-by-gender)
  - [Get Passengers by Age Range](#get-passengers-by-age-range)
  - [Get Passengers by Fare Range](#get-passengers-by-fare-range)
  - [Get Passenger Survival Rate](#get-passenger-survival-rate)
  - [Get Passenger by ID](#get-passenger-by-id)
  - [Create a Passenger](#create-a-passenger)
  - [Update a Passenger](#update-a-passenger)
  - [Delete a Passenger](#delete-a-passenger)
- [User Endpoints](#user-endpoints)
  - [Register a User](#register-a-user)
  - [Login a User](#login-a-user)
  - [Get User by ID](#get-user-by-id)
  - [Update a User](#update-a-user)
  - [Delete a User](#delete-a-user)
- [Error Handling](#error-handling)
- [Authentication and Authorization](#authentication-and-authorization)
- [Conclusion](#conclusion)

---

## Authentication

The Poseidon API uses **JWT (JSON Web Tokens)** for authentication and authorization. To access protected endpoints, clients must include a valid JWT token in the `Authorization` header of their HTTP requests.

### Obtaining a JWT Token

To obtain a JWT token, users must first **register** and then **login** using their credentials. Upon successful authentication, the API will return a JWT token, which should be used for subsequent requests to protected endpoints.

### Authorization Header Format

Include the JWT token in the `Authorization` header of your HTTP requests as follows:

```
Authorization: Bearer {your_token}
```

**Example:**

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR...
```

**Note:** Replace `{your_token}` with the actual token received from the login endpoint.

---

## Passenger Endpoints

The Passenger Endpoints allow you to perform various operations on passenger data, including retrieval, creation, updating, and deletion. Below are the detailed specifications for each endpoint.

### Get All Passengers

**Endpoint:** `GET /api/Passenger/all`

**Description:** Retrieves a paginated list of all passengers.

**Authentication:** **Required** (Any authenticated user)

**Query Parameters:**

- `page` (optional, integer): The page number to retrieve. Default is `1`.
- `pageSize` (optional, integer): Number of passengers per page. Default is `10`.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/all?page=1&pageSize=10" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalPassengers": 50,
  "passengers": [
    {
      "id": "60d5ec49f8d4b81234567890",
      "survived": true,
      "pclass": 1,
      "name": "John Doe",
      "sex": "male",
      "age": 35,
      "siblingsOrSpousesAboard": 1,
      "parentsOrChildrenAboard": 0,
      "fare": 100.0
    },
    // ... more passengers
  ]
}
```

---

### Get Survivors

**Endpoint:** `GET /api/Passenger/survivors`

**Description:** Retrieves a paginated list of passengers who survived.

**Authentication:** **Required** (Any authenticated user)

**Query Parameters:**

- `page` (optional, integer): The page number to retrieve. Default is `1`.
- `pageSize` (optional, integer): Number of passengers per page. Default is `10`.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/survivors?page=1&pageSize=10" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 3,
  "totalPassengers": 25,
  "passengers": [
    {
      "id": "60d5ec49f8d4b81234567891",
      "survived": true,
      "pclass": 1,
      "name": "Jane Smith",
      "sex": "female",
      "age": 28,
      "siblingsOrSpousesAboard": 0,
      "parentsOrChildrenAboard": 0,
      "fare": 200.0
    },
    // ... more passengers
  ]
}
```

---

### Get Passengers by Class

**Endpoint:** `GET /api/Passenger/class/{classNumber}`

**Description:** Retrieves a paginated list of passengers filtered by their travel class.

**Authentication:** **Required** (Any authenticated user)

**Path Parameters:**

- `classNumber` (required, integer): The class number (`1`, `2`, or `3`).

**Query Parameters:**

- `page` (optional, integer): The page number to retrieve. Default is `1`.
- `pageSize` (optional, integer): Number of passengers per page. Default is `10`.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/class/1?page=1&pageSize=10" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 2,
  "totalPassengers": 20,
  "passengers": [
    {
      "id": "60d5ec49f8d4b81234567892",
      "survived": false,
      "pclass": 1,
      "name": "John Smith",
      "sex": "male",
      "age": 45,
      "siblingsOrSpousesAboard": 0,
      "parentsOrChildrenAboard": 1,
      "fare": 150.0
    },
    // ... more passengers
  ]
}
```

---

### Get Passengers by Gender

**Endpoint:** `GET /api/Passenger/gender/{sex}`

**Description:** Retrieves a paginated list of passengers filtered by their gender.

**Authentication:** **Required** (Any authenticated user)

**Path Parameters:**

- `sex` (required, string): The gender of passengers to retrieve (`male` or `female`).

**Query Parameters:**

- `page` (optional, integer): The page number to retrieve. Default is `1`.
- `pageSize` (optional, integer): Number of passengers per page. Default is `10`.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/gender/female?page=1&pageSize=10" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 3,
  "totalPassengers": 30,
  "passengers": [
    {
      "id": "60d5ec49f8d4b81234567893",
      "survived": true,
      "pclass": 2,
      "name": "Mary Johnson",
      "sex": "female",
      "age": 30,
      "siblingsOrSpousesAboard": 1,
      "parentsOrChildrenAboard": 0,
      "fare": 75.0
    },
    // ... more passengers
  ]
}
```

---

### Get Passengers by Age Range

**Endpoint:** `GET /api/Passenger/age-range`

**Description:** Retrieves a paginated list of passengers within a specified age range.

**Authentication:** **Required** (Any authenticated user)

**Query Parameters:**

- `minAge` (required, number): The minimum age of passengers.
- `maxAge` (required, number): The maximum age of passengers.
- `page` (optional, integer): The page number to retrieve. Default is `1`.
- `pageSize` (optional, integer): Number of passengers per page. Default is `10`.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/age-range?minAge=20&maxAge=40&page=1&pageSize=10" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 2,
  "totalPassengers": 15,
  "passengers": [
    {
      "id": "60d5ec49f8d4b81234567894",
      "survived": true,
      "pclass": 1,
      "name": "Alice Brown",
      "sex": "female",
      "age": 28,
      "siblingsOrSpousesAboard": 0,
      "parentsOrChildrenAboard": 2,
      "fare": 180.0
    },
    // ... more passengers
  ]
}
```

---

### Get Passengers by Fare Range

**Endpoint:** `GET /api/Passenger/fare-range`

**Description:** Retrieves a paginated list of passengers within a specified fare range.

**Authentication:** **Required** (Any authenticated user)

**Query Parameters:**

- `minFare` (required, number): The minimum fare.
- `maxFare` (required, number): The maximum fare.
- `page` (optional, integer): The page number to retrieve. Default is `1`.
- `pageSize` (optional, integer): Number of passengers per page. Default is `10`.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/fare-range?minFare=50&maxFare=150&page=1&pageSize=10" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalPages": 2,
  "totalPassengers": 12,
  "passengers": [
    {
      "id": "60d5ec49f8d4b81234567895",
      "survived": true,
      "pclass": 1,
      "name": "Charlie White",
      "sex": "male",
      "age": 50,
      "siblingsOrSpousesAboard": 0,
      "parentsOrChildrenAboard": 0,
      "fare": 120.0
    },
    // ... more passengers
  ]
}
```

---

### Get Passenger Survival Rate

**Endpoint:** `GET /api/Passenger/survival-rate`

**Description:** Retrieves the overall survival rate of passengers as a percentage.

**Authentication:** **Required** (Any authenticated user)

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/survival-rate" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "survivalRate": 32.0
}
```

---

### Get Passenger by ID

**Endpoint:** `GET /api/Passenger/{id}`

**Description:** Retrieves detailed information about a specific passenger using their unique ID.

**Authentication:** **Required** (Any authenticated user)

**Path Parameters:**

- `id` (required, string): The unique identifier of the passenger.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/Passenger/60d5ec49f8d4b81234567890" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "id": "60d5ec49f8d4b81234567890",
  "survived": true,
  "pclass": 1,
  "name": "John Doe",
  "sex": "male",
  "age": 35,
  "siblingsOrSpousesAboard": 1,
  "parentsOrChildrenAboard": 0,
  "fare": 100.0
}
```

---

### Create a Passenger

**Endpoint:** `POST /api/Passenger`

**Description:** Creates a new passenger record. The `id` field is automatically generated by MongoDB and should not be included in the request body.

**Authentication:** **Required** (Only users with the **Admin** role)

**Request Body:**

| Field                       | Type    | Description                              |
|-----------------------------|---------|------------------------------------------|
| `survived`                  | Boolean | Indicates if the passenger survived.     |
| `pclass`                    | Integer | Passenger class (1, 2, or 3).            |
| `name`                      | String  | Full name of the passenger.              |
| `sex`                       | String  | Gender of the passenger (`male` or `female`). |
| `age`                       | Number  | Age of the passenger.                    |
| `siblingsOrSpousesAboard`   | Integer | Number of siblings or spouses aboard.    |
| `parentsOrChildrenAboard`   | Integer | Number of parents or children aboard.    |
| `fare`                      | Number  | Fare paid by the passenger.              |

**Request Example:**

```bash
curl -X POST "http://localhost:8080/api/Passenger" \
     -H "Authorization: Bearer {your_token}" \
     -H "Content-Type: application/json" \
     -d '{
           "survived": true,
           "pclass": 1,
           "name": "John Doe",
           "sex": "male",
           "age": 35,
           "siblingsOrSpousesAboard": 1,
           "parentsOrChildrenAboard": 0,
           "fare": 100.0
         }'
```

**Response Example:**

```json
{
  "id": "60d5ec49f8d4b81234567896",
  "survived": true,
  "pclass": 1,
  "name": "John Doe",
  "sex": "male",
  "age": 35,
  "siblingsOrSpousesAboard": 1,
  "parentsOrChildrenAboard": 0,
  "fare": 100.0
}
```

**Response Status Code:** `201 Created`

---

### Update a Passenger

**Endpoint:** `PUT /api/Passenger/{id}`

**Description:** Updates the details of an existing passenger. All fields except `id` can be updated.

**Authentication:** **Required** (Only users with the **Admin** role)

**Path Parameters:**

- `id` (required, string): The unique identifier of the passenger to update.

**Request Body:**

| Field                       | Type    | Description                              |
|-----------------------------|---------|------------------------------------------|
| `survived`                  | Boolean | Indicates if the passenger survived.     |
| `pclass`                    | Integer | Passenger class (1, 2, or 3).            |
| `name`                      | String  | Full name of the passenger.              |
| `sex`                       | String  | Gender of the passenger (`male` or `female`). |
| `age`                       | Number  | Age of the passenger.                    |
| `siblingsOrSpousesAboard`   | Integer | Number of siblings or spouses aboard.    |
| `parentsOrChildrenAboard`   | Integer | Number of parents or children aboard.    |
| `fare`                      | Number  | Fare paid by the passenger.              |

**Request Example:**

```bash
curl -X PUT "http://localhost:8080/api/Passenger/60d5ec49f8d4b81234567896" \
     -H "Authorization: Bearer {your_token}" \
     -H "Content-Type: application/json" \
     -d '{
           "survived": false,
           "pclass": 1,
           "name": "John Doe",
           "sex": "male",
           "age": 36,
           "siblingsOrSpousesAboard": 1,
           "parentsOrChildrenAboard": 1,
           "fare": 110.0
         }'
```

**Response Example:**

```json
{
  "id": "60d5ec49f8d4b81234567896",
  "survived": false,
  "pclass": 1,
  "name": "John Doe",
  "sex": "male",
  "age": 36,
  "siblingsOrSpousesAboard": 1,
  "parentsOrChildrenAboard": 1,
  "fare": 110.0
}
```

**Response Status Code:** `200 OK`

---

### Delete a Passenger

**Endpoint:** `DELETE /api/Passenger/{id}`

**Description:** Deletes a passenger record by their unique ID.

**Authentication:** **Required** (Only users with the **Admin** role)

**Path Parameters:**

- `id` (required, string): The unique identifier of the passenger to delete.

**Request Example:**

```bash
curl -X DELETE "http://localhost:8080/api/Passenger/60d5ec49f8d4b81234567896" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Status Code:** `204 No Content`

**No Response Body**

---

## User Endpoints

The User Endpoints handle user-related operations, including registration, authentication, retrieval, updating, and deletion of user accounts.

### Register a User

**Endpoint:** `POST /api/User/register`

**Description:** Registers a new user with either an **Admin** or **User** role. The `id` field is automatically generated by MongoDB and should not be included in the request body.

**Authentication:** **Not Required**

**Request Body:**

| Field     | Type    | Description                       |
|-----------|---------|-----------------------------------|
| `username`| String  | Unique username for the user.     |
| `email`   | String  | User's email address.             |
| `password`| String  | Secure password for the account.  |
| `role`    | String  | Role of the user (`Admin` or `User`). |

**Request Example:**

```bash
curl -X POST "http://localhost:8080/api/User/register" \
     -H "Content-Type: application/json" \
     -d '{
           "username": "johndoe",
           "email": "johndoe@example.com",
           "password": "SecurePassword123!",
           "role": "Admin"
         }'
```

**Response Example:**

```json
{
  "id": "60d5ec49f8d4b81234567897",
  "username": "johndoe",
  "email": "johndoe@example.com",
  "role": "Admin"
}
```

**Response Status Code:** `201 Created`

---

### Login a User

**Endpoint:** `POST /api/User/login`

**Description:** Authenticates a user and returns a JWT token for authorized access to protected endpoints.

**Authentication:** **Not Required**

**Request Body:**

| Field     | Type    | Description                      |
|-----------|---------|----------------------------------|
| `email`   | String  | User's registered email address. |
| `password`| String  | Password for the user account.    |

**Request Example:**

```bash
curl -X POST "http://localhost:8080/api/User/login" \
     -H "Content-Type: application/json" \
     -d '{
           "email": "johndoe@example.com",
           "password": "SecurePassword123!"
         }'
```

**Response Example:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-10-02T14:48:00Z"
}
```

**Response Status Code:** `200 OK`

---

### Get User by ID

**Endpoint:** `GET /api/User/{id}`

**Description:** Retrieves detailed information about a specific user using their unique ID.

**Authentication:** **Required** (Only users with the **Admin** role or the user themselves)

**Path Parameters:**

- `id` (required, string): The unique identifier of the user.

**Request Example:**

```bash
curl -X GET "http://localhost:8080/api/User/60d5ec49f8d4b81234567897" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Example:**

```json
{
  "id": "60d5ec49f8d4b81234567897",
  "username": "johndoe",
  "email": "johndoe@example.com",
  "role": "Admin"
}
```

**Response Status Code:** `200 OK`

---

### Update a User

**Endpoint:** `PUT /api/User/{id}`

**Description:** Updates the details of an existing user. Only the `username`, `email`, and `password` fields can be updated. The `role` field can only be updated by users with the **Admin** role.

**Authentication:** **Required** (Only users with the **Admin** role or the user themselves)

**Path Parameters:**

- `id` (required, string): The unique identifier of the user to update.

**Request Body:**

| Field     | Type    | Description                             |
|-----------|---------|-----------------------------------------|
| `username`| String  | Updated username (optional).            |
| `email`   | String  | Updated email address (optional).       |
| `password`| String  | New password (optional).                |
| `role`    | String  | Updated role (`Admin` or `User`, optional; only Admins can update this). |

**Request Example:**

```bash
curl -X PUT "http://localhost:8080/api/User/60d5ec49f8d4b81234567897" \
     -H "Authorization: Bearer {your_token}" \
     -H "Content-Type: application/json" \
     -d '{
           "username": "john_doe",
           "email": "john.doe@example.com",
           "password": "NewSecurePassword456!",
           "role": "User"
         }'
```

**Response Example:**

```json
{
  "id": "60d5ec49f8d4b81234567897",
  "username": "john_doe",
  "email": "john.doe@example.com",
  "role": "User"
}
```

**Response Status Code:** `200 OK`

---

### Delete a User

**Endpoint:** `DELETE /api/User/{id}`

**Description:** Deletes a user account by their unique ID.

**Authentication:** **Required** (Only users with the **Admin** role)

**Path Parameters:**

- `id` (required, string): The unique identifier of the user to delete.

**Request Example:**

```bash
curl -X DELETE "http://localhost:8080/api/User/60d5ec49f8d4b81234567897" \
     -H "Authorization: Bearer {your_token}" \
     -H "Accept: application/json"
```

**Response Status Code:** `204 No Content`

**No Response Body**

---

## Error Handling

The Poseidon API uses standard HTTP status codes to indicate the success or failure of an API request. Below are the common status codes you might encounter:

| Status Code | Meaning                       | Description                                           |
|-------------|-------------------------------|-------------------------------------------------------|
| `200 OK`    | Success                       | The request was successful.                          |
| `201 Created`| Resource Created             | A new resource has been successfully created.        |
| `204 No Content`| Successful but No Content | The request was successful but there's no content to return. |
| `400 Bad Request`| Invalid Request          | The server could not understand the request due to invalid syntax. |
| `401 Unauthorized`| Authentication Required | The request lacks valid authentication credentials.   |
| `403 Forbidden`| Access Denied              | The authenticated user does not have permission to access the resource. |
| `404 Not Found`| Resource Not Found         | The requested resource could not be found.            |
| `500 Internal Server Error`| Server Error  | The server encountered an unexpected condition.       |

### Error Response Structure

When an error occurs, the API will return a JSON response with details about the error.

**Example:**

```json
{
  "error": {
    "code": 400,
    "message": "Invalid request parameters.",
    "details": "The 'age' parameter must be a positive number."
  }
}
```

**Fields:**

- `code` (integer): HTTP status code.
- `message` (string): Brief description of the error.
- `details` (string, optional): Additional details about the error.

---

## Authentication and Authorization

### Authentication

Authentication is handled using JWT tokens. Users must first **register** and then **login** to receive a token. This token must be included in the `Authorization` header of all requests to protected endpoints.

### Authorization

The Poseidon API implements role-based access control (RBAC) to restrict access to certain endpoints based on user roles.

- **Admin Role:**
  - Can access all endpoints, including creating, updating, and deleting users and passengers.
  
- **User Role:**
  - Can access read-only endpoints, such as fetching passenger data.

**Note:** Ensure that the JWT token includes the user's role to enforce these access controls.

---

## Conclusion

This **API Documentation** provides a thorough overview of all available endpoints in the Poseidon API Orchestrator. By following the detailed descriptions, request and response examples, and authentication guidelines, you can effectively integrate and interact with the API.

For further assistance or questions, please refer to the [Installation Guide](./Docs/InstallationGuide.md) or reach out to the project maintainers via the [Poseidon GitHub Repository](https://github.com/cheloghm/Poseidon).

---

## Additional Steps and Notes

1. **Update Swagger Documentation:**
   - Ensure that all endpoints are properly documented in the Swagger UI. This can be accessed at `/swagger` once the API is running.
   - Use annotations and XML comments in your code to enhance Swagger documentation.

2. **Maintain Consistency:**
   - Keep the API Documentation updated with any changes to the endpoints, parameters, or authentication mechanisms.
   - Regularly review and test the endpoints to ensure documentation accuracy.

3. **Secure Sensitive Information:**
   - Do not expose sensitive information in error messages or responses.
   - Ensure that JWT tokens are securely generated and stored.

4. **Versioning:**
   - Consider implementing API versioning (e.g., `/api/v1/`) to manage changes and updates without disrupting existing integrations.

5. **Testing Endpoints:**
   - Utilize tools like Postman or Insomnia for testing API endpoints during development and integration.

6. **Feedback and Contributions:**
   - Encourage users and contributors to provide feedback on the documentation to improve clarity and comprehensiveness.

---
