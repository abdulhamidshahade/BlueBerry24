FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /source

COPY BlueBerry24.sln ./
COPY BlueBerry24.API/*.csproj BlueBerry24.API/
COPY BlueBerry24.Application/*.csproj BlueBerry24.Application/
COPY BlueBerry24.Domain/*.csproj BlueBerry24.Domain/
COPY BlueBerry24.Infrastructure/*.csproj BlueBerry24.Infrastructure/
COPY BlueBerry24.Tests/*.csproj BlueBerry24.Tests/
COPY Blueberry24.Bot/*.csproj Blueberry24.Bot/

RUN dotnet restore BlueBerry24.sln

COPY . ./

RUN dotnet publish BlueBerry24.API/BlueBerry24.API.csproj -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out ./

EXPOSE 7105

ENV ASPNETCORE_URLS=http://+:7105
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=15s --timeout=10s --start-period=120s --retries=10 \
    CMD curl -f http://localhost:7105/health || exit 1

ENTRYPOINT ["dotnet", "BlueBerry24.API.dll"]