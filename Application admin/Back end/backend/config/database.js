const pgp = require('pg-promise')();

const cn = {
    host: '192.168.1.134', // 'localhost' is the default;
    port: 5432, // 5432 is the default;
    database: 'entreprisebd',
    user: 'zflub',
    password: 'M#N@d31N',
};

module.exports = pgp(cn); // database instance;