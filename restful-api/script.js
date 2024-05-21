
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
app = express();
app.use(express.json());
//app.use(bodyParser.json());
function createSenderId(req) {
    const ipAddress = req.ip;
    const username = req.headers['username']; // Assumes username is sent in headers
    return `${ipAddress}_${username}`;
}

const upload = multer({ dest: 'uploads/' }); // Destination folder for uploaded files

// POST endpoint for file uploads
app.post('/upload', upload.single('file'), (req, res) => {
   try {
        if (!req.file ) {
            return res.status(400).json({ error: 'No file uploaded' });
        }

        // old one: const senderId = req.headers['sender-id'] || 'unknown';
        const senderId = createSenderId(req);
        if (!senderId) {
            return res.status(400).json({ error: 'No sender ID provided' });
        }

        const senderDir = path.join('/data/uploads', senderId);

        // Delete the existing directory if it exists
        if (fs.existsSync(senderDir)) {
            fs.rmSync(senderDir, { recursive: true, force: true });
        }

        // Create a new directory
        fs.mkdirSync(senderDir, { recursive: true });

        const uploadedFilePath = req.file.path;
        const destinationPath = path.join(senderDir, req.file.originalname);
        fs.renameSync(uploadedFilePath, destinationPath);

        // Extract ZIP file
        const zip = new AdmZip(destinationPath);
        zip.extractAllTo(senderDir, true);

        // Delete the uploaded ZIP file after extraction
        fs.unlinkSync(destinationPath);

        // Call the analyze endpoint with the new folder path
        const analyzeUrl = `http://localhost:3000/analyze`;
        console.log("senderDir: ",senderDir)
        axios.post(analyzeUrl, { path: senderDir })
            .then(response => res.json(response.data))
            .catch(error => res.status(500).json({ error: error.message }));

    }
    catch (error) {
        console.error('Error during file upload:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});




async function getPaths(folderPath){
    let folder_paths=[folderPath]
    let i = 0
    while(true){
    //   console.log("getting folder paths....",i)
      const FILES = await fs.promises.readdir(folder_paths[i]);
      for (const file of FILES) {
        const filePath = path.join(folder_paths[i], file);
        // console.log("files nd folders path:",filePath)
        const stats = await fs.promises.stat(filePath);
        // console.log("stats: ",stats)
        if (stats.isDirectory()) {
             folder_paths.push(filePath)
            //  console.log("folder path:",folderPath)
        }
      }
      i++
      if (i == folder_paths.length) {
        // console.log("finale folders paths:",folder_paths)
        return folder_paths
      }
    }
}

app.post("/analyze", async function (req, res, next) {
    
    //!here
    console.log("req body path: ",req.body.path)
    const folderPath = req.body.path;
    if (!folderPath) {
        return res.status(400).json({ error: 'No folder path provided' });
    }

    console.log("Received analysis request for:", folderPath);
    //!
    let badFiles = []
    
    try {
        const folder_paths = await getPaths(folderPath)
        console.log("All folders paths from outside fnc :\n",folder_paths)
        
        const scanPromises = folder_paths.map(async (fpath) => {
            const files = await fs.promises.readdir(fpath);
            console.log("files in that path : ",files,fpath)

            const filePromises = files.map(async (file) => {
                const filePath = path.join(fpath, file);
                console.log("file path ",filePath)
                const stats = await fs.promises.stat(filePath);
                if (!stats.isDirectory()) {
                    try{
                        const scanResult = await ClamScan.then(x => x.scanFile(filePath));
                        console.log("scan result: ", scanResult);
                        if (!scanResult || scanResult.isInfected=== true) {
                            badFiles.push({ file, scanResult });
                            console.log("array of bad files : ",badFiles)
                            console.log("bad file : ", filePath, "\n result:", scanResult, "\n \n result.isInfected: ", scanResult.isInfected,"\n ---\n");
                        }                
                    }
                    catch (err) {
                        console.error("Error scanning file: ", filePath, err);
                    }
                }
                
                
            });
            const r= await Promise.all(filePromises)
            return r;

        });
        console.log("hello m before the promise all for scan promises")
        const y= await Promise.all(scanPromises);

        console.log("final bad files \n", badFiles);
        let response_to_client = badFiles.length;
        if (response_to_client==0){
            res.json(false);
        }
        else {

            res.json(true);
        }
    }
    catch (error) {
        console.error('Error during file upload:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
  
  
});


/* old one
app.post("/analyze", async function (req, res, next) {
    console.log("Received analysis request");
    ClamScan.then(async (x) =>{
        const result = await x.scanFile('/data/malware.txt');
        return res.json({ result })
    })
});*/
// Error handling middleware
app.use((err, req, res, next) => {
    console.error('Error:', err);
    res.status(500).json({ error: 'Internal server error' });
});

app.listen(3000, function () {
    console.log("Server listening on port 3000");
});

 
