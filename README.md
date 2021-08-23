# CMS

The repo contains a simple Contact Management System written in .Net 5 and Angular Framework.

## Pre-requisite 

To be able to run the application, the following components need to be installed:

- [.Net 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [NodeJs](https://nodejs.org/en/)

## How to build/run

Clone the project from github to the local machine 

```
git clone https://github.com/moattarwork/cms
```

### API

In order to run the API:

```
cd cms\api

dotnet build
dotnet run 
```

The OpenAPI endpoint for the API is available at `https://localhost:5001/swagger` which the endpoints are directly testable from API but also postman can be used to test the API. Also the API currently have almost full unit and integration tests.

**NOTE:** The API uses in memory database instead of SQL Server to avoid dependency to SQL however SQL Server 2017+ in docker can be used for local development. 

### Web App

In order to run the application:

```
cd cms\webapp

npm install
npm start
```

The configuration is already pointing to `https://localhost:5001` so by browsing to `https://localhost:4200` the application will be accessible.

The Angular application has been done in NX framework which facilitates a couple of criteria for Angular Apps:

- MicroApp development
- Mono-Repo development
- Better packaging, publishing and upgrade for Angular components and apps
- Integration with Jest, Cypress and NGRX

**NOTE:** NGRX based solution has not been implemented as part of the solution (Due to the timing and effort for unit testing too) however a foundation work has been done for defining the state, reducers, effects, actions and selectors on a separate branch (NGRX which is not runnable yet). It can be considered as an extension point to the current solution for a redux-based implementation.

