
const express = require("express");
const NodeClam = require('clamscan');
const fs = require('fs');
const path = require('path');

const ClamScan = new NodeClam().init({
    clamdscan:{
        host: "clamav",
        port: 3310
    }
});
app = express();

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
    const folderPath = '/data'; // Change this to the folder path where files are copied in the container
    // console.log("Received analysis request");
    const badFiles = []
    
    const folder_paths = await getPaths(folderPath)
    console.log("All folders paths from outside fnc :\n",folder_paths)
    for (const fpath of folder_paths){
        // console.log("scanning folder....: fpath=",fpath)
        const files = await fs.promises.readdir(fpath);
        for (const file of files) {
            const filePath = path.join(fpath, file);
            // console.log("reading file....:",filePath)
            // we scan the file
            ClamScan.then(async (x) =>{
                const result = await x.scanFile(filePath);
               
                if (!result || result.isInfected === 'true') {
                    badFiles.push({ file, result });
                    console.log("bad file ",filePath,"\n result:",result,"\n --- \n result.isInfected: ",result.isInfected)
                }
            })
            
            
            }
    }

        return res.json({ badFiles  });
    // });
  
});
/* old one
app.post("/analyze", async function (req, res, next) {
    console.log("Received analysis request");
    ClamScan.then(async (x) =>{
        const result = await x.scanFile('/data/malware.txt');
        return res.json({ result })
    })
});*/
app.listen(3000, function () {
    console.log("Server listening on port 3000");
});

 