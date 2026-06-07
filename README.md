# E-Learning API — Course Enrollment & Approval System

RESTful API built with ASP.NET Core 10 for managing employee course enrollments with an approval workflow, designed for governmental e-learning platforms.

---

## ⚡ Quick Start

> **The database is already deployed on a SQL Server instance and ready to use. No database setup or migration execution is required.**

### Requirements

* .NET 10 SDK

### Run the Application

```bash
# Clone the repository
git clone <repository-url>

# Navigate to the project
cd E-Learning

# Run the application
dotnet run
```

Swagger will be available after startup.

---

## 🗄️ Database

The SQL Server database is already hosted and configured.

The connection string is preconfigured in `appsettings.json`, so no additional database setup is required.

---

## 🏗️ Architecture & Design Decisions

### Service Layer Pattern

Controllers contain no business logic. All business rules are implemented inside dedicated service classes.

### ServiceResult<T>

Expected business failures are returned using a unified `ServiceResult<T>` wrapper instead of throwing exceptions.

### Global Exception Middleware

Unhandled exceptions are centrally managed through a Global Exception Middleware to ensure consistent API responses.

### Role Simulation via Request Headers

Authentication is simulated using request headers:

* `X-User-Role`
* `X-User-Id`

The middleware converts these values into a strongly typed `UserContext`.

### Enum Storage

`EnrollmentStatus` values are stored as strings in SQL Server for improved readability and maintainability.

### Audit Logging

Every approval or rejection decision generates an audit log entry containing old and new values serialized as JSON.

### Pagination

List endpoints support pagination using:

```http
?page=1&pageSize=10
```

---

## 🔐 Role Simulation

Every request should include:

| Header      | Allowed Values          |
| ----------- | ----------------------- |
| X-User-Role | Admin, Manager, Learner |
| X-User-Id   | Any valid integer       |

Examples:

```http
X-User-Role: Admin
X-User-Id: 1
```

---

## 📡 API Endpoints

### Courses

| Method | Endpoint          | Description      | Role  |
| ------ | ----------------- | ---------------- | ----- |
| GET    | /api/courses      | Get all courses  | All   |
| GET    | /api/courses/{id} | Get course by id | All   |
| POST   | /api/courses      | Create course    | Admin |
| PUT    | /api/courses/{id} | Update course    | Admin |
| DELETE | /api/courses/{id} | Delete course    | Admin |

---

### Learners

| Method | Endpoint           |
| ------ | ------------------ |
| GET    | /api/learners      |
| GET    | /api/learners/{id} |
| POST   | /api/learners      |

---

### Enrollments

| Method | Endpoint                       | Description                    | Role    |
| ------ | ------------------------------ | ------------------------------ | ------- |
| GET    | /api/enrollments               | Get enrollments with filtering | All     |
| GET    | /api/enrollments/{id}          | Get enrollment by id           | All     |
| POST   | /api/enrollments               | Create enrollment              | Learner |
| POST   | /api/enrollments/{id}/decision | Approve/Reject enrollment      | Manager |

---

## 🔍 Filtering & Pagination

```http
GET /api/enrollments?learnerId=1
GET /api/enrollments?courseId=2
GET /api/enrollments?status=Approved
GET /api/enrollments?fromDate=2026-01-01&toDate=2026-12-31

GET /api/enrollments?page=2&pageSize=5
```

---

## 📦 Sample Requests

### Create Course

```http
POST /api/courses
X-User-Role: Admin
X-User-Id: 1
```

```json
{
  "title": "Government Cyber Security",
  "description": "Cyber security best practices",
  "durationHours": 8,
  "requiresApproval": true,
  "isActive": true
}
```

### Create Learner

```json
{
  "fullName": "Ahmed Al Rashidi",
  "email": "ahmed@ministry.gov",
  "nationalId": "1234567890",
  "department": "IT Department"
}
```

### Create Enrollment

```http
POST /api/enrollments
X-User-Role: Learner
X-User-Id: 10
```

```json
{
  "learnerId": 1,
  "courseId": 1
}
```

### Approve Enrollment

```http
POST /api/enrollments/1/decision
X-User-Role: Manager
X-User-Id: 5
```

```json
{
  "decision": "Approved",
  "reason": "Employee meets all requirements"
}
```

### Reject Enrollment

```json
{
  "decision": "Rejected",
  "reason": "Employee does not meet prerequisites"
}
```

---

## ✅ Business Rules

* A learner cannot enroll in the same course more than once.
* Enrollment in inactive courses is not allowed.
* Courses requiring approval start with `PendingApproval`.
* Courses not requiring approval are automatically `Approved`.
* Only `PendingApproval` enrollments can be approved or rejected.
* Decisions are restricted to `Approved` or `Rejected`.
* Rejected enrollments must include a reason.
* Every approval/rejection action is recorded in `AuditLogs`.

---

## 🗄️ Database Schema

### Courses

* Id
* Title
* Description
* DurationHours
* RequiresApproval
* IsActive
* CreatedAt

### Learners

* Id
* FullName
* Email (Unique)
* NationalId (Unique)
* Department
* CreatedAt

### Enrollments

* Id
* LearnerId (FK)
* CourseId (FK)
* Status
* EnrollmentDate
* DecisionDate
* DecisionReason
* DecisionByUserId

Unique Constraint:

```sql
UNIQUE(LearnerId, CourseId)
```

### AuditLogs

* Id
* EntityName
* EntityId
* Action
* OldValue
* NewValue
* PerformedBy
* PerformedAt
* EnrollmentId (Nullable FK)

---

## 🛠️ Technologies

* ASP.NET Core 10
* Entity Framework Core
* SQL Server
* Swagger / OpenAPI
* FluentValidation
* Global Exception Middleware
* Service Layer Pattern
* Pagination
* Audit Logging

---

## 👨‍💻 Author

Developed as part of the Backend .NET Developer Assessment.
