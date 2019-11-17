FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY tripdini.accounts/tripdini.accounts.csproj tripdini.accounts/
RUN dotnet restore tripdini.accounts/tripdini.accounts.csproj
COPY . .
WORKDIR /src/tripdini.accounts
RUN dotnet build tripdini.accounts.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish tripdini.accounts.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "tripdini.accounts.dll"]