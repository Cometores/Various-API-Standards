# Minimal API
API for getting and managing information about dishes and their ingredients.

Consist of three tables in SQLite:
```Mathematics
+-----------------+            +-------------------------------------+             +-------------------+ 
|     Dishes      |            |           DishIngredient            |             |    Ingredients    |
+-----------------+            +-------------------------------------+             +-------------------+ 
| Id (PK)         |◄───────────| DishesId (FK → Dishes.Id)           |             | Name              |
| Name            |            | IngredientId (FK → Ingredients.Id)  |───────────► | Id (PK)           |
+-----------------+            +-------------------------------------+             +-------------------+
```

**Types of endpoints implemented:**</br>
&emsp;`get`, `post`, `put`, `delete`

## Start Exploring
Run `MinimalAPI: http` profile. I recommend using Rider, as you can run this line from there and use their 
database representation configured for Rider. </br>
Use **Swagger** accessible at: [http://localhost:5080/swagger/index.html](http://localhost:5080/swagger/index.html)
or **Postman** collection to test the API.

## Postman
Import the collection from `Minimal.API.postman_collection.json` [file](Minimal.API.postman_collection.json).
After that you need to authenticate via CLI.

### Auth
JWT-Bearer Token is used to perform the authorization. To create such a token, use the CLI command inside the
MinimalAPI project folder:
<br />`dotnet user-jwts create --audience MinimalAPI`
<br />The command will create a token with the following parameters:
- audience: **MinimalAPI**
- valid issuer: **dotnet-user-jwts**
- name: **your computer name**

*You can see what's inside the generated token on this [website](https://jwt.io/) or via CLI command:*
<br />`dotnet user-jwts print <TokenID>`

<br />

Use the generated token in the request authorization tab or on the Postman collection itself:
![SSL](../../docs/PostmanBearer_MinimalAPI.png)

The token parameters are checked by the API using the Microsoft JwtBearer package configuration settings, which are
taken from [appsettings.json "Authentication"](appsettings.json).

<br />

### Authorization policy
For the `POST /dishes` method you need to have the **admin role** and the claim **country=Germany**:
<br />`dotnet user-jwts create --audience MinimalAPI --claim country=Germany --role admin`

## What makes it Minimal?
- No Controllers, no MVC pipeline.
- Endpoints defined via `app.MapGet/MapPost/`...
- Dependency injection directly into handler parameters.
- [Filters](#endpoint-filters) added inline via `.AddEndpointFilter<...>()` instead of attributes.

## Endpoint filters
In ASP.NET Core Minimal APIs, EndpointFilter is similar in spirit to MVC’s ActionFilter, but designed specifically for 
the lightweight Minimal API model. Instead of attributes, filters are attached in the route builder 
(e.g. `.AddEndpointFilter<T>()`). They allow cross-cutting concerns (validation, logging, access rules) 
without cluttering the handler logic.

### Filters in this project:
1) **ValidateAnnotationsFilter**<br />
   &emsp;Validates DTOs against DataAnnotations using MiniValidation.<br />
   &emsp;If validation fails → returns ValidationProblem(400).<br />
   &emsp;Usage: POST /dishes.<br /><br />

2) **DishIsLockedFilter** <br />
   &emsp;Checks whether a given dishId is marked as “locked”.<br />
   &emsp;If locked → short-circuits with Problem(400).<br />
   &emsp;Usage: PUT/DELETE /dishes/{dishId}.<br /><br />

3) **LogNotFoundResponseFilter**
   &emsp;Executes after the handler.<br />
   &emsp;If result is 404 NotFound, logs the request path.<br />
   &emsp;Usage: DELETE /dishes/{dishId}.