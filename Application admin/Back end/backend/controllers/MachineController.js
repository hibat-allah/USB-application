const database = require('../config/database');

class MachineController {
    getMachine = async (req, res) => {
        const result = await database.any("SELECT * FROM Machine");
        console.log(result);
        return res.json(result);
    }

    insertMachine = async (req, res) => {
        const { nameM,typeM    } = req.body;
        try {
            await database.query("INSERT INTO Machine ( nameM,typeM ) VALUES ($1 , $2 )", [nameM,typeM   ]);
            console.log("machine inserted:", req.body);
            return res.status(201).json({ message: "machine added successfully" });
        } catch (error) {
            console.error("Error inserting machine:", error);
            return res.status(500).json({ message: "There was an error adding the machine" });
        }
    }

    deleteMachine = async (req, res) => {
        const { nameM   } = req.body;
        try {
            await database.query("delete from Machine where nameM=($1) ",[nameM]);
            console.log("machine deleted:", req.body);
            return res.status(201).json({ message: "machine added successfully" });
        } catch (error) {
            console.error("Error deleting machine:", error);
            return res.status(500).json({ message: "There was an error deleting the machine" });
        }
    }
}

/******** EXPORTS ********/
module.exports = new MachineController();
