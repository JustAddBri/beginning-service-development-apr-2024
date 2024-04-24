#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
## app runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

## tools to build/restore/test
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HelpDeskSupportApi/HelpDeskSupportApi.csproj", "HelpDeskSupportApi/"]
RUN dotnet restore "./HelpDeskSupportApi/HelpDeskSupportApi.csproj"
COPY . .
WORKDIR "/src/HelpDeskSupportApi"
RUN dotnet build "./HelpDeskSupportApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HelpDeskSupportApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

## production image
WORKDIR /app
FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelpDeskSupportApi.dll"]