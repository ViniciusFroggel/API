# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar apenas a solução e o csproj
COPY *.sln ./
COPY *.csproj ./

# Restaurar dependências
RUN dotnet restore

# Copiar todo o resto do código
COPY . ./

# Publicar a aplicação
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expõe a porta padrão
EXPOSE 5000
EXPOSE 5001

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "SistemaBarbearia.dll"]
