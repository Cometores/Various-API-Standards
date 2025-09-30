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





## Basic CRUD API
API for getting and managing simple information about cities and their points of interest. 
More information about the API can be found in the [documentation file](./CRUD.API/README.md).

Key features:
- **Authentication** & **Authorization**
- **Swagger** documentation
- **Logging** into file and console
- **XML** and **JSON** serialization
- **Dummy mail service**
- **SQLite** database
- **API versioning**
- **Paging**
- **File** upload




## Minimal API
Inspired by
[Kevin's Dockx](https://app.pluralsight.com/library/courses/asp-dot-net-core-7-building-minimal-apis/table-of-contents)
course from pluralsight.

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
![SSL](./others/PostmanBearer_MinimalAPI.png)

The token parameters are checked by the API using the Microsoft JwtBearer package configuration settings, which are 
taken from [appsettings.json "Authentication"](MinimalAPI/appsettings.json).

<br />

#### Authorization policy
For the `POST /dishes` method you need to have the **admin role** and the claim **country=Germany**:
<br />`dotnet user-jwts create --audience MinimalAPI --claim country=Germany --role admin`



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