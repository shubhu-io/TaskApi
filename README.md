# Task Manager REST API

A REST API for managing tasks, built with C#, ASP.NET Core minimal APIs and Entity Framework Core with a SQLite database. Includes input validation, filtering, search, a stats endpoint and xUnit tests.

## Features
- Full CRUD operations (create, read, update, delete) for tasks
- Input validation: title is required (max 100 characters), priority must be low, medium or high
- Filter by status and case-insensitive search by title
- Stats endpoint with total, completed and pending counts
- Entity Framework Core for database access, SQLite database created automatically on first run
- Unit tests for the validation logic using xUnit

## Tech Stack
C#, ASP.NET Core, Entity Framework Core, SQLite, xUnit

## API Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /tasks | Get all tasks |
| GET | /tasks?done=true | Filter tasks by completion status |
| GET | /tasks?search=net | Search tasks by title |
| GET | /tasks/stats | Get total, completed and pending counts |
| GET | /tasks/{id} | Get one task |
| POST | /tasks | Create a task |
| PUT | /tasks/{id} | Update a task |
| DELETE | /tasks/{id} | Delete a task |

Task model:
```json
{ "id": 1, "title": "Learn .NET", "isDone": false, "priority": "high", "dueDate": null }
```

Invalid requests return `400 Bad Request` with a list of errors.

## Run Locally
Requires the .NET SDK.

```powershell
git clone https://github.com/shubhu-io/TaskApi.git
cd TaskApi
dotnet run --urls http://localhost:5000
```

## Run Tests
```powershell
dotnet test TaskApi.Tests
```

## Try It (PowerShell)
```powershell
Invoke-RestMethod -Method Post -Uri http://localhost:5000/tasks -ContentType "application/json" -Body '{"title":"Learn .NET","priority":"high","isDone":false}'
Invoke-RestMethod -Uri "http://localhost:5000/tasks?search=net"
Invoke-RestMethod -Uri "http://localhost:5000/tasks?done=true"
Invoke-RestMethod -Uri http://localhost:5000/tasks/stats
Invoke-RestMethod -Method Put -Uri http://localhost:5000/tasks/1 -ContentType "application/json" -Body '{"title":"Learn .NET","priority":"high","isDone":true}'
Invoke-RestMethod -Method Delete -Uri http://localhost:5000/tasks/1
```

## Project Structure
- `Program.cs`: API endpoints
- `Models.cs`: task model, database context and validation
- `TaskApi.Tests/`: xUnit tests

## Author
Shubham Mane - [GitHub](https://github.com/shubhu-io)
