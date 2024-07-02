const dotenv = require('dotenv'); dotenv.config();
const express = require('express');
const bodyParser = require('body-parser');
const cookieParser = require('cookie-parser');
const cors = require('cors');
const helmet = require("helmet");
const app = express();
const database = require('./config/database');
const fs = require('fs')
const https = require('https');
const options = {
  key: fs.readFileSync("./certs/server.key"),
  cert: fs.readFileSync("./certs/server.crt"),
}
const server = https.createServer(options ,app);

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
const Class_UsersController = require('./controllers/Class_UsersController');
const MachineController = require('./controllers/MachineController');
const Logs = require('./controllers/Logs');
// routes
app.post('/login', AuthController.login);
app.get ('/users', UsersController.getUsers);
app.post('/users', UsersController.insertUser);
app.post('/delete-user', UsersController.deleteUser);


app.get('/Logs', Logs.getLogs);
app.post('/class', ClassesController.insertClass);
app.get('/class', ClassesController.getClass);

app.get('/peripherique', DeviceController.getDevice);
app.post('/peripherique', DeviceController.insertDevice);
app.post('/delete-peripherique', DeviceController.deleteDevice);

app.post('/DeviceUsers', DeviceUserController.insertUserDevice);
app.get('/DeviceUsers', DeviceUserController.getUserDevice);
app.post('/delete-DeviceUsers', DeviceUserController.deleteUserDevice);

app.post('/ClassUsers', Class_UsersController.insertClassUser);
app.get('/ClassUsers', Class_UsersController.getClassUser);
app.post('/delete-ClassUsers', Class_UsersController.deleteUserClass);

app.post('/Machine', MachineController.insertMachine)
app.post('/delete-Machine', MachineController.deleteMachine)
app.get('/Machine', MachineController.getMachine)


app.use((req, res) => res.sendStatus(404))

// start server
server.listen(8080, () => console.log(`[SERVER] Listening on port ${8080}`));

module.exports = app;