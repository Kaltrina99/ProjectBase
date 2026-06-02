# Structure Analysis

Overview
- The solution follows a Clean Architecture pattern separating concerns into layers:
  - `Core/` — domain entities and repository contracts.
  - `Application/` — application services, DTOs, request/response models.
  - `Infrastructure/` — EF Core DbContext, repositories, external service clients, and migrations.
  - `Api/` — ASP.NET Core Web API, controllers, middleware, DI, and startup.
  - `ClientApp/` — React + Vite frontend consuming the API.

ClientApp structure
- `src/` — application source.
- `src/api/` — API client code. Exposes typed functions (`fetchProducts`, `createProduct`, ...) and `swaggerUrl` (configurable via `VITE_SWAGGER_URL`).
- `src/modules/` — feature modules. Each module is a self-contained folder with component(s) and a CSS file. `App.tsx` mounts modules.
- `src/styles.ts` — central place to import global CSS (`index.css`, `App.css`).

Adding new modules
- Use the `npm run create-module -- <Name>` script in `ClientApp` to scaffold.
- Import the new module in `App.tsx` and render it.

Deployment considerations
- The UI is built into `ClientApp/dist` by Vite. For production, serve the static files from a CDN or a static hosting provider. API endpoints should be configured via environment variables in the API and the UI (set `VITE_SWAGGER_URL` or proxy accordingly).

Observations
- Keeping modules isolated makes the UI easy to extend.
- The API client is centralized; consider adding a thin wrapper to handle auth tokens and global error formatting.
