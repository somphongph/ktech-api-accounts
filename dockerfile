FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 59518
EXPOSE 44364

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS build
WORKDIR /src
COPY apiaccounts/apiaccounts.csproj apiaccounts/
RUN dotnet restore apiaccounts/apiaccounts.csproj
COPY . .
WORKDIR /src/apiaccounts
RUN dotnet build apiaccounts.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish apiaccounts.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "apiaccounts.dll"]