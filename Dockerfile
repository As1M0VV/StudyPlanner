# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY StudyPlanner.csproj ./
RUN dotnet restore StudyPlanner.csproj

COPY . ./
RUN dotnet publish StudyPlanner.csproj -c Release -f net8.0 -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

CMD dotnet StudyPlanner.dll --urls "http://0.0.0.0:${PORT:-10000}"
