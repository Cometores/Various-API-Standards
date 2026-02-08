# AirVinyl API
OData API for managing people, their vinyl records, and record stores.

**Types of endpoints implemented:** </br>
&emsp;`get`, `post`, `put`, `patch`, `delete`, `function`, `action`, `singleton`

Data model (SQL Server LocalDB):
```Mathematics
+-------------------+            +-------------------------+
|      People       |            |      VinylRecords       |
+-------------------+            +-------------------------+
| PersonId (PK)     |◄───────────| PersonId (FK → People)  |
| FirstName         |            | VinylRecordId (PK)      |
| LastName          |            | Artist                  |
| Email             |            | Title                   |
| DateOfBirth       |            | CatalogNumber           |
| Gender            |            | Year                    |
| ...               |            | PressingDetailId (FK)   |
+-------------------+            +-------------------------+
                                   │
                                   │
                                   ▼
                          +-------------------------+
                          |     PressingDetails     |
                          +-------------------------+
                          | PressingDetailId (PK)   |
                          | Grams                   |
                          | Inches                  |
                          | Description             |
                          +-------------------------+

+-------------------+            +-------------------------+
|    RecordStores   |            |        Ratings          |
+-------------------+            +-------------------------+
| RecordStoreId(PK) |◄───────────| RatingId (PK)           |
| Name              |            | RecordStoreId (FK)      |
| Tags              |            | RatedByPersonId (FK)    |
| StoreAddress(*)   |            | Value                   |
| ...               |            +-------------------------+
+-------------------+

* StoreAddress is an owned type (Address).
```

## Start exploring
Run `AirVinyl` profile. The API uses SQL Server LocalDB:
`(localdb)\mssqllocaldb`, database `AirVinylDemoDB`.

OData endpoints are rooted at `/odata`. The service metadata is available at:
`/odata/$metadata`

Use any HTTP client (browser, curl, Postman) to try requests. Example queries:

```http
GET https://localhost:5001/odata/People
GET https://localhost:5001/odata/People?$select=FirstName,LastName
GET https://localhost:5001/odata/People?$expand=VinylRecords
GET https://localhost:5001/odata/RecordStores?$filter=contains(Name,'Rock')
GET https://localhost:5001/odata/People(1)/VinylRecords
```

Query options enabled: `$select`, `$expand`, `$orderby`, `$filter`, `$count`.
The global max `$top` is 10; People queries additionally cap `$top` to 5 and
set page size to 4.

## Core endpoints
Base route: `/odata`

### People
- `GET /odata/People`
- `GET /odata/People({id})`
- `POST /odata/People`
- `PUT /odata/People({id})`
- `PATCH /odata/People({id})`
- `DELETE /odata/People({id})`
- `GET /odata/People({id})/VinylRecords`
- `GET /odata/People({id})/VinylRecords({vinylRecordId})`
- `POST /odata/People({id})/VinylRecords`
- `PATCH /odata/People({id})/VinylRecords({vinylRecordId})`
- `DELETE /odata/People({id})/VinylRecords({vinylRecordId})`
- `GET /odata/People({id})/Email` (same for `FirstName`, `LastName`, `DateOfBirth`, `Gender`)
- `GET /odata/People({id})/Email/$value` (raw value; same for properties above)

### RecordStores
- `GET /odata/RecordStores`
- `GET /odata/RecordStores({id})`
- `POST /odata/RecordStores`
- `PATCH /odata/RecordStores({id})`
- `DELETE /odata/RecordStores({id})`
- `GET /odata/RecordStores({id})/Tags`
- `GET /odata/RecordStores/AirVinyl.SpecializedRecordStore`
- `GET /odata/RecordStores({id})/AirVinyl.SpecializedRecordStore`

### Singleton
- `GET /odata/Tim` (Person with id 5)
- `PATCH /odata/Tim`

## Functions and actions
Bound and unbound operations are exposed through OData:

### Functions
- `GET /odata/RecordStores({id})/AirVinyl.Functions.IsHighRated(minimumRating={n})`
- `GET /odata/RecordStores/AirVinyl.Functions.AreRatedBy(personIds=[1,2])`
- `GET /odata/GetHighRatedRecordStores(minimumRating={n})`

### Actions
- `POST /odata/RecordStores({id})/AirVinyl.Actions.Rate`  
  Body: `{ "rating": 4, "personId": 1 }`
- `POST /odata/RecordStores/AirVinyl.Actions.RemoveRatings` (known to be unstable)
- `POST /odata/RemoveRecordStoreRatings`  
  Body: `{ "personId": 1 }`

## Data access

### Entity Framework Core
- Uses SQL Server LocalDB with seeded data and migrations.
- Entities include `Person`, `VinylRecord`, `RecordStore`, `PressingDetail`, `Rating`.
- `SpecializedRecordStore` inherits from `RecordStore`.
- `Address` is an owned type on `RecordStore`.

### OData model
- Convention-based entity sets: `People`, `RecordStores`, `Tim` singleton.
- `VinylRecords` are contained within `People` (no top-level entity set).
