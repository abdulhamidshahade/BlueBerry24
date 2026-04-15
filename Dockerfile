# Stage 1: Build .NET API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS api-build

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

RUN dotnet publish BlueBerry24.API/BlueBerry24.API.csproj -c Release -o /app/api-out

# Stage 2: Build Next.js Web
FROM node:20-alpine AS web-build

WORKDIR /web-build

COPY BlueBerry24.Web/package.json BlueBerry24.Web/package-lock.json ./

RUN npm ci

COPY BlueBerry24.Web/ ./

RUN npm run build

# Stage 3: Runtime - Combine both
FROM debian:bookworm-slim

# Install .NET runtime dependencies
RUN apt-get update && apt-get install -y \
    ca-certificates \
    curl \
    libc6 \
    libgcc-s1 \
    libgssapi-krb5-2 \
    libicu72 \
    libssl3 \
    libstdc++6 \
    zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET runtime
RUN apt-get update && apt-get install -y \
    wget && \
    wget -q https://dot.net/release-metadata/releases-index.json -O /tmp/releases.json && \
    apt-get remove -y wget && \
    rm -rf /var/lib/apt/lists/*

RUN curl https://dot.net/v1/dotnet-install.sh -O && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --channel 9.0 --install-dir /usr/local/dotnet && \
    rm dotnet-install.sh

ENV PATH="/usr/local/dotnet:${PATH}"
ENV DOTNET_ROOT="/usr/local/dotnet"

# Install Node.js
RUN apt-get update && apt-get install -y \
    nodejs npm \
    && rm -rf /var/lib/apt/lists/*

# Create app directories
RUN mkdir -p /app/api /app/web

WORKDIR /app

# Copy .NET API from build stage
COPY --from=api-build /app/api-out ./api/

# Copy Next.js Web from build stage
COPY --from=web-build /web-build/.next ./web/.next
COPY --from=web-build /web-build/public ./web/public
COPY BlueBerry24.Web/package.json BlueBerry24.Web/package-lock.json ./web/
COPY BlueBerry24.Web/next.config.ts BlueBerry24.Web/tsconfig.json ./web/

# Install production dependencies for Next.js
WORKDIR /app/web
RUN npm ci --only=production && npm cache clean --force

# Expose both ports
EXPOSE 7105 30305

# Set environment variables
ENV ASPNETCORE_URLS=http://+:7105
ENV ASPNETCORE_ENVIRONMENT=Production
ENV NODE_ENV=production
ENV PORT=30305

WORKDIR /app

CMD dotnet /app/api/BlueBerry24.API.dll & \
    cd /app/web && node_modules/.bin/next start -p ${PORT:-30305} & \
    wait
