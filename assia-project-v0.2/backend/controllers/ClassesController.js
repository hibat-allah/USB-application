const database = require('../config/database');

class ClassesController {
    getClass = async (req, res) => {
        const result = await database.any("SELECT * FROM Class");
        console.log(result);
        return res.json(result);
    }

    insertClass = async (req, res) => {
        const { guid  , path,type    } = req.body;
        try {
            await database.query("INSERT INTO Class ( guid, chemain  ,type) VALUES ($1 , $3 ,$4 )", [guid  , path,type  ]);
            console.log("Class inserted:", req.body);
            return res.status(201).json({ message: "class added successfully" });
        } catch (error) {
            console.error("Error inserting class:", error);
            return res.status(500).json({ message: "There was an error adding the user" });
        }
    }
}

/******** EXPORTS ********/
module.exports = new ClassesController();
