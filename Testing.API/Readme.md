# Testing API
Consists of two projects - API itself and tests on xUnit.

## EmployeeManagement API


**Types of endpoints implemented:** </br>
&emsp;`get`, `post`

```mathematics
┌─────────────────────┐        ┌─────────────────────┐     ┌───────────────────────────────┐
│   InternalEmployee  │        │       Course        │     │   CourseInternalEmployee      │
│─────────────────────│        │─────────────────────│     │───────────────────────────────│
│ Id (PK)             │        │ Id (PK)             │     │ AttendedCoursesId (FK→Course) │
│ FirstName           │        │ Title               │     │ EmployeesThatAttendedId (FK→  │
│ LastName            │        │ IsNew               │     │                  InternalEmp) │
│ YearsWorked         │        └─────────────────────┘     └───────────────────────────────┘
│ Salary              │
│ HasBeenPromoted     │
│ VacationDays        │
└─────────────────────┘



```