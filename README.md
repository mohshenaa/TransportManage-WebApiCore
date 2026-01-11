🚍 TransportManage-WebApiCore

A full-featured Transport Management System Web API built with ASP.NET Core, implementing a secure master-detail architecture with JWT authentication and role-based authorization.

Designed for scalable transport operations where each Trip can manage multiple Passengers with complete CRUD functionality.

🔗 Client Applications
Angular: https://github.com/mohshenaa/TransportManageSys_Angular-pro-9

React: https://github.com/mohshenaa/transport_management_react

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

Visual Studio 2022/2026 / VS Code

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


Request:

{
  "username": "admin",
  "password": "Admin@123"
}

Login
POST /api/auth/login


Response:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

🚌 Trip APIs
Get All Trips
GET /api/trips

Create Trip
POST /api/trips

{
  "route": "Dhaka → Chittagong",
  "departureTime": "2026-02-01T10:00:00"
}

Update Trip
PUT /api/trips/{id}

Delete Trip
DELETE /api/trips/{id}

👥 Passenger APIs (Master-Detail)
Add Passengers to Trip
POST /api/trips/{tripId}/passengers

[
  { "name": "Rahim", "seatNo": 5 },
  { "name": "Karim", "seatNo": 6 }
]

Get Passengers of Trip
GET /api/trips/{tripId}/passengers

🧠 Master-Detail Concept

Each Trip can have multiple Passengers.
The API allows bulk insert/update of passengers in one transaction — ensuring consistency and high performance.

🧪 Recommended Development Flow

Register admin user

Login & obtain JWT

Use Swagger for testing

Connect Angular / React frontend

📦 Tech Stack
Layer	Technology
Backend	ASP.NET Core
ORM	Entity Framework Core
DB	SQL Server
Auth	JWT
Frontend	Angular, React
Docs	Swagger
