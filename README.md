# Overload
A work-out tracker app, made for myself

Progressive overload tracker met grafieken per oefening. Backend in ASP.NET Core (C#) met PostgreSQL, frontend in Angular (nog te bouwen).

## Inhoud

- [Stack](#stack)
- [Projectstructuur](#projectstructuur)
- [Alles starten met Docker](#alles-starten-met-docker)
- [Lokaal ontwikkelen in Rider](#lokaal-ontwikkelen-in-rider)
- [Database en migraties](#database-en-migraties)
- [Poorten en URL's](#poorten-en-urls)
- [Controllers en services](#controllers-en-services)
- [Problemen oplossen](#problemen-oplossen)

## Stack

| Onderdeel | Keuze |
|---|---|
| Backend | ASP.NET Core Web API, C#, .NET 10 |
| ORM | Entity Framework Core (Code First) |
| Database | PostgreSQL 17 |
| Frontend | Angular (gepland) |
| Auth | Azure AD B2C (gepland) |
| Containers | Docker Compose |

## Projectstructuur

```
Overload/
  docker-compose.yml        db + api in één keer
  .env.example              voorbeeld voor het wachtwoord
  backend/
    Dockerfile
    Overload.slnx
    Overload.Domain/        entities (geen ASP.NET- of EF-code)
      Entities/             Exercise, WorkoutSession, SetEntry
    Overload.Api/           controllers, services, DbContext, Program.cs
      Controllers/
      Services/             IExerciseService, ISetEntryService, ...
      Models/Requests/      request-records (DTO's)
      Data/                 OverloadDbContext
      Migrations/
  frontend/
```

Afhankelijkheden lopen één kant op: `Overload.Api` verwijst naar `Overload.Domain`, niet andersom.

## Alles starten met Docker

Database en API tegelijk:

```bash
docker compose up --build
```

- De API wacht tot Postgres gezond is en past bij het opstarten automatisch de migraties toe (`MigrateAsync()` in `Program.cs`). Je hoeft dus geen `dotnet ef database update` te draaien.
- API: <http://localhost:5226/api/exercise>
- OpenAPI-document (alleen in Development): <http://localhost:5226/openapi/v1.json>

Handige commando's:

| Doel | Commando |
|---|---|
| Op de achtergrond starten | `docker compose up -d --build` |
| Logs volgen | `docker compose logs -f api` |
| Stoppen | `docker compose down` |
| Stoppen en database wissen | `docker compose down -v` |
| Alleen de database starten | `docker compose up -d db` |
| Opnieuw bouwen na codewijziging | `docker compose up --build api` |

Wachtwoord aanpassen: kopieer `.env.example` naar `.env` en wijzig `POSTGRES_PASSWORD`. Het wachtwoord wordt bij het eerste aanmaken van het volume vastgelegd. Wijzig je het later, doe dan eerst `docker compose down -v`.

Heb je eerder een losse container gestart met `docker run --name overload-db`? Verwijder die eerst, anders botst de naam en poort 5432: `docker rm -f overload-db`.

## Lokaal ontwikkelen in Rider

Handig voor debuggen met breakpoints: alleen de database in Docker, de API in Rider.

1. Database starten: `docker compose up -d db`
2. Connection string eenmalig opslaan als user secret (vanuit de map `backend`):

   ```bash
   dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=overload;Username=overload;Password=dev" --project Overload.Api
   ```

3. API starten in Rider met het profiel `http`. Hij luistert dan op <http://localhost:5226>.

Draai niet tegelijk de API in Rider en de `api`-container: ze gebruiken dezelfde poort 5226.

## Database en migraties

Het schema volgt de entities in `Overload.Domain`. Een entity wijzigen gaat zo (vanuit de map `backend`):

```bash
dotnet ef migrations add NaamVanDeWijziging --project Overload.Api
```

De nieuwe migratie wordt bij de volgende start van de API toegepast. Commit de map `Migrations/` mee.

Handmatig toepassen kan ook: `dotnet ef database update --project Overload.Api`.

### Tabellen

| Tabel | Belangrijkste kolommen |
|---|---|
| `Exercises` | `Id`, `Name`, `Category`, `IsCustom`, `CreatedByUserId` |
| `WorkoutSessions` | `Id`, `UserId`, `Date`, `Notes` |
| `SetEntries` | `Id`, `SessionId` (FK), `ExerciseId` (FK), `SetNumber`, `Weight`, `Reps`, `Rpe`, `IsWarmup` |

Een sessie heeft veel sets, een oefening komt in veel sets voor. De foreign keys staan aan de kant van `SetEntries`.

Database bekijken in Rider: Database-paneel, `+`, Data Source, PostgreSQL, host `localhost`, poort `5432`, database `overload`, user `overload`.

## Poorten en URL's

| Wat | Waar | Poort |
|---|---|---|
| API via Docker | <http://localhost:5226> | 5226 (container 8080) |
| API via Rider, profiel `http` | <http://localhost:5226> | 5226 |
| API via Rider, profiel `https` | <https://localhost:7259> | 7259 |
| PostgreSQL | `localhost` | 5432 |

Poort 5432 is de database en spreekt geen HTTP. Een request daarheen geeft in de logs `invalid length of startup packet`.

## Controllers en services

Een controller regelt alleen HTTP (routes, statuscodes). De logica en database-aanroepen zitten in een service achter een interface. De koppeling gebeurt met dependency injection in `Program.cs`:

```csharp
builder.Services.AddScoped<IExerciseService, ExerciseService>();
```

De route komt uit de klassenaam: `[Route("api/[controller]")]` op `ExerciseController` geeft `/api/exercise`. Routes zijn niet hoofdlettergevoelig.

## Problemen oplossen

| Melding | Oorzaak en oplossing |
|---|---|
| `ECONNREFUSED 127.0.0.1:7259` | Er luistert niets op de https-poort. Gebruik <http://localhost:5226> of start het profiel `https`. |
| `invalid length of startup packet` | Je stuurt HTTP naar poort 5432. Gebruik poort 5226. |
| `password authentication failed` | Wachtwoord in de connection string klopt niet met dat van de database. Zie hierboven bij wachtwoord aanpassen. |
| `Failed to connect to ... 5432` | Database draait niet. Controleer met `docker ps`. |
| `Type 'IQueryable<...>' is not awaitable` | Gebruik `ToListAsync()` (en `using Microsoft.EntityFrameworkCore;`) in plaats van alleen `await` op een query. |
| `Could not find a MSBuild project file` | Je zit in de verkeerde map. Voeg `--project Overload.Api` toe of ga naar `backend/Overload.Api`. |
| Waarschuwing `Failed to determine the https port for redirect` in Docker | Onschadelijk: in de container is alleen http actief. |
