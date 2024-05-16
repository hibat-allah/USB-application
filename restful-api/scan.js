const NodeClam = require('clamscan');
const options = { clamdscan: { host: "clamav", port: 3310 } };
let clamscan;
new NodeClam().init(options).then(x => clamscan=x);

exports.scanDir = async (path) => {
    await clamscan.scanDir(path)
    try {
        const result = await clamscan.scanDir(path);
        console.log("Scan result:", result);
        return result;
    } catch (error) {
        console.error("Error during scan:", error);
        return { error: "An error occurred during the scan." };
    }
}