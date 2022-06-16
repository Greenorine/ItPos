FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ItPos.Api/ItPos.Api.csproj", "ItPos.Api/"]
RUN dotnet restore "ItPos.Api/ItPos.Api.csproj"
COPY . .
WORKDIR "/src/ItPos.Api"
RUN dotnet build "ItPos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ItPos.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ItPos.Api.dll"]
