const dotenv = require('dotenv'); dotenv.config();
const express = require('express');
const bodyParser = require('body-parser');
const cookieParser = require('cookie-parser');
const cors = require('cors');
const helmet = require("helmet");
const app = express();
const database = require('./config/database');

// configure express app
app.set('trust proxy', 1) // trust first proxy
app.use(cors({
  origin: '*',
  methods: ["GET", "POST", "PUT", "DELETE"],
  credentials: true,
}));
app.use(helmet()); // add security measures
app.use(cookieParser());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

// controllers
const AuthController = require('./controllers/AuthController');
const UsersController = require('./controllers/UsersController');
const ClassesController = require('./controllers/ClassesController');
const DeviceController = require('./controllers/PeripheriqueController');
const DeviceUserController = require('./controllers/UserDeviceController');

// routes
app.post('/login', AuthController.login);
app.get ('/users', UsersController.getUsers);
app.post('/users', UsersController.insertUser)
app.post('/class', ClassesController.insertClass)
app.get('/class', ClassesController.getClass)
app.get('/peripherique', DeviceController.getDevice)
app.post('/peripherique', DeviceController.insertDevice)
app.post('/DeviceUsers', DeviceUserController.insertUserDevice)
app.get('/DeviceUsers', DeviceUserController.getUserDevice)


app.use((req, res) => res.sendStatus(404))

// start server
app.listen(8080, () => console.log(`[SERVER] Listening on port ${8080}`));

module.exports = app;