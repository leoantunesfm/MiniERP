FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MiniERP.Api/MiniERP.Api.csproj", "MiniERP.Api/"]
COPY ["MiniERP.Application/MiniERP.Application.csproj", "MiniERP.Application/"]
COPY ["MiniERP.Domain/MiniERP.Domain.csproj", "MiniERP.Domain/"]
COPY ["MiniERP.Infrastructure/MiniERP.Infrastructure.csproj", "MiniERP.Infrastructure/"]
RUN dotnet restore "MiniERP.Api/MiniERP.Api.csproj"
COPY . .
WORKDIR "/src/MiniERP.Api"
RUN dotnet publish "MiniERP.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MiniERP.Api.dll"]