# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar solução e projeto
COPY *.sln ./
COPY SistemaBarbearia/*.csproj ./SistemaBarbearia/

# Restaurar dependências
RUN dotnet restore

# Copiar todo o restante do código
COPY . ./

# Publicar o projeto (vai incluir appsettings.json por causa do CopyToOutputDirectory)
RUN dotnet publish SistemaBarbearia/SistemaBarbearia.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar publicação
COPY --from=build /app/publish ./

# Expor portas
EXPOSE 5000
EXPOSE 5001

# Entrypoint
ENTRYPOINT ["dotnet", "SistemaBarbearia.dll"]
