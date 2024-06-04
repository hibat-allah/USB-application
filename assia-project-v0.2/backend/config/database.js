const pgp = require('pg-promise')();

const cn = {
    host: 'localhost', // 'localhost' is the default;
    port: 5432, // 5432 is the default;
    database: 'mydatabase',
    user: 'usb',
    password: 'assia',
};

module.exports = pgp(cn); // database instance;