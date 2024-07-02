const database = require('../config/database');

class Class_UsersController {
    getClassUser = async (req, res) => {
        const result = await database.any("SELECT * FROM Class_Users");
        console.log(result);
        return res.json(result);
    }

    insertClassUser = async (req, res) => {
        const {user_id , class_id    } = req.body;
        try {
            await database.query("INSERT INTO Class_Users ( user_id , class_id      ) VALUES ($1, $2 )", [user_id , class_id   ]);
            console.log("Class inserted:", req.body);
            return res.status(201).json({ message: "class added successfully" });
        } catch (error) {
            console.error("Error inserting class:", error);
            return res.status(500).json({ message: "There was an error adding the user" });
        }
    }
    deleteUserClass = async (req , res )=>{
        console.log("aaa")
        const {username , class_id} = req.body;
        try{
            await database.query("delete from Class_Users where user_id=($1)  and class_id=($2)",[username,class_id]);
            console.log("ef")
            console.log("User deleted:", req.body);
            return res.status(201).json({ message: "User deleted successfully" });
        } catch (error) {
            console.error("Error deleting user:", error);
            return res.status(500).json({ message: "There was an error deleting the user" });
        }
    }
}

/******** EXPORTS ********/
module.exports = new Class_UsersController();
