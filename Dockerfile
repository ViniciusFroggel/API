# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar a solução
COPY *.sln ./

# Criar diretório para os projetos
RUN mkdir SistemaBarbearia
COPY SistemaBarbearia/*.csproj ./SistemaBarbearia/

# Restaurar dependências
WORKDIR /src/SistemaBarbearia
RUN dotnet restore

# Copiar todo o restante do código
COPY SistemaBarbearia/. ./

# Publicar
RUN dotnet publish -c Release -o /app

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "SistemaBarbearia.dll"]
