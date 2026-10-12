# Expense Tracker .Net



## Project setup

```
mkdir ExpenseTrackerAPI
cd ExpenseTrackerAPI
dotnet new webapi -controllers -f net9.0


cd ..
mkdir ExpenseTrackerWeb
cd ExpenseTrackerWeb
dotnet new razor -n ExpenseTrackerWeb

cd ..
dotnet new sln -n ExpenseTracker
dotnet sln ExpenseTracker.sln add ExpenseTrackerAPI/ExpenseTrackerAPI.csproj
dotnet sln ExpenseTracker.sln add ExpenseTrackerWeb/ExpenseTrackerWeb.csproj

```

## build all the project
```
dotnet build ExpenseTracker.sln
```

## Run the API
```
cd ExpenseTrackerAPI
dotnet run

```

go to http://localhost:<port>/WeatherForecast
https://localhost:{port}/openapi/v1.json


## Run the Web
```
cd ExpenseTrackerWeb
dotnet run
```

## Run with Docker

Docker Desktop (or another Docker installation with Docker Compose) is required.

Run all commands from the repository root.

### Build the images separately

```bash
docker build -f ExpenseTrackerAPI/Dockerfile -t expense-tracker-api .
docker build -f ExpenseTrackerWeb/Dockerfile -t expense-tracker-web .
```

### Build and run with Docker Compose

```bash
docker compose up --build
```

The applications are then available at:

- Web application: http://localhost:5072
- API: http://localhost:5155
- API OpenAPI document: http://localhost:5155/openapi/v1.json

Run the containers in the background:

```bash
docker compose up --build -d
```

View container logs:

```bash
docker compose logs -f
```

Stop and remove the containers:

```bash
docker compose down
```

Rebuild the images after changing the source:

```bash
docker compose build --no-cache
docker compose up -d
```



