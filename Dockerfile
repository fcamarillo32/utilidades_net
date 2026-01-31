# ========= BUILD =========
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copiar archivo de proyecto y restaurar (cache de dependencias)
COPY *.csproj ./
RUN dotnet restore --runtime linux-musl-x64

# Copiar todo el código y publicar
COPY . ./
RUN dotnet publish -c Release -o /app/publish \
    --runtime linux-musl-x64 \
    --self-contained false \
    /p:UseAppHost=false \
    /p:PublishReadyToRun=true

# ========= RUNTIME =========
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Instalar dependencias necesarias para iText7 (fuentes y librerías)
RUN apk add --no-cache \
    icu-libs \
    fontconfig \
    ttf-dejavu \
    libgdiplus

# Variables de entorno para Easypanel
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Puerto que usará Easypanel
EXPOSE 8080

# Usuario no-root para seguridad
RUN adduser -D -u 1000 appuser
USER appuser

# Copiar app compilada
COPY --from=build --chown=appuser:appuser /app/publish .

# Health check para Easypanel
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/swagger/index.html || exit 1

ENTRYPOINT ["dotnet", "n8n_urilisSWA.dll"]
