# Diseño: Integración de Asincronismo en Todas las Capas

**Fecha:** 2026-05-29  
**Alcance:** GestionComplejo API — repositorios, servicios, controladores

---

## Contexto

La API actualmente tiene asincronismo parcial: solo `ClimaService`, `ReservaService.CreateAsync` y `ReservaController.CreateAsync` usan `async/await`. Todos los demás repositorios, servicios y controladores son síncronos, bloqueando threads del servidor en cada operación de base de datos.

## Enfoque elegido

**Async completo en todas las capas** — cadena continua de `await` desde el controlador hasta EF Core, sin ningún bloqueo síncrono en el camino.

---

## Sección 1 — Repositorios

### `IBaseRepository<T>`

Todas las firmas cambian a async:

```csharp
Task<List<T>> GetAllAsync();
Task<T?> GetByIdAsync(Guid id);
Task<T> AddAsync(T entity);
Task UpdateAsync(T entity);
Task DeleteAsync(Guid id);
```

### `BaseRepository<T>`

Usa equivalentes async de EF Core:

| Antes | Después |
|-------|---------|
| `ToList()` | `ToListAsync()` |
| `FirstOrDefault()` | `FirstOrDefaultAsync()` |
| `Any()` | `AnyAsync()` |
| `SaveChanges()` | `SaveChangesAsync()` |

El método protegido `SaveChanges()` pasa a `Task SaveChangesAsync()`.

### Interfaces específicas

**`ICanchaRepository`:**
```csharp
Task<Cancha?> AsociarServiciosAsync(Guid canchaId, List<Guid> servicioIds);
Task<Cancha?> AsociarVestuarioAsync(Guid canchaId, Guid vestuarioId);
```

**`IReservaRepository`:**
```csharp
Task<bool> ExisteReservaEnHorarioAsync(Guid canchaId, DateTime inicio, DateTime fin);
```

**`IVestuarioRepository` / `IServicioRepository`:** no tienen métodos propios; heredan todo de `IBaseRepository<T>`.

---

## Sección 2 — Servicios

### `ICanchaService`

```csharp
Task<List<CanchaResponse>> GetAllAsync();
Task<CanchaResponse> GetByIdAsync(Guid id);
Task<CanchaResponse> CreateAsync(CanchaRequest cancha);
Task UpdateAsync(CanchaRequest cancha, Guid id);
Task DeleteAsync(Guid id);
Task<CanchaResponse> AsociarServiciosAsync(Guid canchaId, List<Guid> servicioIds);
Task<CanchaResponse> AsociarVestuarioAsync(Guid canchaId, Guid vestuarioId);
```

### `IServicioService` / `IVestuarioService`

Mismo patrón: `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`.

### `IReservaService`

No cambia — `CreateAsync` ya existe con la firma correcta.

### `IAuthService`

**No se convierte a async.** `SignUp` y `SignIn` usan BCrypt (CPU-bound) y generación de JWT (en memoria). No hay I/O real; agregar `Task` sería overhead sin beneficio.

### Implementaciones

Cada método de servicio agrega `async`/`await` y delega en el repositorio async. Ejemplo:

```csharp
public async Task<CanchaResponse> GetByIdAsync(Guid id)
{
    var cancha = await _canchaRepository.GetByIdAsync(id);
    if (cancha == null) throw new NotFoundException($"No se encontró una cancha con id '{id}'.");
    return cancha.ToCanchaResponse();
}
```

---

## Sección 3 — Controladores

Todos los action methods pasan a `async Task<ActionResult<...>>`. Los métodos se renombran con sufijo `Async` (no afecta las rutas HTTP).

**`CanchaController` (ejemplo):**
```csharp
[HttpGet]
public async Task<ActionResult<List<CanchaResponse>>> GetAllAsync()
{
    var canchas = await _canchaService.GetAllAsync();
    if (!canchas.Any()) return NotFound("No hay canchas registradas.");
    return Ok(canchas);
}
```

**`AuthController`:** no cambia — `IAuthService` sigue siendo síncrono.

**`ReservaController`:** ya es async, no requiere cambios estructurales.

---

## Archivos afectados

| Archivo | Cambio |
|---------|--------|
| `Application/Abstractions/Infrastructure/IBaseRepository.cs` | Todas las firmas a async |
| `Application/Abstractions/Infrastructure/ICanchaRepository.cs` | Métodos específicos a async |
| `Application/Abstractions/Infrastructure/IReservaRepository.cs` | `ExisteReservaEnHorarioAsync` |
| `Application/Abstractions/ICanchaService.cs` | Todas las firmas a async |
| `Application/Abstractions/IServicioService.cs` | Todas las firmas a async |
| `Application/Abstractions/IVestuarioService.cs` | Todas las firmas a async |
| `Infrastructure/Persistance/Repository/BaseRepository.cs` | EF Core async + `SaveChangesAsync` |
| `Infrastructure/Persistance/Repository/CanchaRepository.cs` | Métodos override + específicos async |
| `Infrastructure/Persistance/Repository/ReservaRepository.cs` | `ExisteReservaEnHorarioAsync` |
| `Infrastructure/Persistance/Repository/VestuarioRepository.cs` | Hereda de BaseRepository (sin cambios propios) |
| `Infrastructure/Persistance/Repository/ServicioRepository.cs` | Hereda de BaseRepository (sin cambios propios) |
| `Application/Services/CanchaService.cs` | Todos los métodos async |
| `Application/Services/ServicioService.cs` | Todos los métodos async |
| `Application/Services/VestuarioService.cs` | Todos los métodos async |
| `Presentation/Controllers/CanchaController.cs` | Todos los actions async |
| `Presentation/Controllers/ServicioController.cs` | Todos los actions async |
| `Presentation/Controllers/VestuarioController.cs` | Todos los actions async |

**No afectados:** `IAuthService`, `AuthService`, `AuthController`, `IClimaService`, `ClimaService`, `ReservaService`, `ReservaController`.
