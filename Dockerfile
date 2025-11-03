# Build com .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY *.sln .
COPY SistemaBarbearia/*.csproj ./SistemaBarbearia/
RUN dotnet restore

# Copiar tudo e build
COPY . .
WORKDIR /src/SistemaBarbearia
RUN dotnet publish -c Release -o /app/publish

# Runtime com .NET 8 ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "SistemaBarbearia.dll"]
