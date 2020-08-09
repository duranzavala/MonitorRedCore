#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY MonitorRedCore.API/MonitorRedCore.API.csproj MonitorRedCore.API/
COPY MonitorRedCore.Core/MonitorRedCore.Core.csproj MonitorRedCore.Core/
COPY MonitorRedCore.Infraestructure/MonitorRedCore.Infraestructure.csproj MonitorRedCore.Infraestructure/
RUN dotnet restore "MonitorRedCore.API/MonitorRedCore.API.csproj"
COPY . .
WORKDIR "/src/MonitorRedCore.API"
RUN dotnet build "MonitorRedCore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonitorRedCore.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonitorRedCore.API.dll"]
