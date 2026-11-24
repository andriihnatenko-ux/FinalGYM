# GymApp

REST API for a gym training catalogue, built with ASP.NET Core and a layered
architecture:

```
Api ──► Infrastructure ──► Application ──► Domain
```

## Theme

The domain is a gym training catalogue.

| Role | Entity |
| --- | --- |
| Main entity | `Exercise` |
| Group entity | `MuscleGroup` |
| Link entity | `ExerciseMuscleGroup` |
| Attachment entity | `ExerciseImage` |
| Owned value object | `ExerciseAttributes` (stored as `jsonb`) |

An `Exercise` has a unique title, a description, calories burned per hour,
an equipment flag, sets, reps, an owned `Attributes` value object and belongs
to one or more `MuscleGroup`s. Images uploaded for an exercise are tracked as
`ExerciseImage` attachments and stored on disk under `uploads/{exerciseId}`.

`ExerciseAttributes` is a value object owned by `Exercise`. It holds the
difficulty, required equipment and free-form notes and is persisted as a
single PostgreSQL `jsonb` column.

## Projects

| Project | Responsibility |
| --- | --- |
| `Domain` | Entities, value objects and domain rules, no external dependencies |
| `Application` | Business logic, service and repository abstractions |
| `Infrastructure` | EF Core persistence, file storage and notifications |
| `Api` | Controllers, DTOs and application setup |

## Persistence

PostgreSQL is accessed through EF Core with snake_case naming. The connection
string lives in `src/Api/appsettings.Development.json` under
`ConnectionStrings:DefaultConnection`. Migrations are applied automatically on
startup.

```bash
dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api --output-dir Persistence/Migrations
```

## Endpoints

### Exercises

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/exercises` | List exercises with muscle groups and images |
| `GET` | `/exercises/{id}` | Get a single exercise |
| `POST` | `/exercises` | Create an exercise |
| `PUT` | `/exercises/{id}` | Update an exercise |
| `DELETE` | `/exercises/{id}` | Delete an exercise |
| `POST` | `/exercises/{id}/images` | Upload an image attachment (`multipart/form-data`) |

### Muscle groups

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/muscle-groups` | List muscle groups |
| `GET` | `/muscle-groups/{id}` | Get a single muscle group |
| `POST` | `/muscle-groups` | Create a muscle group |

Creating or updating an exercise requires at least one existing muscle group
id. Duplicate titles and duplicate muscle group names return `409`.

## Run

```bash
dotnet run --project src/Api
```

Swagger UI is available in the Development environment at `/swagger`.
