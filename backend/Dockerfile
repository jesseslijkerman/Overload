FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the .csproj files first so the 'restore' layer stays cached until the packages change
COPY Overload.Domain/Overload.Domain.csproj Overload.Domain/
COPY Overload.Api/Overload.Api.csproj Overload.Api/
RUN dotnet restore Overload.Api/Overload.Api.csproj

COPY . .
RUN dotnet publish Overload.Api/Overload.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# The aspnet image listens on port 8080 by default
EXPOSE 8080
ENTRYPOINT ["dotnet", "Overload.Api.dll"]
