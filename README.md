# Clean Architecture Sample

A production-style ASP.NET Core + React solution using Clean Architecture (N-Tier) principles.

## Overview

This repository contains:

- `Api/` - ASP.NET Core Web API with controllers, middleware, structured logging, global error handling, and environment-aware configuration.
- `Application/` - Application services, DTOs, request models, and service interfaces.
- `Core/` - Domain entities, repository contracts, and business logic.
- `Infrastructure/` - EF Core `DbContext`, repository implementations, HttpClient resiliency, and data seeding.
- `ClientApp/` - React + Vite frontend consuming the API with full CRUD support.

## Folder structure

- `Api/`
  - `Controllers/ProductsController.cs`
  - `Middleware/ApiExceptionMiddleware.cs`
  - `Program.cs`
  - `Properties/launchSettings.json`
  - `appsettings.json`, `appsettings.Development.json`, `appsettings.Production.json`
- `Application/`
  - `DTOs/` product response models
  - `Requests/` create/update request models
  - `Interfaces/IProductService.cs`
  - `Services/ProductService.cs`
- `Core/`
  - `Entities/Product.cs`
  - `Interfaces/IRepository.cs`, `IProductRepository.cs`
- `Infrastructure/`
  - `Data/AppDbContext.cs`
  - `Data/AppDbContextSeed.cs`
  - `Repositories/ProductRepository.cs`
  - `External/ExternalServiceClient.cs`
  - `Extensions/ServiceCollectionExtensions.cs`
  - `Migrations/` generated EF Core migrations
- `ClientApp/`
  - `src/` React app source
  - `vite.config.ts` proxy to API
  - `package.json` UI scripts
- `package.json` root launcher for both API and UI

## Run the solution

### 1. Install dependencies

```powershell
cd C:\Users\kaltr\source\Base
npm install
cd ClientApp
npm install
```

### 2. Run API + UI together

From the repo root:

```powershell
cd C:\Users\kaltr\source\Base
npm run start
```

This starts:

- API: `http://localhost:5010`
- UI: `http://localhost:5173`

### 3. Run API only

```powershell
cd C:\Users\kaltr\source\Base
npm run api
```

### 4. Run UI only

```powershell
cd C:\Users\kaltr\source\Base
npm run ui
```

## Database and migrations

The repository includes an initial EF Core migration in `Infrastructure/Migrations/`.

To apply migrations manually:

```powershell
cd C:\Users\kaltr\source\Base
dotnet ef database update --project Infrastructure --startup-project Api --context AppDbContext
```

The Startup logic in `Api/Program.cs` also applies migrations and seeds data automatically at runtime.

## API Endpoints

- `GET /api/Products` - list products
- `GET /api/Products/{id}` - get product by id
- `POST /api/Products` - create product
- `PUT /api/Products/{id}` - update product
- `DELETE /api/Products/{id}` - delete product

## React UI

The React frontend is in `ClientApp/` and is configured with a proxy to the API.

- `ClientApp/vite.config.ts` proxies `/api` to `http://localhost:5010`
- `ClientApp/src/App.tsx` is the main CRUD UI
- `ClientApp/src/api.ts` consumes the API endpoints

## Environment and configuration

- `Api/appsettings.json` contains shared configuration.
- `Api/appsettings.Development.json` contains development connection strings and external service settings.
- `Api/appsettings.Production.json` contains production placeholders.
- `Api/Properties/launchSettings.json` includes development and production launch profiles.

## Structure Notes

This solution separates concerns by layer:

- Core: business entities and repository contracts.
- Application: service orchestration, request/response models, and business rules.
- Infrastructure: external dependencies, data persistence, HTTP clients, and migrations.
- Api: web endpoints, middleware, dependency injection, and startup behavior.
- ClientApp: frontend UI and API consumption.

## UI Improvements

The React UI now includes:

- responsive dashboard layout
- product cards with edit/delete actions
- create/update form with inline editing support
- status cards and better spacing
- clear error and loading states

## Modules (UI)

The frontend uses a modular layout under `ClientApp/src/modules/`. Each feature can be placed inside its own folder (component + styles + README). This makes it easy to add new functionality without changing `App.tsx` beyond importing the module.

Scaffolding helper:

From `ClientApp` you can run:

```powershell
npm run create-module -- Product
```

This creates `src/modules/Product` with a starter component, CSS and README.

Swagger configuration for the UI:

The frontend reads the Swagger URL from the Vite env variable `VITE_SWAGGER_URL` (if set). If not provided it falls back to `http://localhost:5010/swagger/index.html`.

