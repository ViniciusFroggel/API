# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar apenas o .sln e o csproj(s)
COPY *.sln ./
COPY *.csproj ./   # <- alterado para copiar diretamente da raiz
RUN dotnet restore

# Copiar tudo e buildar
COPY . ./
WORKDIR /src
RUN dotnet publish -c Release -o /app

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "SistemaBarbearia.dll"]
