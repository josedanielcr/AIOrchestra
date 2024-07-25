# Stage 1: Build the Angular application
FROM node:20 as build-stage

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm install

COPY . .
RUN npm run build --prod --output-path=dist/aiorchestra-ui --base-href=/

# Check the contents of the build directory
RUN ls -l /app/dist/aiorchestra-ui

# Stage 2: Serve the application with Nginx
FROM nginx:alpine

COPY --from=build-stage /app/dist/aiorchestra-ui/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Check the contents of the Nginx HTML directory
RUN ls -l /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
