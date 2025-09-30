# Examples of different API standards
Comprehensive examples of various API types with configured services. They can be used for prototyping or as 
a basis for creating a new API. For a detailed description of the services 
and architecture, see the corresponding documentation file.

APIs described in the repository:
- [Basic CRUD API](#basic-crud-api)
- [Minimal API](#minimal-api)
- [Restful API](#restful-api)
- [OData API](#odata-api)
- [API with Unit Testing](#unit-testing)

Postman collections with tests are also attached to the API's folders.



## Basic CRUD API
API for getting and managing simple information about cities and their points of interest.</br>
More information about the API can be found in the [documentation file](./CRUD.API/README.md).

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
More information about the API can be found in the [documentation file](./MinimalAPI/README.md).

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
Inspired by
[Kevin's Dockx](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-deep-dive/table-of-contents)
course from pluralsight.


## OData API
Inspired by
[Kevin's Dockx](https://app.pluralsight.com/library/courses/building-consistent-restful-api-odata-v4-asp-dot-net-core/table-of-contents)
course from pluralsight.


## Unit testing
Testing API with xUnit.
Inspired by
[Kevin's Dockx](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-unit-testing/table-of-contents)
course from pluralsight.

SQLite as Database.


## Postman Collections
If you encounter a certificate issue when submitting a request, disable certificate validation in the settings:
![SSL](./others/PostmanSSL.png)