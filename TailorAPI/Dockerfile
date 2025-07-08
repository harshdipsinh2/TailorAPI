# --- BASE IMAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# --- BUILD IMAGE ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore
COPY ../ ./
WORKDIR /src/TailorAPI
RUN dotnet restore "TailorAPI.csproj"
RUN dotnet build "TailorAPI.csproj" -c Release -o /app/build
RUN dotnet publish "TailorAPI.csproj" -c Release -o /app/publish

# --- FINAL IMAGE ---
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TailorAPI.dll"]
