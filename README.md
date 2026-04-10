🚍 TransportManage-WebApiCore
<img width="1912" height="777" alt="image" src="https://github.com/user-attachments/assets/eacb9988-2162-4cd1-89e5-b6b573339f83" />

A full-featured Transport Management System Web API built with ASP.NET Core, implementing a secure master-detail architecture with JWT authentication and role-based authorization.

Designed for scalable transport operations where each Trip can manage multiple Passengers with complete CRUD functionality.

🔗 Client Applications

Angular Frontend
https://github.com/mohshenaa/TransportManageSys_Angular-pro-9

React Frontend
https://github.com/mohshenaa/transport_management_react

🧱 System Architecture
Client Apps (Angular / React)
        │
        ▼
ASP.NET Core Web API
        │
        ▼
Entity Framework Core
        │
        ▼
SQL Server Database

🧩 Key Features

Master-Detail Design (Trip → Passengers)

JWT Authentication

Role-based Authorization

Secure API Endpoints

Swagger API Documentation

Clean Project Structure

EF Core Migrations

Production-ready architecture

📂 Project Structure
TransportManage-WebApiCore
│
├── Controllers
├── Models
├── Data
├── Services
├── Migrations
├── Program.cs
├── appsettings.json
└── README.md

🛠 Setup & Installation
1️⃣ Requirements

.NET 6+

SQL Server

Visual Studio 2022 / 2026 or VS Code

2️⃣ Configure Database

Edit appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TransportManageDB;Trusted_Connection=True;"
}

3️⃣ Apply Migrations
Update-Database

4️⃣ Run Project
dotnet run


Swagger UI:

https://localhost:5001/swagger

🔐 Authentication Workflow

Register

Login → Receive JWT Token

Authorize in Swagger

Access secured endpoints

🧪 API Documentation
🧑 Authentication
Register
POST /api/auth/register

{
  "username": "admin",
  "password": "Admin@123"
}

Login
POST /api/auth/login

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

🚌 Trip APIs
Action	Endpoint
Get All Trips	GET /api/trips
Create Trip	POST /api/trips
Update Trip	PUT /api/trips/{id}
Delete Trip	DELETE /api/trips/{id}

Create Trip Example

{
  "route": "Dhaka → Chittagong",
  "departureTime": "2026-02-01T10:00:00"
}

👥 Passenger APIs (Master-Detail)
Action	Endpoint
Add Passengers	POST /api/trips/{tripId}/passengers
Get Passengers	GET /api/trips/{tripId}/passengers
[
  { "name": "Rahim", "seatNo": 5 },
  { "name": "Karim", "seatNo": 6 }
]

🧠 Master-Detail Concept

Each Trip can contain multiple Passengers.
The API supports bulk insert/update operations in a single transaction, ensuring data integrity and high performance.

🛜For visualize connect Angular or react
