# Etapa 1: Base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 3030

# Etapa 2: Construção da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Lambda.Pregiato.csproj", "./"]
RUN dotnet restore "./Lambda.Pregiato.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "./Lambda.Pregiato.csproj" -c Release -o /app/build

# Etapa 3: Publicação
FROM build AS publish
RUN dotnet publish "./Lambda.Pregiato.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagem final para runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lambda.Pregiato.dll"]