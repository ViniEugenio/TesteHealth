#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Teste_Funcional_Health/Teste_Funcional_Health.csproj", "Teste_Funcional_Health/"]
RUN dotnet restore "Teste_Funcional_Health/Teste_Funcional_Health.csproj"
COPY . .
WORKDIR "/src/Teste_Funcional_Health"
RUN dotnet build "Teste_Funcional_Health.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teste_Funcional_Health.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teste_Funcional_Health.dll"]