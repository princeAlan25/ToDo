# ToDo

A cross-platform task manager built on .NET 10: a .NET MAUI client backed by an ASP.NET Core Minimal API over EF Core and SQLite.

> **Status: in active development.** The backend API is functional and covers authentication, users, roles, categories and tasks. The MAUI client is still scaffolding: it builds and navigates, but is not yet wired to the API. See [Current status](#current-status) for the honest breakdown.

## Solution layout

```
ToDo.slnx
├── ToDoApi/             ASP.NET Core Minimal API (net10.0)
├── ToDoEntityModels/    EF Core models, DbContext, migrations (net10.0)
├── ToDoUi/              .NET MAUI client (Android, iOS, macOS, Windows)
└── ToDo.Interfaces/     Not currently referenced by the solution
```

The API follows a layered request path with dependency injection at every seam:

```
HTTP request
  └── EndPoints/       route groups, model binding, auth policy
        └── Services/        business logic (registered as "intermediators")
              └── Repositories/   data access
                    └── ToDoContext (EF Core)
                          └── SQLite
```

Interfaces are declared separately from implementations (`IIntermediators/`, `IRepositories/`) and wired in `Program.cs`, so each layer depends on an abstraction rather than a concrete type.

## Tech stack

| Area | Technology |
|---|---|
| Runtime | .NET 10 |
| API | ASP.NET Core Minimal API, route groups |
| Client | .NET MAUI 10, Shell navigation, CommunityToolkit.Maui 15 |
| Data | EF Core 10, SQLite |
| Auth | JWT bearer tokens, BCrypt.Net-Next password hashing |
| Docs | OpenAPI, Swashbuckle (Swagger UI) |

**Client target platforms:** Android, iOS, macOS (Mac Catalyst), Windows.

## Data model

| Entity | Key | Notable fields |
|---|---|---|
| `User` | `Guid UserId` | Email, Name, Password (BCrypt hash), RoleId |
| `Role` | `int RoleId` | Name (max 30 chars) |
| `Category` | `int CategoryId` | Name, ColorCode, Description, UserId |
| `TaskItem` | `int TaskId` | Name, Description, Priority, Starred, CategoryId |

Relationships:

- `User` to `Role`, many-to-one, `SetNull` on delete
- `User` to `Category`, one-to-many, `SetNull` on delete
- `Category` to `TaskItem`, one-to-many, `Cascade` on delete

Enums: `TaskPriority { Low, Medium, High }` and `TaskRate { Low, Medium, High, VeryHigh, ExtraHigh }`.

Three migrations are applied: entity mapping, role seeding, and auto-increment IDs. Seven roles are seeded on model creation.

## API endpoints

All routes require a valid JWT except the two under `/auth`. Authorization uses the named policy `authenticated`.

### `/auth`
| Method | Route | Purpose |
|---|---|---|
| POST | `/auth/signup` | Register a user, returns the created user |
| POST | `/auth/login` | Verify credentials, returns an access token |

### `/user`
| Method | Route | Auth |
|---|---|---|
| GET | `/user/` | Yes, reads the caller's ID from the token claims |
| POST | `/user/` | No |
| PUT | `/user/` | Yes |
| DELETE | `/user/{userId}` | Yes |

### `/role`
| Method | Route |
|---|---|
| GET | `/role/` |
| GET | `/role/{roleId}` |
| POST | `/role/` |
| PUT | `/role/` |
| DELETE | `/role/{roleId}` |

### `/categories`
| Method | Route |
|---|---|
| GET | `/categories/` |
| GET | `/categories/{categoryId:int}` |
| POST | `/categories/` |
| PUT | `/categories/` |
| DELETE | `/categories/{categoryId:int}` |

### `/tasks`
| Method | Route |
|---|---|
| GET | `/tasks/` |
| GET | `/tasks/{taskId:int}` |
| POST | `/tasks/` |
| PUT | `/tasks/` |
| DELETE | `/tasks/{taskId:int}` |

## Getting started

### Prerequisites

- .NET 10 SDK
- MAUI workloads for the client: `dotnet workload install maui`
- To build the iOS or Mac Catalyst targets you need macOS with Xcode

### Run the API

```bash
git clone https://github.com/princeAlan25/ToDo.git
cd ToDo
dotnet restore
dotnet run --project ToDoApi
```

Swagger UI is served in the Development environment. The SQLite database file is created automatically on first run if it does not exist.

### Apply migrations manually

```bash
dotnet ef database update --project ToDoEntityModels --startup-project ToDoApi
```

### Run the client

```bash
dotnet build ToDoUi -f net10.0-android
```

Swap the framework moniker for `net10.0-ios`, `net10.0-maccatalyst`, or `net10.0-windows10.0.19041.0` as needed.

## Current status

### Working

- Signup and login with BCrypt-hashed passwords and JWT issuance
- Full CRUD for users, roles, categories and tasks
- JWT bearer authentication enforced on all non-auth endpoints
- EF Core schema with three migrations and seeded roles
- Swagger UI with a Bearer security definition, so the API is testable end to end
- MAUI project builds and targets all four platforms, Shell flyout navigation in place

### Not done yet

- **The MAUI client is not connected to the API.** There is no `HttpClient`, no service layer, and no models on the client side.
- `HomePage.xaml` is a placeholder containing a single coloured `BoxView`.
- `AppShell.xaml` declares four `FlyoutItem` entries that all point at `HomePage`; three are duplicates awaiting real pages.
- No automated tests in the solution.
- No CI workflow.

## Known issues

These are tracked deliberately rather than hidden, and are the next things to address.

1. **The JWT signing key is committed in `appsettings.json`.** It needs to move to user secrets or environment variables before this repository is used for anything real, and the current key should be rotated.
2. **Access tokens expire after 3 minutes**, which looks like a debugging value rather than an intended lifetime. There is no refresh token flow.
3. **`AuthService` blocks on async calls** using `.Result` in `SignIn` and `SignUp`. These should be awaited to avoid thread-pool starvation and deadlock risk.
4. **54 log files are committed** under `ToDoApi/Loggs/`. The `.gitignore` does not cover them or the generated `.db` file.
5. **`ToDo.Interfaces` is orphaned.** It is not listed in `ToDo.slnx` and no project references it; its interfaces are duplicated inside `ToDoApi`. It should be adopted as the shared contracts project or removed.
6. **The database path uses relative traversal** (`../../../../Database/ToDo.db`) in both `ToDoContext` and `ToDoContextExtension`. This breaks outside the development directory layout and should come from configuration.
7. **Seeded roles carry `new DateTime()`**, which stores `0001-01-01` rather than a real timestamp.
8. **The seeded role names** (Business, Sport, Learning, Teaching, Daily, Travel, Work) read as task categories rather than security roles, so `Role` and `Category` may be modelling the same concept twice.
9. `Utilities/Securit.cs` is missing a "y".

## Roadmap

- [ ] Wire the MAUI client to the API with a typed `HttpClient` and token storage
- [ ] Build real task list, task detail, and category pages
- [ ] Move JWT configuration to user secrets and set a sensible token lifetime
- [ ] Add a refresh token flow

## Recent commits (since 2026-08-13)

The following changes were committed after the README was last updated (commit 1ddf62f). They are recorded here so the repository documentation stays up to date.

- 65f1588 (2026-08-14) — "Switch to MauiIcons Cupertino; update Flyout menu/icons"
  - UI: Replaced icon set with MauiIcons Cupertino and updated the Shell flyout items to use the new icons. No functional API changes.

- 6477493 (2026-08-14) — "Refactor DTOs to shared project; add auth features to UI"
  - Shared code: Moved DTO classes to a shared project so both API and MAUI client can reuse the models.
  - Client: Added initial authentication-related UI elements and plumbing in the MAUI project (signup/login pages and wiring for auth flows).

- e6ddbb1 (2026-08-14) — "Merge branch 'master' of https://github.com/princeAlan25/ToDo into frontend"
  - Merge: Integrated incoming changes from master into the frontend branch. See the other entries here for concrete feature additions.

- 3f339c0 (2026-08-15) — "Implement MVVM for LoginPage with validation"
  - Client: Converted the LoginPage to follow MVVM patterns, added a LoginViewModel with validation logic and basic commands. This improves testability and separates UI from auth logic.

If you'd like these entries promoted to a formal CHANGELOG file or expanded with links to PRs and issue numbers, tell me which format you prefer and I will add it.
- [ ] Await the async calls in `AuthService`
- [ ] Gitignore `Loggs/` and `*.db`, and purge the committed logs
- [ ] Resolve the `ToDo.Interfaces` project
- [ ] Add unit tests for services and integration tests for endpoints
- [ ] Add a GitHub Actions build workflow

## License

See [LICENSE.txt](LICENSE.txt).
