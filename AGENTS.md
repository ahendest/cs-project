# AGENTS

## Overview
This repository is a layered .NET Web API.
- `cs-project.Core` contains entities, DTOs, interfaces and common models.
- `cs-project.Infrastructure` implements data access, repositories, and services.
- `cs-project` hosts the API with controllers and program startup.

## Code Style
- Use the `cs_project` root namespace and keep folders consistent with project structure.
- Indent using **4 spaces**. Place braces on new lines.
- Group `using` directives at the top of the file; system/usings first, then project references. Remove unused directives.
- Name asynchronous methods with the `Async` suffix and return `Task`/`Task<T>`.
- Prefer expression-bodied members for simple getters or methods, e.g. `public void Update(Pump pump) => _context.Update(pump);`.
- Initialize required navigation properties with `= null!;` where needed for EF Core.
- When querying with EF Core, access data asynchronously (e.g. `ToListAsync`, `SaveChangesAsync`).

## Practices
- Controllers are annotated with `[ApiController]`, `[Route("api/[controller]")]`, and `[Authorize]`. Inject services and `ILogger<T>` through constructors.
- Repositories encapsulate `AppDbContext` and return domain entities. Services orchestrate repositories and map to DTOs using AutoMapper.
- Entities inherit from `BaseEntity` which provides `Id`, `CreatedAtUtc`, `UpdatedAtUtc`, `IsActive`, and `RowVersion` for concurrency.

## Tests
After making changes run:
1. `dotnet build`
2. `dotnet test`

Both commands should succeed before committing.
