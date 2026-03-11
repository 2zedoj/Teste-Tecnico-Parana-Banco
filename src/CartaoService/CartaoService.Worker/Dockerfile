FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/CartaoService/CartaoService.Worker/CartaoService.Worker.csproj", "src/CartaoService/CartaoService.Worker/"]
COPY ["src/CartaoService/CartaoService.Application/CartaoService.Application.csproj", "src/CartaoService/CartaoService.Application/"]
COPY ["src/CartaoService/CartaoService.Domain/CartaoService.Domain.csproj", "src/CartaoService/CartaoService.Domain/"]
COPY ["src/CartaoService/CartaoService.Infrastructure/CartaoService.Infrastructure.csproj", "src/CartaoService/CartaoService.Infrastructure/"]
COPY ["src/Shared/Shared.Integration/Shared.Integration.csproj", "src/Shared/Shared.Integration/"]

RUN dotnet restore "src/CartaoService/CartaoService.Worker/CartaoService.Worker.csproj"

COPY . .

WORKDIR "/src/src/CartaoService/CartaoService.Worker"
RUN dotnet build "CartaoService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CartaoService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CartaoService.Worker.dll"]
