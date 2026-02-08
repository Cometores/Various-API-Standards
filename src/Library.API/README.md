# Library API
API for managing authors and their books.

**Types of endpoints implemented:** </br>
&emsp;`get`, `post`, `put`, `patch`

Consists of two tables in SQLite:
```Mathematics
+-------------------+            +-------------------------+
|      Authors      |            |          Books          |
+-------------------+            +-------------------------+
| Id (PK)           |◄───────────| AuthorId (FK → Authors) |
| FirstName         |            | Id (PK)                 |
| LastName          |            | Title                   |
|                   |            | Description             |
|                   |            | AmountOfPages           |
+-------------------+            +-------------------------+
```

## Start exploring
Run `Library.API` profile. The API uses SQLite:
`Data Source=LibraryAPI.db`.

You can use Swagger or any HTTP client.

### Swagger
Accessible at [http://localhost:5251](http://localhost:5251).

## Core endpoints
Base route: `/api`

### Authors
- `GET /api/authors`
- `GET /api/authors/{authorId}`
- `PUT /api/authors/{authorId}`
- `PATCH /api/authors/{authorId}`

### Books
- `GET /api/authors/{authorId}/books`
- `GET /api/authors/{authorId}/books/{bookId}`
- `POST /api/authors/{authorId}/books`

## Data access

### Data Transfer Objects
- Models in `Models/` are used to shape request/response payloads.
- Mapping is handled with AutoMapper profiles.

### Repository Pattern
- `IAuthorRepository` and `IBookRepository` abstract data access.
- Operations include:
  - Get all authors
  - Get author by ID
  - Get books for author
  - Get book by ID
  - Create book
  - Update author

### Entity Framework Core
- Uses SQLite database `LibraryAPI.db`.
- Entities:
  - `Author`
  - `Book`

## Services
**Services used:**
- `JsonPatch` - to process `PATCH` endpoints
- `NewtonsoftJson` formatter - to support JSON Patch and camelCase serialization
- `AutoMapper` - mapping between entities and DTOs
- Swagger (`Swashbuckle`) - interactive API documentation
