# Poseidon API - API Documentation

This document provides a comprehensive overview of the endpoints available in the Poseidon API. Each endpoint is listed with its HTTP method, expected parameters, and sample responses. You will also find guidance on authentication and access control.

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

---

## Authentication

Poseidon API uses JWT-based authentication. Each endpoint that requires authentication will need an Authorization header containing a valid JWT token. The format for this header is:

```
Authorization: Bearer {your_token}
```

When registering or logging in, you will receive a token in the response, which you should use for all authenticated endpoints.

## Passenger Endpoints

### Get All Passengers

**Endpoint**: `GET /api/Passenger/all`

**Description**: Fetches all passengers in a paginated format.

**Parameters**:
- `page` (optional, integer): The page number to retrieve. Default is 1.
- `pageSize` (optional, integer): Number of passengers per page. Default is 10.

**Response Example**:
```json
[
  {
    "id": "string",
    "survived": true,
    "pclass": 1,
    "name": "John Doe",
    "sex": "male",
    "age": 35,
    "siblingsOrSpousesAboard": 1,
    "parentsOrChildrenAboard": 0,
    "fare": 100.0
  }
]
```

### Get Survivors

**Endpoint**: `GET /api/Passenger/survivors`

**Description**: Fetches only the passengers who survived.

**Parameters**:
- `page` (optional, integer): The page number to retrieve. Default is 1.
- `pageSize` (optional, integer): Number of passengers per page. Default is 10.

**Response Example**:
```json
[
  {
    "id": "string",
    "survived": true,
    "pclass": 1,
    "name": "Jane Doe",
    "sex": "female",
    "age": 28,
    "siblingsOrSpousesAboard": 0,
    "parentsOrChildrenAboard": 0,
    "fare": 200.0
  }
]
```

### Get Passengers by Class

**Endpoint**: `GET /api/Passenger/class/{classNumber}`

**Description**: Fetches passengers filtered by their travel class.

**Parameters**:
- `classNumber` (required, integer): The class number (1, 2, or 3).
- `page` (optional, integer): The page number to retrieve. Default is 1.
- `pageSize` (optional, integer): Number of passengers per page. Default is 10.

**Response Example**:
```json
[
  {
    "id": "string",
    "survived": false,
    "pclass": 3,
    "name": "John Smith",
    "sex": "male",
    "age": 45,
    "siblingsOrSpousesAboard": 0,
    "parentsOrChildrenAboard": 1,
    "fare": 50.0
  }
]
```

### Get Passengers by Gender

**Endpoint**: `GET /api/Passenger/gender/{sex}`

**Description**: Fetches passengers filtered by their gender.

**Parameters**:
- `sex` (required, string): The gender ("male" or "female").
- `page` (optional, integer): The page number to retrieve. Default is 1.
- `pageSize` (optional, integer): Number of passengers per page. Default is 10.

**Response Example**:
```json
[
  {
    "id": "string",
    "survived": true,
    "pclass": 2,
    "name": "Mary Johnson",
    "sex": "female",
    "age": 30,
    "siblingsOrSpousesAboard": 1,
    "parentsOrChildrenAboard": 0,
    "fare": 75.0
  }
]
```

### Get Passengers by Age Range

**Endpoint**: `GET /api/Passenger/age-range`

**Description**: Fetches passengers filtered by an age range.

**Parameters**:
- `minAge` (required, number): Minimum age of the passengers.
- `maxAge` (required, number): Maximum age of the passengers.
- `page` (optional, integer): The page number to retrieve. Default is 1.
- `pageSize` (optional, integer): Number of passengers per page. Default is 10.

**Response Example**:
```json
[
  {
    "id": "string",
    "survived": true,
    "pclass": 1,
    "name": "Alice Brown",
    "sex": "female",
    "age": 5,
    "siblingsOrSpousesAboard": 0,
    "parentsOrChildrenAboard": 2,
    "fare": 300.0
  }
]
```

### Get Passengers by Fare Range

**Endpoint**: `GET /api/Passenger/fare-range`

**Description**: Fetches passengers filtered by the fare they paid.

**Parameters**:
- `minFare` (required, number): Minimum fare.
- `maxFare` (required, number): Maximum fare.
- `page` (optional, integer): The page number to retrieve. Default is 1.
- `pageSize` (optional, integer): Number of passengers per page. Default is 10.

**Response Example**:
```json
[
  {
    "id": "string",
    "survived": true,
    "pclass": 1,
    "name": "Charlie White",
    "sex": "male",
    "age": 50,
    "siblingsOrSpousesAboard": 0,
    "parentsOrChildrenAboard": 0,
    "fare": 150.0
  }
]
```

### Get Passenger Survival Rate

**Endpoint**: `GET /api/Passenger/survival-rate`

**Description**: Returns the survival rate of passengers as a percentage.

**Response Example**:
```json
{
  "survivalRate": 32.0
}
```

### Get Passenger by ID

**Endpoint**: `GET /api/Passenger/{id}`

**Description**: Fetches a specific passenger by their ID.

**Parameters**:
- `id` (required, string): The ID of the passenger.

**Response Example**:
```json
{
  "id": "string",
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

### Create a Passenger

**Endpoint**: `POST /api/Passenger`

**Description**: Creates a new passenger. The ID will be generated automatically by MongoDB, so leave it blank in the request body.

**Request Example**:
```json
{
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

### Update a Passenger

**Endpoint**: `PUT /api/Passenger/{id}`

**Description**: Updates the details of a passenger.

**Request Example**:
```json
{
  "id": "string",
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

### Delete a Passenger

**Endpoint**: `DELETE /api/Passenger/{id}`

**Description**: Deletes a passenger by their ID.

---

## User Endpoints

### Register a User

**Endpoint**: `POST /api/User

/register`

**Description**: Registers a new user with either an Admin or User role. The ID is generated automatically by MongoDB, so leave it blank in the request body.

**Request Example**:
```json
{
  "username": "string",
  "email": "user@example.com",
  "password": "string",
  "role": "Admin"
}
```

### Login a User

**Endpoint**: `POST /api/User/login`

**Description**: Logs in a user and returns a JWT token for authentication.

**Request Example**:
```json
{
  "email": "user@example.com",
  "password": "string"
}
```

### Get User by ID

**Endpoint**: `GET /api/User/{id}`

**Description**: Fetches a user by their ID.

### Update a User

**Endpoint**: `PUT /api/User/{id}`

**Description**: Updates the details of a user.

### Delete a User

**Endpoint**: `DELETE /api/User/{id}`

**Description**: Deletes a user by their ID.

---

## Conclusion

For more details about the API or about each endpoint's request and response structure, please refer to the in-code comments or visit the Swagger UI at `/swagger/index.html`.

For a detailed setup and deployment guide, please refer to [InstallationGuide.md](InstallationGuide.md).
