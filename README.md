# ToDo

A cross-platform task manager built on .NET 10: a .NET MAUI client backed by an ASP.NET Core Minimal API over EF Core and SQLite.
<img width="960" height="540" alt="Screenshot 2026-08-24 211808" src="https://github.com/user-attachments/assets/609f2f31-720e-45b7-bcdd-fddea3db0f11" />
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
6. **The database path uses relative traversal** (`../../../../Database/ToDo.db`) in both `ToDoContext` and

- File: ToDoUi/AppShell.xaml.cs
  - Problem: A StackOverflowException occurred during navigation when the app attempted to navigate to LoginPage while already navigating there. The root cause was a logical condition that evaluated true even when the destination was LoginPage, causing recursive navigation.
  - Fix applied: Replaced the logical OR (||) with logical AND (&&) in OnNavigating so the code only navigates to LoginPage when the destination is neither LoginPage nor SignUpPage:
    if (!destination.Contains(nameof(LoginPage)) && !destination.Contains(nameof(SignUpPage)))
  - Effect: Prevents re-entrant navigation to LoginPage and resolves the StackOverflowException observed during debugging.
  - Suggested follow-ups (optional):
    - Implement a reentrancy guard (e.g., a private bool _isNavigating) to prevent recursive navigation more robustly.
    - Consider using args.Cancel = true before calling GoToAsync to explicitly cancel the current navigation before starting a new one.
    - Add a unit/integration test for navigation flows or manually verify by running the app and navigating between pages.

- Switch to MauiIcons Cupertino; update Flyout menu/icons
  - UI: Replaced icon set with MauiIcons Cupertino and updated the Shell flyout items to use the new icons. No functional API changes.

- Refactor DTOs to shared project; add auth features to UI
  - Shared code: Moved DTO classes to a shared project so both API and MAUI client can reuse the models.
  - Client: Added initial authentication-related UI elements and plumbing in the MAUI project (signup/login pages and wiring for auth flows).

- Implement MVVM for LoginPage with validation
  - Client: Converted the LoginPage to follow MVVM patterns, added a LoginViewModel with validation logic and basic commands. This improves testability and separates UI from auth logic.

-Add Authentication and Refactoring
  - Client: Refactor the Components by Adding AppShell Helper for registering Routes and MauiProgram Helper for registering different services clear
  - Client: Add Authentication by Connecting the client Login component to the Backend Api


## Unstaged changes (local, not yet staged or committed)

Generated automatically. These are the files currently modified or untracked in your working tree and a short description of each change — document these before you stage/commit them.

- ToDoUi/App.xaml.cs
  - Added `using ToDoUi.Views;` to allow direct references to MAUI pages from App.

- ToDoUi/AppShell.xaml
  - Adjusted FlyoutItem route values (capitalization) and ensured Login flyout entry points to the LoginPage. No functional behavior change other than route normalization.

- ToDoUi/AppShell.xaml.cs
  - Implemented root-route navigation handling: collects top-level routes, overrides OnNavigating to detect navigation to a top-level route and perform absolute navigation (Shell.Current.GoToAsync("//{route}")). Added AppShellHelper.RegisterRoutes() call and basic error handling.

- ToDoUi/Helpers/AppShellHelper.cs
  - Updated RegisterRoutes() to register the `signup` route (SignUpPage). Removed previous routing registrations for login/home so routing is centralized here.

- ToDoUi/Helpers/MauiProgramHelper.cs
  - Registered SignUpPage and SignUpViewModel in the DI container (AddTransient). Removed AppShell singleton registration. Ensures pages and viewmodels are available via constructor injection.

- ToDoUi/Services/Implementations/AuthenticationService.cs
  - Minor reordering/cleanup in LoginAsync and SignUpAsync: sets access token when login response is present and returns the response. Formatting and consistency fixes.

- ToDoUi/ViewModels/LoginViewModel.cs
  - Converted generated-parameter style to an explicit constructor that accepts IAuthenticationService and calls ValidateAllProperties(). Initialized Email and Password backing fields to empty strings, added IQueryAttributable implementation (ApplyQueryAttributes) to accept `userEmail` from navigation query, and adjusted CanLogin logic.

- ToDoUi/Views/LoginPage.xaml
  - UI update: added a small "You don't have an account? Signup" link (HorizontalStackLayout) under the Login button with a TapGestureRecognizer wired to navigate to the SignUp route.

- ToDoUi/Views/LoginPage.xaml.cs
  - Removed an unused using; added GoToSignUp event handler to navigate to the signup route using Shell.Current.GoToAsync("signup", true).

- ToDoUi/Views/SignUpPage.xaml and ToDoUi/Views/SignUpPage.xaml.cs
  - Added a new SignUpPage view with bindings to a SignUpViewModel (x:DataType). The code-behind constructor now accepts a SignUpViewModel and sets BindingContext accordingly. Page contains email/username/password entries, validation labels and a SignUp button bound to SignUpCommand.

