# CNAB Processing App

This application is a full-stack solution built with .NET 9 (C#) for the backend and React 19 for the frontend. It uses Microsoft SQL Server as the database.

This project was developed as part of a technical exercise to demonstrate proficiency in modern software architecture, clean coding practices, and full-stack application development.

## Architecture Overview

Although a simple 3-tier architecture would suffice for this task, Clean Architecture principles were implemented to align with the job posting's requirements. This includes the use of:

- CQRS (Command Query Responsibility Segregation) via MediatR
- Repository Pattern
- Unit of Work Pattern

Both unit tests and integration tests are provided for the implemented features.

## Features

- Clean Architecture
- Documented API with Swagger UI
- Simple React-based UI for viewing and importing transactions
- Idempotent import: ensures repeated file uploads don't create duplicate records
- Line-based error reporting during import: errors are identified and reported per line
- Real-time account balance computation after each transaction
- Support for running locally or via Docker

## API Access

Once the backend is running in development mode, Swagger UI will be available at:

```
https://localhost:9910/swagger/index.html
```

## Repository

Repository located at

```
https://github.com/jonnyartificial/cnab
```

Clone the repository (private fork):

```
git clone https://github.com/jonnyartificial/cnab.git
```

Inside the cloned cnab folder, you'll find:

- backend/ – C# application

- frontend/ – React application

## Running the Application Locally

### Prerequisites

- Visual Studio 2022
- .NET 9 SDK
- React 19
- Microsoft SQL Server Express

### Backend Setup

Open backend/Cnab.sln in Visual Studio.

Ensure the connection string in appsettings.Development.json is correct for your enviroment.

Apply Entity Framework migrations manually using the Package Manager Console:

```
PM> dotnet ef database update -s cnab.webapi -p cnab.application
```

This will create the database cnab-dev in your local SQLEXPRESS.

Run the project. You should see Swagger UI with 3 available endpoints.

### Frontend Setup

Open a terminal (preferably Bash or Git Bash).

Navigate to the frontend folder.

Install dependencies and run the development server using Bash terminal:

```
$ npm install
$ npm run dev
```

Access the UI at:

```
http://localhost:9000/
```

## Running with Docker

Ensure Docker Desktop is installed and running.

In a Bash terminal, from the cnab root folder, run:

```
$ docker compose build
$ docker compose up -d
```

Once the containers are up, open the application in your browser:

```
http://localhost:9000/
```
