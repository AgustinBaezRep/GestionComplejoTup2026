# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run API (from Presentation project)
dotnet run --project GestionComplejo.Presentation

# Add EF Core migration
dotnet ef migrations add <MigrationName> --project GestionComplejo.Infrastructure --startup-project GestionComplejo.Presentation

# Apply migrations
dotnet ef database update --project GestionComplejo.Infrastructure --startup-project GestionComplejo.Presentation
```

API documentation is available at `/swagger` when the app is running.

## Architecture

Clean Architecture with 4 layers, each in its own .NET project:

**Domain** → **Application** → **Infrastructure** → **Presentation**

- **Domain**: Entities only. `BaseEntity` provides `Id` (Guid), `IsDeleted`, `UpdatedDateTime`, `DeletedDateTime`. No external dependencies.
- **Application**: Service interfaces and implementations (business logic), DTOs (`*Request`/`*Response`), mapper extension methods, and custom exception types. Depends on Domain.
- **Infrastructure**: `GestionComplejoDbContext` (EF Core + SQL Server), `BaseRepository<T>` (generic CRUD with built-in soft delete), specific repository implementations, `AuthService` (JWT + BCrypt), `ClimaService` (external HTTP). Depends on Application.
- **Presentation**: ASP.NET Core controllers, middleware (`ExceptionHandlingMiddleware`), DI wiring (`Program.cs`). Depends on Application and Infrastructure.

## Domain Entities

- **Usuario** (abstract TPC hierarchy): `Nombre`, `Apellido`, `Email`, `Contrasena` (BCrypt-hashed), `Telefono`
  - **Cliente** — no additional fields
  - **Admin** — adds `Cargo`
- **Cancha**: `Nombre`, `Deporte`, `Capacidad`, `Precio`, optional one-to-one `Vestuario`, many-to-many `Servicios` (join table `CanchaServicio`)
- **Vestuario**: `NumeroVestuarios`, `TieneDuchas`, `Capacidad`, FK `CanchaId`
- **Servicio**: `Nombre`, `Descripcion`, `CostoAdicional`, many-to-many `Canchas`
- **Reserva**: FK `ClienteId`, FK `CanchaId`, `FechaInicio`, `FechaFin` (auto +1h), `Estado` ("Pendiente"/"Confirmada"), `PrecioTotal` (copied from cancha at creation)

## Key Patterns

**Soft Delete**: `BaseRepository.Delete()` sets `IsDeleted = true` and `DeletedDateTime`; all queries filter out `IsDeleted == true`. Never hard-delete entities.

**Repository pattern**: `IBaseRepository<T>` and feature-specific interfaces (`ICanchaRepository`, `IReservaRepository`, `IVestuarioRepository`, `IServicioRepository`) are defined in Application and implemented in Infrastructure. The generic base is registered as open generic: `AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))`.

**DTOs**: Request DTOs for input, Response DTOs returned from controllers. Mapping via extension methods in `Application/Mapper/*Mapper.cs` files (e.g., `ToCancha()`, `ToCanchaResponse()`).

**Exception handling**: Throw typed exceptions from services; `ExceptionHandlingMiddleware` maps them to HTTP status codes — `ValidationException`→400, `NotFoundException`→404, `ConflictException`→409, `UnauthorizedException`→401, `DatabaseException`→500.

**Authorization**: Two policies — `SoloAdmin` (role "Admin") and `SoloCliente` (role "Cliente"). Read endpoints use plain `[Authorize]`; write/delete endpoints use `[Authorize(Policy: "SoloAdmin")]`.

## Business Logic Notes

**Reservation creation** (`ReservaService.CreateAsync`):
1. Validates cancha exists.
2. Calls `ClimaService.EsLluviosoAsync` (Open-Meteo API, coords fixed to Tucumán) — throws `ConflictException` if rainy.
3. Checks for time-slot conflicts via `IReservaRepository.ExisteReservaEnHorario` — throws `ConflictException` if overlap exists.
4. End time is always start time + 1 hour; price is copied from the cancha; status starts as "Pendiente".

**Authentication** (`AuthService` in Infrastructure): BCrypt password hashing, JWT tokens with claims `Sub` (UserId), `Email`, `Role`, `Jti`, `Iat`. Configured via `appsettings.json` under `Jwt` (Key, Issuer, Audience, ExpirationMinutes).

## Database

SQL Server via Entity Framework Core 10. Connection string key: `GestionComplejoConnectionString` in `appsettings.json` (must be configured locally). Usuario hierarchy uses Table Per Concrete Type (TPC). External weather API base URL configured under `Clima.BaseUrl`.
