# Twitter Clone App

A full-stack MVP social media application inspired by X (Twitter). Users can browse all posts, view their own posts, create new posts (optionally with an image), and delete their own posts. The application assumes a single pre-authenticated user, as no authentication system was in scope.

## Tech Stack

**Backend**
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core (In-Memory provider)
- CQRS pattern via MediatR
- FluentValidation
- Clean Architecture (Domain / Application / Infrastructure / Api)
- xUnit + Moq for unit testing

**Frontend**
- Angular 22 (standalone components, zoneless change detection, Signals)
- Angular Material
- RxJS
- SCSS

## Features

- Browse all posts in a chronological feed
- View only your own posts on a separate page
- Create a new post (12–140 characters, optional image attachment)
- Delete your own posts
- Client-side and server-side validation
- Image upload with type and size restrictions (JPG, PNG, WEBP, max 5MB)
- Responsive design for desktop, tablet, and mobile
- Seeded demo data (multiple users and posts) for immediate exploration

## Architecture

The backend follows Clean Architecture principles with a CQRS implementation:

```
Domain        - Core entities (User, Post), no external dependencies
Application   - Commands, Queries, Handlers (MediatR), DTOs, validation, interfaces
Infrastructure - EF Core DbContext, entity configurations, seed data, service implementations
Api           - Controllers, middleware, dependency injection composition root
```

Dependency direction: `Api -> Application + Infrastructure`, `Infrastructure -> Application`, `Application -> Domain`, `Domain -> nothing`.

The frontend follows a smart/dumb component split, with a single service (`PostsService`) as the source of truth for API communication, and a publish/subscribe pattern (via an RxJS `Subject`) to keep multiple pages in sync after create/delete operations.

## Project Structure

```
twitter-clone-app/
├── backend/
│   ├── src/
│   │   ├── TwitterCloneApp.Domain/
│   │   ├── TwitterCloneApp.Application/
│   │   ├── TwitterCloneApp.Infrastructure/
│   │   └── TwitterCloneApp.Api/
│   └── tests/
│       └── TwitterCloneApp.Application.Tests/
└── frontend/
    └── src/app/
        ├── components/
        ├── pages/
        ├── services/
        ├── interceptors/
        ├── validators/
        ├── pipes/
        ├── utils/
        └── models/
```

## Prerequisites

- .NET SDK 10.0 or later
- Node.js 22.22.0 or later
- Angular CLI 22 (`npm install -g @angular/cli`)

## Getting Started

### Backend

```bash
cd backend
dotnet build
dotnet run --project src/TwitterCloneApp.Api
```

The API will be available at `http://localhost:5118`, with Swagger UI at `http://localhost:5118/swagger` in development mode. Demo data (users and posts) is seeded automatically on startup.

### Frontend

```bash
cd frontend
npm install
ng serve
```

The application will be available at `http://localhost:4200`. The backend must be running for the frontend to load data.

## Running Tests

```bash
cd backend
dotnet test
```

Tests cover business-critical logic in the Application layer: content validation rules and ownership checks for post deletion (not-found and forbidden-access scenarios).

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/posts` | Get all posts |
| GET | `/api/posts/mine` | Get posts belonging to the current user |
| POST | `/api/posts` | Create a new post (multipart/form-data: `content`, optional `image`) |
| DELETE | `/api/posts/{id}` | Delete a post owned by the current user |

## Notes

- The application uses an In-Memory database; all data is reset whenever the backend restarts.
- There is no authentication system. The current user is a fixed value provided through an `ICurrentUserProvider` abstraction, designed so that real authentication could be introduced later without changing any business logic.
- Uploaded images are stored on local disk under `backend/src/TwitterCloneApp.Api/wwwroot/uploads` and served as static files. This is suitable for a local MVP but would require cloud-based storage for a production deployment.
