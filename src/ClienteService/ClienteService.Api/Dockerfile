FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia todos os .csproj referenciados
COPY ["src/ClienteService/ClienteService.Api/ClienteService.Api.csproj", "src/ClienteService/ClienteService.Api/"]
COPY ["src/ClienteService/ClienteService.Application/ClienteService.Application.csproj", "src/ClienteService/ClienteService.Application/"]
COPY ["src/ClienteService/ClienteService.Domain/ClienteService.Domain.csproj", "src/ClienteService/ClienteService.Domain/"]
COPY ["src/ClienteService/ClienteService.Infrastructure/ClienteService.Infrastructure.csproj", "src/ClienteService/ClienteService.Infrastructure/"]
COPY ["src/Shared/Shared.Integration/Shared.Integration.csproj", "src/Shared/Shared.Integration/"]

# Restaura as dependências
RUN dotnet restore "src/ClienteService/ClienteService.Api/ClienteService.Api.csproj"

# Copia todo o código fonte
COPY . .

WORKDIR "/src/src/ClienteService/ClienteService.Api"
RUN dotnet build "ClienteService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClienteService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClienteService.Api.dll"]