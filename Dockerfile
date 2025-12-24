FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/CoreHub.Web/CoreHub.Web.csproj", "src/CoreHub.Web/"]
COPY ["src/CoreHub.Infrastructure/CoreHub.Infrastructure.csproj", "src/CoreHub.Infrastructure/"]
COPY ["src/CoreHub.Application/CoreHub.Application.csproj", "src/CoreHub.Application/"]
COPY ["src/CoreHub.Domain/CoreHub.Domain.csproj", "src/CoreHub.Domain/"]
RUN dotnet restore "src/CoreHub.Web/CoreHub.Web.csproj"
COPY . .
WORKDIR "/src/src/CoreHub.Web"
RUN dotnet build "CoreHub.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreHub.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreHub.Web.dll"]
