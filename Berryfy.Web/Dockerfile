FROM node:20-alpine AS build

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm ci

COPY . ./

RUN npm run build

FROM node:20-alpine AS runtime

WORKDIR /app

# 1. Create the user/group early, and give them ownership of the empty /app directory
RUN addgroup -g 1001 -S nodejs \
    && adduser -S nextjs -u 1001 -G nodejs \
    && chown nextjs:nodejs /app

# 2. Switch to the non-root user BEFORE installing dependencies
USER nextjs

# 3. Copy package files and install production dependencies
# Because we are now the 'nextjs' user, the resulting node_modules folder 
# will automatically be owned by nextjs, skipping the need for chown later.
COPY --chown=nextjs:nodejs package.json package-lock.json ./
RUN npm ci --only=production && npm cache clean --force

# 4. Copy build artifacts from the build stage, assigning ownership in transit
COPY --from=build --chown=nextjs:nodejs /app/.next ./.next
COPY --from=build --chown=nextjs:nodejs /app/public ./public
COPY --from=build --chown=nextjs:nodejs /app/next.config.ts ./next.config.ts
COPY --from=build --chown=nextjs:nodejs /app/tsconfig.json ./tsconfig.json

EXPOSE 30305

ENV NODE_ENV=production
ENV PORT=30305

CMD ["npm", "start"]