# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["LibraryBook/LibraryBook.csproj", "./LibraryBook/"]
WORKDIR /src/LibraryBook
RUN dotnet restore

# Copy all files and build the project
COPY ./LibraryBook ./LibraryBook
WORKDIR /src/LibraryBook
RUN dotnet build -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryBook.dll"]
