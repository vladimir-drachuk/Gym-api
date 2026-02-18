# GymApi

A RESTful API for managing gym workouts built with **.NET 10** and **ASP.NET Core**.

## Features

- JWT authentication (register / login)
- Manage **exercises** (CRUD)
- Manage **workout plans** (CRUD)
- Manage **workouts** with exercises and sets
- Add / update / delete exercises and sets within a workout

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server (LocalDB) |
| Auth | JWT Bearer |
| API Docs | Scalar (OpenAPI) |
| Password Hashing | BCrypt |

## Project Structure

```
GymApi/
├── GymApi/          # Web API (controllers, services, models)
└── DataLayer/       # EF Core (entities, repositories, migrations)
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server or LocalDB

### Setup

**1. Clone the repository**
```bash
git clone <repo-url>
cd GymApi
```

**2. Configure the connection string**

Edit `GymApi/appsettings.json` if needed (defaults to LocalDB):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GymDb;Trusted_Connection=true"
}
```

**3. Apply migrations**
```bash
dotnet ef database update --project DataLayer --startup-project GymApi
```

Or via Package Manager Console in Visual Studio:
```powershell
Update-Database -Project DataLayer -StartupProject GymApi
```

**4. Run the API**
```bash
dotnet run --project GymApi
```

### API Docs

Once running, open the interactive API reference:
```
https://localhost:{port}/scalar/v1
```

## Authentication

All endpoints (except `/api/auth/register` and `/api/auth/login`) require a Bearer token.

```http
POST /api/auth/register
POST /api/auth/login
```

Use the returned token in the `Authorization` header:
```
Authorization: Bearer <token>
```

## Main Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and get JWT token |
| GET/POST/PUT/DELETE | `/api/exercises` | Manage exercises |
| GET/POST/PUT/DELETE | `/api/workoutplans` | Manage workout plans |
| GET/POST/PUT/DELETE | `/api/workouts` | Manage workouts |
| POST | `/api/workouts/{id}/exercises` | Add exercises to workout |
| PUT/DELETE | `/api/workouts/exercises/{id}` | Update / delete workout exercise |
| POST/PUT/DELETE | `/api/workouts/exercises/{id}/sets/{setId}` | Manage sets |
