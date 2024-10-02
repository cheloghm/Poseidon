### Security Guide for Poseidon API

---

## Table of Contents

1. [Introduction](#introduction)
2. [Authentication and Authorization](#authentication-and-authorization)
   - [JWT-based Authentication](#jwt-based-authentication)
   - [Role-based Authorization](#role-based-authorization)
3. [Rate Limiting to Prevent DDoS Attacks](#rate-limiting-to-prevent-ddos-attacks)
4. [Sensitive Data Handling](#sensitive-data-handling)
5. [Password Security](#password-security)
6. [HTTPS Configuration](#https-configuration)
7. [Security Headers](#security-headers)
8. [Logging and Monitoring](#logging-and-monitoring)
9. [Vulnerability Scanning](#vulnerability-scanning)
10. [Best Practices and Recommendations](#best-practices-and-recommendations)

---

## 1. Introduction

Security is a key focus in the Poseidon API project. This guide outlines the measures implemented to protect the API from potential threats such as unauthorized access, DDoS attacks, and data leaks. 

The Poseidon API uses several security practices, such as JWT-based authentication, rate limiting, sensitive data handling, password hashing, and logging to ensure a secure application.

---

## 2. Authentication and Authorization

### JWT-based Authentication

The Poseidon API uses JSON Web Tokens (JWT) for user authentication. JWTs are issued upon successful login and must be included in the `Authorization` header for all subsequent requests.

#### Steps for JWT Authentication:

1. **User Login**: A user provides their email and password in a POST request to the `/api/User/login` endpoint.
2. **JWT Token Issuance**: If the login is successful, a JWT token is issued. This token includes the user’s ID and role (`Admin` or `User`) and is signed using a secret key stored securely.
3. **Token Validation**: For every request, the JWT token must be included in the `Authorization` header as a Bearer token. The token is validated on every request to ensure the user is authenticated.
   
   Example header:
   ```
   Authorization: Bearer <JWT_TOKEN>
   ```

### Role-based Authorization

The API implements role-based authorization to restrict access to certain endpoints. Users can be assigned one of two roles:

- **Admin**: Can access administrative endpoints like managing users, passengers, and other sensitive data.
- **User**: Has access to user-level endpoints such as viewing passengers or their data.

Role-based authorization is enforced using attributes like `[Authorize(Roles = "Admin")]` and `[Authorize(Roles = "User, Admin")]` to protect specific API actions.

---

## 3. Rate Limiting to Prevent DDoS Attacks

To protect against Distributed Denial of Service (DDoS) attacks, Poseidon implements a custom **RateLimitingMiddleware**. This middleware limits the number of API requests a user can make within a defined time window. If the limit is exceeded, the API returns a `429 Too Many Requests` response.

### Configuration

In `RateLimitingMiddleware.cs`, you can configure:

- **Max Requests**: The maximum number of requests allowed per user within a specific time frame.
- **Time Window**: The time period (in seconds or minutes) during which the request count is calculated.

This helps mitigate DDoS attacks by preventing excessive API usage.

---

## 4. Sensitive Data Handling

Sensitive data, such as database credentials, JWT secrets, and other confidential information, is stored in environment variables and managed via `.env` files.

### Environment Files:

1. **Root-level `.env` file**: Contains MongoDB credentials, JWT secrets, and other sensitive configuration data for Docker and Kubernetes deployments.
2. **API-level `.env` file**: Contains sensitive information for local development.

Sensitive environment variables include:

- `MONGO_INITDB_ROOT_USERNAME`
- `MONGO_INITDB_ROOT_PASSWORD`
- `JWT_KEY`
- `JWT_ISSUER`
- `JWT_AUDIENCE`

This ensures that sensitive data is not hardcoded in the source code, reducing the risk of accidental exposure.

---

## 5. Password Security

User passwords are stored securely in the database by applying a cryptographic hash function. We use `PasswordHasher` with a strong algorithm like **PBKDF2** to hash and verify passwords.

### Password Security Workflow:

1. **Password Hashing**: Upon user registration, the password is hashed using a secure algorithm before being stored in the database.
2. **Password Verification**: During login, the provided password is hashed and compared with the stored hash to verify the user’s identity.

This ensures that passwords are never stored in plain text and are secure even if the database is compromised.

---

## 6. HTTPS Configuration

To secure communications between the client and server, the Poseidon API should be served over **HTTPS**. HTTPS ensures that all traffic is encrypted, preventing man-in-the-middle (MITM) attacks.

- When deploying Poseidon to production, ensure an SSL certificate is configured and enabled.
- All HTTP traffic should be redirected to HTTPS to enforce encryption.

For local development, SSL can be configured via your IDE or Docker.

---

## 7. Security Headers

Security headers provide an additional layer of protection against common attacks such as XSS, clickjacking, and code injection. Here are some headers implemented in Poseidon:

- **Content-Security-Policy**: Prevents XSS attacks by controlling the sources of content that the browser is allowed to load.
- **X-Frame-Options**: Prevents clickjacking by restricting iframes to the same origin.
- **Strict-Transport-Security**: Ensures that HTTPS is used for all communications with the server.

These headers can be added using middleware in the `Startup.cs` file or `Program.cs` configuration.

---

## 8. Logging and Monitoring

Poseidon uses **Serilog** for logging, and logs should be centrally monitored to detect suspicious activity or potential security incidents.

### Logging Considerations:

- **Sensitive Data**: Ensure sensitive data such as passwords and JWT tokens are never logged.
- **Access Logs**: Log user access patterns to detect unusual behavior.
- **Error Logs**: Log any unauthorized access attempts and security-related errors for auditing.

Logging and monitoring are critical to detect and respond to security incidents in real-time.

---

## 9. Vulnerability Scanning

To detect potential security vulnerabilities in the Poseidon API, we have integrated **Trivy** for container scanning.

### Steps for Vulnerability Scanning:

1. **Trivy Configuration**: A `trivy-config.yaml` file is used to specify severity levels and scanning options.
2. **CI/CD Integration**: Trivy is run as part of the CI/CD pipeline to automatically scan Docker images for known vulnerabilities.
3. **Reporting**: Vulnerabilities with a severity of **HIGH** or **CRITICAL** will fail the CI pipeline, ensuring they are fixed before production deployment.

---

## 10. Best Practices and Recommendations

- **Use HTTPS Everywhere**: Always serve the API over HTTPS, especially in production environments.
- **Enforce Strong Password Policies**: Ensure that passwords are strong (minimum length, special characters, etc.).
- **Regularly Rotate Secrets**: Rotate API keys, JWT secrets, and database credentials periodically.
- **Audit User Activity**: Regularly review logs and monitor access to detect any suspicious activity.
- **Update Dependencies**: Regularly update third-party packages to mitigate the risk of vulnerabilities in dependencies.
- **Run Penetration Tests**: Periodically run penetration tests to identify any security weaknesses.

---

This **SecurityGuide.md** provides a comprehensive view of the security practices implemented in the Poseidon API. These measures ensure that the API remains secure, reliable, and resilient against potential attacks.
