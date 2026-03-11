FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/PropostaService/PropostaService.Worker/PropostaService.Worker.csproj", "src/PropostaService/PropostaService.Worker/"]
COPY ["src/PropostaService/PropostaService.Application/PropostaService.Application.csproj", "src/PropostaService/PropostaService.Application/"]
COPY ["src/PropostaService/PropostaService.Domain/PropostaService.Domain.csproj", "src/PropostaService/PropostaService.Domain/"]
COPY ["src/PropostaService/PropostaService.Infrastructure/PropostaService.Infrastructure.csproj", "src/PropostaService/PropostaService.Infrastructure/"]
COPY ["src/Shared/Shared.Integration/Shared.Integration.csproj", "src/Shared/Shared.Integration/"]

RUN dotnet restore "src/PropostaService/PropostaService.Worker/PropostaService.Worker.csproj"

COPY . .

WORKDIR "/src/src/PropostaService/PropostaService.Worker"
RUN dotnet build "PropostaService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PropostaService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropostaService.Worker.dll"]