# GymApp

REST API for a gym exercise catalogue, built with ASP.NET Core and a layered
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

`Exercise` keeps the name of the movement, calories burned per hour, whether
equipment is required, the target muscle group, sets, reps and the creation
date. The exercise name is unique.

## Projects

| Project | Responsibility |
| --- | --- |
| `Domain` | Entities and domain rules, no external dependencies |
| `Application` | Business logic and repository abstractions |
| `Infrastructure` | Repository implementations |
| `Api` | Controllers, DTOs and application setup |

## Run

```bash
dotnet run --project src/Api
```
