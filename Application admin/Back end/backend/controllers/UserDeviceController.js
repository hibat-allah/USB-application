const database = require('../config/database');

class UserDeviceController {
    getUserDevice = async (req, res) => {
        const result = await database.any("SELECT * FROM Device_Users");
        console.log(result);
        return res.json(result);
    }

    insertUserDevice = async (req, res) => {
        const { device_id, user_id } = req.body;
        
        try {
            await database.query("INSERT INTO Device_Users   (user_id, device_id   ) VALUES ($1, $2 )", [user_id, device_id ]);
            console.log("Device_Users inserted:", req.body);
            return res.status(201).json({ message: "User added successfully" });
        } catch (error) {
            console.error("Error inserting Device_Users:", error);
            return res.status(500).json({ message: "There was an error adding the user" });
        }
    }
    deleteUserDevice = async (req , res )=>{
        
        const {username , device_id} = req.body;
        try{
            await database.query("delete from Device_Users where user_id=($1) and device_id=($2)  ",[username , device_id]);
            
            console.log("Device_Users deleted:", req.body);
            return res.status(201).json({ message: "Device_Users deleted successfully" });
        } catch (error) {
            console.error("Error deleting user:", error);
            return res.status(500).json({ message: "There was an error deleting the Device_Users" });
        }
    }
    
}

/******** EXPORTS ********/
module.exports = new UserDeviceController();
