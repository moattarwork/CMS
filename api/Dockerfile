FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY Emporos.Core/*.csproj Emporos.Core/
COPY Emporos.Core.UnitTests/*.csproj Emporos.Core.UnitTests/
COPY Emporos.WebApi/*.csproj Emporos.WebApi/
COPY Emporos.WebApi.IntegrationTests/*.csproj Emporos.WebApi.IntegrationTests/
RUN dotnet restore

COPY . .
WORKDIR /src
RUN dotnet build -c Release -o /app/build
RUN dotnet test -c Release 

FROM build AS publish
WORKDIR /src/Emporos.WebApi
RUN dotnet publish "Emporos.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emporos.WebApi.dll"]
