FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FeatureFlag.Api/FeatureFlag.Api.csproj", "FeatureFlag.Api/"]
COPY ["FeatureFlag.Application/FeatureFlag.Application.csproj", "FeatureFlag.Application/"]
COPY ["FeatureFlag.Domain/FeatureFlag.Domain.csproj", "FeatureFlag.Domain/"]
COPY ["FeatureFlag.Repository/FeatureFlag.Repository.csproj", "FeatureFlag.Repository/"]
RUN dotnet restore "FeatureFlag.Api/FeatureFlag.Api.csproj"
COPY . .
WORKDIR "/src/FeatureFlag.Api"
RUN dotnet build "FeatureFlag.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FeatureFlag.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeatureFlag.Api.dll"]
