const database = require('../config/database');

class Class_UsersController {
    getClassUser = async (req, res) => {
        const result = await database.any("SELECT * FROM Class_Users");
        console.log(result);
        return res.json(result);
    }

    insertClassUser = async (req, res) => {
        const { guid  , path,type, autorise    } = req.body;
        try {
            await database.query("INSERT INTO Class (chemain, type ,autorise , guid   ) VALUES ($1, $2 , $3 ,$4 )", [guid  , path,type, autorise  ]);
            console.log("Class inserted:", req.body);
            return res.status(201).json({ message: "class added successfully" });
        } catch (error) {
            console.error("Error inserting class:", error);
            return res.status(500).json({ message: "There was an error adding the user" });
        }
    }
}

/******** EXPORTS ********/
module.exports = new Class_UsersController();
