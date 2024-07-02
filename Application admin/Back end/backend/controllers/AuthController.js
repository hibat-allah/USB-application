const db = require('../config/database')
const bcrypt = require("bcrypt");
const jwt = require('jsonwebtoken')

const JWT_PRIVATE_KEY = "javainuse-secret-key"

class AuthController {
    login = async (req, res) => {
        const { username, password } = req.body;
        //TODO: fix this
        /*const user = await db.query("SELECT * FROM users WHERE username=?", [username]); 
        const login_match = user.password && (await bcrypt.compare(password, user.password));

        if (!login_match)
            return res.status(400).send("Wrong username or password");*/

        // TODO: remove this
        if (username != "admin" || password != "4dm!n$")
            return res.status(400).send("Wrong username or password");
        // ENDTODO

        const token = jwt.sign({username: username}, JWT_PRIVATE_KEY);
        return res.status(200).json(token);
    }

    logout = async (req, res) => {
        res.clearCookie("jwt");
        return res.status(200).send("Successfully logged out!");
    }
}

/******** EXPORTS ********/
module.exports = new AuthController();
