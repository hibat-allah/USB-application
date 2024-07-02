const express = require("express");
const NodeClam = require('clamscan');
const fs = require('fs');
const path = require('path');
const AdmZip = require('adm-zip');
const axios = require('axios');
const multer = require('multer');

const ClamScan = new NodeClam().init({
    clamdscan:{
        host: "clamav",
        port: 3310,
        timeout: 60000 // Timeout for scanning files (in ms)
    }
});

const app = express();
app.use(express.json());

function createSenderId(req) {
    const ipAddress = req.ip;
    const username = req.headers['username']; // Assumes username is sent in headers
    return `${ipAddress}_${username}`;
}

const upload = multer({ dest: 'uploads/' }); // Destination folder for uploaded files

async function deleteFile(filePath) {
    return new Promise((resolve, reject) => {
        fs.unlink(filePath, err => {
            if (err) {
                console.error(`Error deleting file: ${filePath}`, err);
                reject(err);
            } else {
                resolve();
            }
        });
    });
}

app.post('/analyze', upload.single('file'), async (req, res) => {
    const filePath = req.file.path;
    const username = req.headers['username'];

    console.log(`Analyzing file: ${filePath} from user: ${username}`);

    try {
        const scanResult = await ClamScan.then(clam => clam.scanFile(filePath));
        console.log(`Scan result: ${JSON.stringify(scanResult)}`);

        const isInfected = scanResult.isInfected === true;
        res.json(isInfected);

    } catch (err) {
        console.error('Error during file analysis:', err);
        res.status(500).json({ error: 'Internal server error' });
    } finally {
        try {
            await deleteFile(filePath);
        } catch (err) {
            console.error(`Failed to delete file ${filePath} after analysis.`);
        }
    }
});

async function getPaths(folderPath){
    let folder_paths = [folderPath];
    let i = 0;

    while(true){
        const FILES = await fs.promises.readdir(folder_paths[i]);
        for (const file of FILES) {
            const filePath = path.join(folder_paths[i], file);
            const stats = await fs.promises.stat(filePath);
            if (stats.isDirectory()) {
                folder_paths.push(filePath);
            }
        }
        i++;
        if (i == folder_paths.length) {
            return folder_paths;
        }
    }
}

app.post("/analyze-folder", async (req, res) => {
    const folderPath = req.body.path;
    if (!folderPath) {
        return res.status(400).json({ error: 'No folder path provided' });
    }

    console.log("Received analysis request for:", folderPath);

    let badFiles = [];

    try {
        const folder_paths = await getPaths(folderPath);
        console.log("All folders paths:", folder_paths);

        const scanPromises = folder_paths.map(async (fpath) => {
            const files = await fs.promises.readdir(fpath);
            console.log("Files in path:", files, fpath);

            const filePromises = files.map(async (file) => {
                const filePath = path.join(fpath, file);
                console.log("File path:", filePath);

                const stats = await fs.promises.stat(filePath);
                if (!stats.isDirectory()) {
                    try {
                        const scanResult = await ClamScan.then(x => x.scanFile(filePath));
                        console.log("Scan result:", scanResult);

                        if (!scanResult || scanResult.isInfected === true) {
                            badFiles.push({ file, scanResult });
                            console.log("Bad file:", filePath, "\n result:", scanResult);
                        }
                    } catch (err) {
                        console.error("Error scanning file:", filePath, err);
                    } finally {
                        try {
                            await deleteFile(filePath);
                        } catch (err) {
                            console.error(`Failed to delete file ${filePath} after analysis.`);
                        }
                    }
                }
            });

            await Promise.all(filePromises);
        });

        await Promise.all(scanPromises);

        console.log("Final bad files:", badFiles);
        res.json(badFiles.length > 0);

    } catch (error) {
        console.error('Error during folder analysis:', error);
        res.status(500).json({ error: 'Internal server error' });
    } finally {
        fs.rmdir(folderPath, { recursive: true }, err => {
            if (err) {
                console.error(`Error deleting folder: ${folderPath}`, err);
            }
        });
    }
});

// Error handling middleware
app.use((err, req, res, next) => {
    console.error('Error:', err);
    res.status(500).json({ error: 'Internal server error' });
});

app.listen(3000, function () {
    console.log("Server listening on port 3000");
});
