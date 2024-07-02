const database = require('../config/database');

class Logs {
    getLogs = async (req, res) => {
        const result = await database.any("SELECT * FROM Logs");
        console.log(result);
        return res.json(result);
    }
}

/******** EXPORTS ********/
module.exports = new Logs();