- ToDoUi/ViewModels/SignUpViewModel.cs (untracked)
  - New view model implementing validation using ObservableValidator and CommunityToolkit attributes. Exposes Email, UserName, Password, error properties and a SignUpCommand that calls IAuthenticationService.SignUpAsync and navigates back to Login with the signed-up user's email as a query parameter.

Notes / testing
- These changes add a signup flow (page + viewmodel) and wire it into Shell routing and DI. Verify the app starts and navigate to the Login page, then tap the Signup link to confirm the SignUp page opens.


### Refactor route naming and navigation for consistency

- Standardized FlyoutItem routes in AppShell.xaml to "HomePage" and updated Login route to "LoginPAge" (with typo).
- Simplified AppShell.xaml.cs by removing top-level navigation and route collection logic.
- Updated AppShellHelper.cs to register sign-up route using nameof(SignUpPage).
- Changed SignUpViewModel.cs navigation to use "LoginPage" and removed double slash.
- Modified LoginPage.xaml.cs to navigate to sign-up using nameof(SignUpPage).
- Refactored App.xaml.cs for formatting and code style; logic unchanged.

## License

See [LICENSE.txt](LICENSE.txt).

## Commit 85d68705507d837448d64badf5b6ab6394df6439 — In-depth technical documentation

This section documents the local commit that introduced authentication state, a dynamic UI, and related plumbing for the MAUI client. It explains the motivation, the patterns applied, a file-to-pattern mapping, testing guidance, security recommendations, and notes for reviewers and maintainers.

1) Purpose and problem statement
--------------------------------
- Problem: The MAUI client previously had static navigation and no centralized representation of authentication state. This prevented the app from conditionally showing user-specific UI, protecting routes consistently, and reliably attaching bearer tokens to API requests.
- Goal: Add a small, testable authentication subsystem and UI wiring so the client can: (a) perform login/signup/logout, (b) persist and attach access tokens to API calls, and (c) reflect authentication state in the Shell flyout and navigation.

2) High-level solution
----------------------
- ShellViewModel centralizes auth state and exposes UI properties (IsAuthenticated, DisplayName).
- AuthenticationService encapsulates login/signup/logout logic, token storage responsibilities, and publishes state changes.
- AuthHandler is a typed HTTP message handler that attaches Authorization headers to outgoing requests.
- ViewModels (MVVM) are used for pages so UI naturally reacts to state changes via bindings and Commands.
- All services and view models are registered with DI in MauiProgram/MauiProgramHelper for constructor injection and testability.

3) Patterns used (why & how)
---------------------------
- MVVM: Keeps UI thin and testable. ViewModels expose commands and observable properties; pages bind to them.
- Dependency Injection: Decouples concrete implementations from consumers and enables easier unit testing and lifetime control.
- Service Abstraction: IUserService, IAuthenticationService, and IApiClient separate concerns and provide mockable contracts.
- HTTP Message Handler: AuthHandler centralizes header injection for all outgoing HTTP calls made by ApiClient.
- Messaging / Pub-Sub: Lightweight messages (LoginSignalMessage, LogoutSignalMessage) decouple components that need to react to auth changes.
- Shell Navigation: AppShell and AppShellHelper centralize routing and flyout composition; ShellViewModel drives the dynamic state of the flyout.

4) Files implementing the patterns (mapping)
------------------------------------------
- ToDoUi/ViewModels/ShellViewModel.cs — central auth state and UI properties
- ToDoUi/Services/Implementations/AuthenticationService.cs — auth operations and token handling
- ToDoUi/Networking/Handlers/AuthHandler.cs — sets Authorization header
- ToDoUi/Networking/Implementations/ApiClient.cs, ToDoUi/Networking/Interfaces/IApiClient.cs — HTTP wrapper and generic methods
- ToDoUi/Views/{LoginPage,SignUpPage,TasksPage,AppShell}.xaml(.cs) — UI and bindings
- ToDoUi/Helpers/MauiProgramHelper.cs, ToDoUi/MauiProgram.cs — DI registrations and toolkit setup
- ToDoUi/Messengers/*SignalMessage.cs — login/logout messaging
- ToDoEntityModels/DataContexts/ContextLogger.cs — database/context logging updates

5) Concrete benefits
--------------------
- Single source of truth for authentication state and user information.
- Consistent attachment of bearer tokens to API requests, preventing authorization errors caused by manual header handling.
- Improved testability via interfaces and DI.
- A more responsive UX: flyout menu and navigation adapt to auth state immediately after login/logout.

## UI Modernization
- Modernize UI with card-like Borders and new color themes
  - Defined Beautiful Dark Theme
  - <img width="960" height="540" alt="Screenshot 2026-08-24 211808" src="https://github.com/user-attachments/assets/609f2f31-720e-45b7-bcdd-fddea3db0f11" />
  - Defined Beautiful Light Theme
  - <img width="960" height="540" alt="Screenshot 2026-08-24 212811" src="https://github.com/user-attachments/assets/f9fd8516-7b80-485f-89e3-c09cbbdd7392" />
- You can find All Incredible Themes and Palettes in a Resources folder

