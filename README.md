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




