# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar a solução
COPY *.sln ./

# Copiar o projeto SistemaBarbearia
COPY SistemaBarbearia/*.csproj ./SistemaBarbearia/

# Restaurar dependências
WORKDIR /src/SistemaBarbearia
RUN dotnet restore

# Copiar todo o código do projeto
COPY SistemaBarbearia/. ./  

# Publicar
RUN dotnet publish -c Release -o /app

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "SistemaBarbearia.dll"]
