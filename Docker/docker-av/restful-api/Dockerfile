FROM node
WORKDIR /app
COPY package.json .
RUN npm install
RUN npm install express multer adm-zip axios
RUN mkdir /data
COPY . .
EXPOSE 3000
CMD ["npm", "run", "start"]