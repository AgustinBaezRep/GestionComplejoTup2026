# CI/CD Pipeline — GestionComplejo · Azure App Service

## Resumen

Pipeline de integración y despliegue continuo usando GitHub Actions. Se activa en cada push (merge) a `master` y despliega automáticamente la API .NET 10 a Azure App Service, incluyendo migraciones de EF Core contra Azure SQL Database.

---

## Trigger

- **Evento**: `push` a la rama `master`
- **Alcance**: incluye merges de Pull Requests hacia master
- **Ambientes**: un único ambiente de producción

---

## Estructura del Workflow

Archivo: `.github/workflows/deploy.yml`

### Job 1: `build` (ubuntu-latest)

| Paso | Acción | Comando |
|------|--------|---------|
| 1 | Checkout | `actions/checkout@v4` |
| 2 | Setup .NET 10 | `actions/setup-dotnet@v4` |
| 3 | Restore | `dotnet restore` |
| 4 | Build | `dotnet build --no-restore --configuration Release` |
| 5 | Test | `dotnet test --no-build --configuration Release` |
| 6 | Publish | `dotnet publish GestionComplejo.Presentation --configuration Release -o ./publish` |
| 7 | Upload artifact | `actions/upload-artifact@v4` (nombre: `app`) |

### Job 2: `deploy` (ubuntu-latest) — `needs: build`

| Paso | Acción | Comando |
|------|--------|---------|
| 1 | Checkout | `actions/checkout@v4` (necesario para dotnet-ef) |
| 2 | Download artifact | `actions/download-artifact@v4` |
| 3 | Setup .NET 10 | `actions/setup-dotnet@v4` |
| 4 | Login Azure | `azure/login@v2` con `AZURE_CREDENTIALS` |
| 5 | Instalar dotnet-ef | `dotnet tool install --global dotnet-ef` |
| 6 | Migraciones EF | `dotnet ef database update --project GestionComplejo.Infrastructure --startup-project GestionComplejo.Presentation` con `ConnectionStrings__GestionComplejoConnectionString` seteada como env var desde `${{ secrets.CONNECTION_STRING }}` |
| 7 | Deploy App Service | `azure/webapps-deploy@v3` |

El job `deploy` solo se ejecuta si `build` completó exitosamente. Si algún test falla, el deploy nunca ocurre.

---

## GitHub Secrets

Configurar en: GitHub → Settings → Secrets and variables → Actions

| Secret | Descripción | Origen |
|--------|-------------|--------|
| `AZURE_CREDENTIALS` | JSON del Service Principal de Azure | `az ad sp create-for-rbac` al crear recursos Azure |
| `AZURE_WEBAPP_NAME` | Nombre del App Service | Definido al crear el App Service en Azure |
| `CONNECTION_STRING` | Connection string de Azure SQL Database | Azure Portal → SQL Database → Connection strings |
| `JWT_KEY` | Clave secreta para tokens JWT (mín. 32 chars) | Clave generada manualmente y guardada aquí |

---

## Variables de entorno en Azure App Service

ASP.NET Core lee las App Settings de Azure como variables de entorno. El separador `__` mapea secciones anidadas del JSON.

| App Setting en Azure | Sección en appsettings.json |
|----------------------|----------------------------|
| `ConnectionStrings__GestionComplejoConnectionString` | Connection string SQL |
| `Jwt__Key` | Clave secreta JWT |
| `Jwt__Issuer` | `GestionComplejo` |
| `Jwt__Audience` | `GestionComplejoClients` |
| `Clima__BaseUrl` | `https://api.open-meteo.com` |

---

## Recursos Azure necesarios (a crear manualmente)

> Los siguientes recursos deben crearse en Azure antes de activar el pipeline. Este apartado es una guía de referencia.

- **Resource Group**: contenedor lógico para todos los recursos
- **Azure SQL Server + Database**: reemplaza la base de datos local; plan Basic (~5 USD/mes) es suficiente para desarrollo
- **App Service Plan**: plan B1 (Basic) o F1 (Free, con limitaciones) para alojar la API
- **App Service** (Web App): configurado con runtime `.NET 10`, sistema operativo Linux
- **Service Principal**: identidad con rol `Contributor` sobre el Resource Group, genera el JSON para `AZURE_CREDENTIALS`

---

## Flujo completo

```
Developer → git push master
    → GitHub detecta push
    → Job: build
        → checkout → setup .NET → restore → build → test → publish → upload artifact
    → Job: deploy (si build pasó)
        → checkout → download artifact → setup .NET
        → azure/login (Service Principal)
        → dotnet ef database update (Azure SQL)
        → azure/webapps-deploy (App Service)
    → API en producción actualizada
```

---

## Archivos a crear

- `.github/workflows/deploy.yml` — workflow principal
- `.gitignore` — agregar `.superpowers/` si no está

---

## Decisiones de diseño

- **2 jobs separados** (build + deploy) en lugar de 1 solo: permite ver claramente dónde falla el pipeline y reutilizar el artifact entre jobs.
- **dotnet test incluido** aunque no haya tests: facilita agregar tests en el futuro sin tocar el workflow.
- **Migraciones en el job de deploy**: se ejecutan contra la base de datos real justo antes del deploy, garantizando que el schema esté actualizado cuando la nueva versión de la API arranque. El comando requiere `--project GestionComplejo.Infrastructure --startup-project GestionComplejo.Presentation` (igual que en desarrollo local) y recibe la connection string vía variable de entorno `ConnectionStrings__GestionComplejoConnectionString` inyectada desde el secret `CONNECTION_STRING`.
- **GitHub Secrets para toda configuración sensible**: la connection string, JWT key y credenciales de Azure nunca aparecen en el código fuente.
