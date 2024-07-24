# Stage 1: Build the Angular application
FROM node:20 as build-stage

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm install

COPY . .
RUN npm run build --prod --output-path=dist/aiorchestra-ui --base-href=/

# Stage 2: Serve the application with Nginx
FROM nginx:alpine

COPY --from=build-stage /app/dist/aiorchestra-ui /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]