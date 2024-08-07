const database = require('../config/database');

class UsersController {
    getUsers = async (req, res) => {
        const result = await database.any("SELECT * FROM Users");
        console.log(result);
        return res.json(result);
    }

    insertUser = async (req, res) => {
        const { email, username } = req.body;
        try {
            await database.query("INSERT INTO Users (email, username   ) VALUES ($1, $2 )", [email, username]);
            console.log("User inserted:", req.body);
            return res.status(201).json({ message: "User added successfully" });
        } catch (error) {
            console.error("Error inserting user:", error);
            return res.status(500).json({ message: "There was an error adding the user" });
        }
    }
    deleteUser = async (req , res )=>{
        console.log("aaa")
        const {username} = req.body;
        try{
            await database.query("delete from Users where username=($1) ",[username]);
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
module.exports = new UsersController();
