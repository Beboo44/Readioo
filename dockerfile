# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files for better layer caching
COPY *.sln ./
COPY Readioo/*.csproj ./Readioo/
COPY Readioo.Data/*.csproj ./Readioo.Data/
COPY Readioo.Business/*.csproj ./Readioo.Business/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . ./

# Build and publish the web project
RUN dotnet publish Readioo/Readioo.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .
VOLUME /app/wwwroot/images
VOLUME /app/wwwroot/images/authors
VOLUME /app/wwwroot/images/books

# Expose port
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "Readioo.dll"]