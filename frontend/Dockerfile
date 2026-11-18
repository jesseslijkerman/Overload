# Stage 1: build the Angular app
FROM node:22-alpine AS build
WORKDIR /app

# Copy only the package files first so 'npm ci' stays cached until the dependencies change
COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration production

# Stage 2: serve the static files with nginx
FROM nginx:alpine AS runtime
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/overload/browser /usr/share/nginx/html

EXPOSE 80
