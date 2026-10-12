FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ExpenseTrackerWeb/ExpenseTrackerWeb.csproj", "ExpenseTrackerWeb/"]
RUN dotnet restore "ExpenseTrackerWeb/ExpenseTrackerWeb.csproj"

COPY . .
WORKDIR "/src/ExpenseTrackerWeb"
RUN dotnet publish "ExpenseTrackerWeb.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExpenseTrackerWeb.dll"]
