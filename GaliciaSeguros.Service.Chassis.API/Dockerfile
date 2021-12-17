#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GaliciaSeguros.Service.Chassis.API/GaliciaSeguros.Service.Chassis.API.csproj", "GaliciaSeguros.Service.Chassis.API/"]
RUN dotnet restore "GaliciaSeguros.Service.Chassis.API/GaliciaSeguros.Service.Chassis.API.csproj"
COPY . .
WORKDIR "/src/GaliciaSeguros.Service.Chassis.API"
RUN dotnet build "GaliciaSeguros.Service.Chassis.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GaliciaSeguros.Service.Chassis.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GaliciaSeguros.Service.Chassis.API.dll"]