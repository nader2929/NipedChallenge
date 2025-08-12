# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["NipedTestApp/", "./NipedTestApp"]
COPY ["Data/", "./Data"]
COPY ["Shared/", "./Shared"]
RUN dotnet restore "NipedTestApp/NipedTestApp.csproj"

# Copy everything else and build the application
RUN ls -la
RUN dotnet build "NipedTestApp/NipedTestApp.csproj" -c Release -o /app/build

# Stage 2: Publish the .NET application
FROM build AS publish
RUN dotnet publish "NipedTestApp/NipedTestApp.csproj" -c Release -o /app/publish

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NipedTestApp.dll"]
