const database = require('../config/database');

class PeripheriqueController {
    getDevice = async (req, res) => {
        try {
            const result = await database.any("SELECT * FROM Device");
            console.log(result);
            return res.json(result || []); // Renvoyer un tableau vide si aucun résultat n'est retourné
        } catch (error) {
            console.error("Error fetching devices:", error);
            return res.status(500).json({ message: "Error fetching devices" });
        }
    }
    

    insertDevice = async (req, res) => {
        const { idDevice, nameD, classI_id } = req.body;
        let inf;

switch (classI_id) {
    case '{4d36e967-e325-11ce-bfc1-08002be10318}':
        inf = 'usbstor.inf';
        break;
    case '{1ed2bbf9-11f0-4084-b21f-ad83a8e6dcdc}':
        inf = 'usbprint.inf';
        break;
    case '{745a17a0-74d3-11d0-b6fe-00a0c90f57da}':
        inf = 'usb.inf';
        break;
    case '{eec5ad98-8080-425f-922a-dabf3de3f69a}':
        inf = 'winusb.inf';
        break;
    case '{ca3e7ab9-b4c3-4ae6-8251-579ef933890f}':
        inf = 'usbvideo.inf';
        break;
    case '{4d36e972-e325-11ce-bfc1-08002be10318}':
        inf = 'rtlwlanu.inf';
        break;
    default:
        // Si classI_id ne correspond à aucun des cas ci-dessus, vous pouvez définir une valeur par défaut
        inf = 'default.inf';
        break;
}
        try {
            await database.query("INSERT INTO Device (idDevice, nameD, classI_id , inf) VALUES ($1, $2, $3 , $4)", [idDevice, nameD, classI_id , inf]);
            console.log("Device inserted:", req.body);
            return res.status(201).json({ message: "Device added successfully" });
        } catch (error) {
            console.error("Error inserting device:", error);
            return res.status(500).json({ message: "There was an error adding the device" });
        }
    }

        deleteDevice = async (req , res )=>{
            
            const {idDevice} = req.body;
            try{
                await database.query("delete from Device where idDevice=($1) ",[idDevice]);
                
                console.log("Device deleted:", req.body);
                return res.status(201).json({ message: "Device deleted successfully" });
            } catch (error) {
                console.error("Error deleting Device:", error);
                return res.status(500).json({ message: "There was an error deleting the Device" });
            }
        }
    } 

/******** EXPORTS ********/
module.exports = new PeripheriqueController();
