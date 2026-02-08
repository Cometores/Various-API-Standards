# CourseLibrary API
API for managing authors and their courses (HATEOAS + data shaping).

**Types of endpoints implemented:** </br>
&emsp;`get`, `head`, `post`, `put`, `patch`, `delete`

Consists of two tables in SQLite:
```Mathematics
+-------------------+            +-------------------------+
|      Authors      |            |         Courses         |
+-------------------+            +-------------------------+
| Id (PK)           |◄───────────| AuthorId (FK → Authors) |
| FirstName         |            | Id (PK)                 |
| LastName          |            | Title                   |
| DateOfBirth       |            | Description             |
| MainCategory      |            |                         |
+-------------------+            +-------------------------+
```

## Start exploring
Run `CourseLibrary.API` profile. The API uses SQLite (`Data Source=library.db`) and automatically drops & re-applies migrations on startup (`ResetDatabaseAsync`).

Base route: `/api`. Default formatters: JSON (camelCase) and XML. Accept header matters (`ReturnHttpNotAcceptable = true`).

Examples:
```http
GET http://localhost:5000/api/authors?pageNumber=1&pageSize=5&orderBy=Name
GET http://localhost:5000/api/authors?mainCategory=Rum&fields=FirstName,MainCategory
GET http://localhost:5000/api/authors/{authorId}
GET http://localhost:5000/api/authors/{authorId}/courses
```

`X-Pagination` response header carries paging metadata. Author payloads include HATEOAS links; `fields` enables data shaping.

## Core endpoints
### Root
- `GET /api` – returns top-level links

### Authors
- `GET /api/authors` (supports `mainCategory`, `searchQuery`, `orderBy`, `fields`, `pageNumber`, `pageSize`)
- `HEAD /api/authors`
- `GET /api/authors/{authorId}?fields=...`
- `POST /api/authors`
- `OPTIONS /api/authors`

### Author collections
- `GET /api/authorcollections({id1,id2,...})`
- `POST /api/authorcollections`

### Courses (per author)
- `GET /api/authors/{authorId}/courses`
- `GET /api/authors/{authorId}/courses/{courseId}`
- `POST /api/authors/{authorId}/courses`
- `PUT /api/authors/{authorId}/courses/{courseId}` (upsert)
- `PATCH /api/authors/{authorId}/courses/{courseId}`
- `DELETE /api/authors/{authorId}/courses/{courseId}`

## Data access

### Entity Framework Core
- SQLite database `library.db`, seeded with authors and courses.
- Entities: `Author`, `Course`.
- Migration applied on startup; database reset for a clean demo each run.

### Repository pattern
- `ICourseLibraryRepository` encapsulates CRUD for authors and courses.
- Supports paging, filtering, searching, sorting, data shaping, and HATEOAS link generation helpers.

## Services
**Services used:**
- `JsonPatch` + `NewtonsoftJson` formatter for PATCH.
- XML formatter (`AddXmlDataContractSerializerFormatters`).
- `AutoMapper` for entity/DTO mapping.
- `IPropertyMappingService` / `IPropertyCheckerService` for safe sorting and data shaping.
- Custom validation problem details (422) via `InvalidModelStateResponseFactory`.
