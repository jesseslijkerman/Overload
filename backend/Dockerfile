FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Eerst alleen de .csproj-bestanden, zodat 'restore' gecachet blijft zolang de packages niet wijzigen
COPY Overload.Domain/Overload.Domain.csproj Overload.Domain/
COPY Overload.Api/Overload.Api.csproj Overload.Api/
RUN dotnet restore Overload.Api/Overload.Api.csproj

COPY . .
RUN dotnet publish Overload.Api/Overload.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# De aspnet-image luistert standaard op poort 8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Overload.Api.dll"]
