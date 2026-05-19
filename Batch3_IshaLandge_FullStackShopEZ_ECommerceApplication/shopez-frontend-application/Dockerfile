# Stage 1 - Build Angular App
FROM node:22-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build -- --configuration production

# Stage 2 - Nginx
FROM nginx:alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/shopez-frontend-application/browser /usr/share/nginx/html

EXPOSE 80