FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /source

COPY Berryfy.sln ./
COPY Berryfy.API/*.csproj Berryfy.API/
COPY Berryfy.Application/*.csproj Berryfy.Application/
COPY Berryfy.Domain/*.csproj Berryfy.Domain/
COPY Berryfy.Infrastructure/*.csproj Berryfy.Infrastructure/
COPY Berryfy.Tests/*.csproj Berryfy.Tests/
COPY Berryfy.Bot/*.csproj Berryfy.Bot/

RUN dotnet restore Berryfy.sln

COPY . ./

RUN dotnet publish Berryfy.API/Berryfy.API.csproj -c Release -o /app/out


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

ENTRYPOINT ["dotnet", "Berryfy.API.dll"]