# ---- Etapa 1: compilar y publicar ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore TecnoGasHogar/TecnoGasHogar.csproj
RUN dotnet publish TecnoGasHogar/TecnoGasHogar.csproj -c Release -o /app/publish --no-restore

# ---- Etapa 2: imagen de ejecucion ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

# Render inyecta la variable PORT; se usa 8080 como valor por defecto.
CMD dotnet TecnoGasHogar.dll --urls http://0.0.0.0:${PORT:-8080}