FROM node
WORKDIR /app
COPY package.json .
RUN npm install
RUN mkdir /data
COPY . .
EXPOSE 3000
CMD ["npm", "run", "start"]