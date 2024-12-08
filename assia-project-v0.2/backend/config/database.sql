-- Création de la table User
CREATE TABLE Users (
    email VARCHAR(254) PRIMARY KEY,
    userName VARCHAR(100) UNIQUE NOT NULL,
    firstTime BOOLEAN,
    password VARCHAR(100)
);
-- Création de la table Machine
CREATE TABLE Machine (
    nameM VARCHAR(100) PRIMARY KEY,
    typeM VARCHAR(100) NOT NULL DEFAULT 'complet',
    CONSTRAINT typeM_check CHECK (typeM IN ('selectif', 'complet'))
);

-- Création de la table class
CREATE TABLE Class (
    guid VARCHAR(100) PRIMARY KEY,
    chemain VARCHAR(100) NOT NULL,
    type VARCHAR(100) NOT NULL DEFAULT 'hid',
    CONSTRAINT type_check CHECK (type IN ('HidUsb', 'WINUSB', 'RtlWlanu', 'USBSTOR', 'webcam' ,'usbprint' ))
);


-- Création de la table DriverClass
CREATE TABLE DriverClass (
    guid VARCHAR(100),
    nameDrives VARCHAR(100) NOT NULL,
    FOREIGN KEY (guid) REFERENCES Class(guid)
);

-- Création de la table Device
CREATE TABLE Device (
    idDevice VARCHAR(50) PRIMARY KEY,
    nameD VARCHAR(100) NOT NULL,
    fabricant VARCHAR(100) NOT NULL,
    inf TEXT NOT NULL,
    classI_id VARCHAR(100) NOT NULL,
    FOREIGN KEY (classI_id) REFERENCES Class(guid)
);

-- Création de la table DeviceInstalled
CREATE TABLE DeviceInstalled (
    idDevice VARCHAR(20),
    date_installation TIMESTAMP NOT NULL,
    FOREIGN KEY (idDevice) REFERENCES Device(idDevice)
);

-- Création de la table Class_Users
CREATE TABLE Class_Users (
    class_id VARCHAR(100) NOT NULL,
    user_id VARCHAR(254) NOT NULL,
    
    FOREIGN KEY (class_id) REFERENCES Class(guid),
    FOREIGN KEY (user_id) REFERENCES Users(email),
    PRIMARY KEY (class_id, user_id)
);

-- Création de la table Device_Users
CREATE TABLE Device_Users (
    device_id VARCHAR(50) NOT NULL,
    user_id VARCHAR(254) NOT NULL,
    FOREIGN KEY (device_id) REFERENCES Device(idDevice),
    FOREIGN KEY (user_id) REFERENCES Users(email),
    PRIMARY KEY (device_id, user_id)
);

CREATE TABLE Logs (
    log_id SERIAL PRIMARY KEY,
    user_id VARCHAR(254) NOT NULL,
    action_type action_type_enum NOT NULL,
    file_name VARCHAR(100) NOT NULL,
    file_path VARCHAR(100) NOT NULL,
    device_id VARCHAR(20),
    machine_id VARCHAR(100),
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(email),
    FOREIGN KEY (device_id) REFERENCES Device(idDevice),
    FOREIGN KEY (machine_id) REFERENCES Machine(nameM),
    CONSTRAINT action_type_check CHECK (action_type IN ('transfert', 'creation', 'suppression', 'copie'))
);
INSERT INTO Class (guid, chemain, type) VALUES
('{4d36e967-e325-11ce-bfc1-08002be10318}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\USBSTOR', 'USBSTOR'),
('{1ed2bbf9-11f0-4084-b21f-ad83a8e6dcdc}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\usbprint', 'usbprint'),
('{745a17a0-74d3-11d0-b6fe-00a0c90f57da}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\HidUsb', 'HidUsb'),
('{eec5ad98-8080-425f-922a-dabf3de3f69a}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\WINUSB', 'WINUSB'),
('{ca3e7ab9-b4c3-4ae6-8251-579ef933890f}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\usbvideo', 'webcam'),
('{4d36e972-e325-11ce-bfc1-08002be10318}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\RtlWlanu', 'RtlWlanu');
