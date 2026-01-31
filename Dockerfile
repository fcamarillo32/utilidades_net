# ========= BUILD =========
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivo de proyecto y restaurar
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y publicar
COPY . ./
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ========= RUNTIME =========
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Puerto que usará Easypanel
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Copiar app compilada
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "n8n_urilisSWA.dll"]
