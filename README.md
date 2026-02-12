# Examples of different API standards
Comprehensive examples of various API types with configured services. They can be used for prototyping or as 
a basis for creating a new API. For a detailed description of the services 
and architecture, see the corresponding documentation file.

APIs described in the repository:
- [CRUD API](#crud-api)
- [Minimal API](#minimal-api)
- [Restful API](#restful-api)
- [OData API](#odata-api)
- [API with Unit Testing](#testing-an-api)

Postman collections with tests are also attached to the API's folders.




## CRUD API
API for getting and managing simple information about cities and their points of interest.</br>
More information about the API can be found in the [documentation file](./src/CRUD.API/README.md).

Key features:
- **Authentication** & **Authorization**
- **Swagger** documentation
- **Logging** into file and console
- **XML** and **JSON** serialization
- **Dummy mail service**
- **SQLite** database
- **API versioning**
- **Filtering** & **searching**
- **Paging**
- **File** upload




## Minimal API
API for getting and managing information about dishes and their ingredients.</br>
More information about the API can be found in the [documentation file](./src/Minimal.API/README.md).

Key features:
- **Minimal endpoints** setup
- creating auth token with `.net cli`
- **JWT-Bearer Token** authentication
- **Role-based** authorization
- **SQLite** database
- recreating Database on each run
- **Logging**
- **Route grouping**
- **Filtering**
- **Custom parameter binding**
- **Error handling** middleware




## Restful API
API for authors and courses with HATEOAS, data shaping, filtering, paging.</br>
More information about the API can be found in the [documentation file](./src/Restful.API/README.md).

Key features:
- **HATEOAS** links + `X-Pagination` metadata
- **Data shaping** via `fields` & safe sorting via `orderBy`
- **Filtering**, **searching**, **paging**
- **JSON Patch** and **XML** formatters
- **SQLite** database, reset & migrated at startup




## OData API
More information about the API can be found in the [documentation file](./src/OData.API/README.md).

Key features:
- OData v4 with **functions**, **actions**, **singleton**
- **$select**, **$expand**, **$filter**, **$orderby**, **$count**
- Contained navigation: People → VinylRecords
- **SQL Server LocalDB** with seed data




## Testing an API
Consists of two projects - API itself and tests on xUnit.</br>
More information about the API can be found in the [documentation file](./src/EmployeeManagement/README.md).
Testing API with xUnit.

Key features:
- xUnit
- SQLite




## Postman Collections
If you encounter a certificate issue when submitting a request, disable certificate validation in the settings:
![SSL](./docs/PostmanSSL.png)
