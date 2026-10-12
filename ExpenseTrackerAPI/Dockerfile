FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ExpenseTrackerAPI/ExpenseTrackerAPI.csproj", "ExpenseTrackerAPI/"]
RUN dotnet restore "ExpenseTrackerAPI/ExpenseTrackerAPI.csproj"

COPY . .
WORKDIR "/src/ExpenseTrackerAPI"
RUN dotnet publish "ExpenseTrackerAPI.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExpenseTrackerAPI.dll"]
