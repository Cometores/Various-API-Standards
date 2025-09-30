# CRUD API
API for getting and managing simple information about cities and their points of interest.

**Types of endpoints implemented:** </br>
&emsp;`get`, `post`, `put`, `patch`, `delete`

Consists of two tables in SQLite:
```Mathematics
+-------------------+            +-------------------------+
|      Cities       |            |    PointsOfInterest     |
+-------------------+            +-------------------------+
| Id (PK)           |◄───────────| CityId (FK → Cities.Id) |
| Name              |            | Id (PK)                 |
| Description       |            | Name                    |
|                   |            | Description             |
+-------------------+            +-------------------------+
```


## Start exploring
Run `CRUD.API` profile. I recommend using Rider, as you can run this line from there and use their database representation
configured for Rider.

You can use either [Swagger](#swagger) or [Postman](#postman) to play with the API. I would recommend using
**Postman** collection, because it's automated and you don't need to worry about versioning or authentication.


### Postman
Import the collection from `CRUD.API.postman_collection.json` [file](CRUD.API.postman_collection.json).
You need to run authentication first, and token will be automatically stored to the collection variables.
Use the requests in the given order, as `PUT`, `PATCH` and `DELETE` will work with your created data.


### Swagger
Accessible at [http://localhost:7126/swagger](https://localhost:7126/swagger/index.html).

You need to authenticate first.
- Which password and username you choose are not important.

Think about choosing correct API version and correct media type (for example, `application/json`).


## Data access

The application implements a layered architecture for data access using the following patterns and technologies:

### Data Transfer Objects

- Used to shape the data that is exposed through the API
- Prevents exposing internal entity models directly
- Examples:
    - `CityDto` - for retrieving cities with their points of interest
    - `CityWithoutPointsOfInterestDto` - for retrieving cities without related data

### Repository Pattern

- Abstracts the data persistence logic
- Provides interface for database operations
- Implements common operations:
    - Get all entities
    - Get entity by ID
    - Create new entity
    - Update existing entity
    - Delete entity

### Entity Framework Core

- Uses SQLite as the database provider
- Entities:
    - `City` - represents city information
    - `PointOfInterest` - represents points of interest within cities
- Configured using Data Annotations for:
    - Primary keys
    - Foreign key relationships
    - Required fields
    - Field length constraints

## Services
**Services used:**
- for PATCH:
  - `JsonPatch` - to process the **patch** endpoint
  - `NewtonsoftJson` formatter - to serialize a JsonPatch document
- for Files: 
  - `FileExtensionContentTypeProvider` - to create a `Content-Type` field for specific files
- Output XML serialization: `XmlDataContractSerializerFormatters`
- Logging -`Serilog`
    - `serilog.sinks.file`
    - `serilog.sinks.console`
- Dummy mail service — custom-created service
    - different implementations for development and production
    - mail addresses stored in configuration file