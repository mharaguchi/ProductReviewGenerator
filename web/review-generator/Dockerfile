FROM node:13.12.0-alpine
ENV NODE_ENV=production
WORKDIR /usr/src/app
COPY ["package.json", "package-lock.json*", "npm-shrinkwrap.json*", "./"]
RUN npm install
RUN npm install -g react-scripts
COPY . .
EXPOSE 3000
CMD ["npm", "start"]