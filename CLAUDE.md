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
- **Application**: Services (business logic), DTOs (`*Request`/`*Response`), and mapper extension methods. Depends on Domain.
- **Infrastructure**: `GestionComplejoDbContext` (EF Core + SQL Server), `BaseRepository<T>` (generic CRUD with built-in soft delete), specific repository implementations. Depends on Application.
- **Presentation**: ASP.NET Core controllers, DI wiring (`Program.cs`). Depends on Application and Infrastructure.

## Key Patterns

**Soft Delete**: `BaseRepository.Delete()` sets `IsDeleted = true` and `DeletedDateTime`; all queries filter out `IsDeleted == true`. Never hard-delete entities.

**Repository pattern**: `IBaseRepository<T>` (generic) and feature-specific interfaces (e.g., `ICanchaRepository`) are defined in Application and implemented in Infrastructure.

**DTOs**: Request DTOs are used for input (create/update), Response DTOs are returned from controllers. Mapping is done via extension methods in `*Mapper.cs` files under Application.

## Database

SQL Server via Entity Framework Core 10. Connection string key: `GestionComplejoConnectionString` in `appsettings.json` (must be configured locally).

## Current Domain

Single entity: **Cancha** (sports court) with fields `Nombre`, `Deporte`, `Capacidad`, `Piso`, `Precio`. REST endpoints under `api/cancha`.
