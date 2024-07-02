----- creating users
CREATE USER zflub WITH PASSWORD 'M#N@d31N';
CREATE USER xlient WITH PASSWORD 'M4B1U#';

GRANT ALL PRIVILEGES ON DATABASE entreprisebd TO zflub;
ALTER DATABASE entreprisebd OWNER TO zflub;
GRANT USAGE ON SCHEMA public TO zflub;

GRANT SELECT, INSERT,UPDATE,DELETE ON ALL TABLES IN SCHEMA public TO zflub;

GRANT SELECT ON ALL TABLES IN SCHEMA public TO xlient;
-- this after creating tables
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO zflub;


-- Ensure regular_user has usage rights on the schema
GRANT USAGE ON SCHEMA public TO xlient;

-- droping all tables on the schema
psql -d entreprisebd -c "SELECT 'DROP TABLE IF EXISTS ' || tablename || ' CASCADE;' FROM pg_tables WHERE schemaname = 'public';" -t -A -o drop_tables.sql
psql -d entreprisebd -f drop_tables.sql

------------------------------ tables -------------------------------

-- Création de la table User
CREATE TABLE Users (
    email VARCHAR(100),
    userName VARCHAR(100) PRIMARY KEY,
    firstTime BOOLEAN NOT NULL DEFAULT 'FALSE',
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
    description VARCHAR(200) NOT NULL,
    CONSTRAINT type_check CHECK (type IN ('HidUsb', 'WINUSB', 'RtlWlanu', 'USBSTOR', 'webcam' ,'usbprint','USBaudio' ))
);


-- Création de la table DriverClass
CREATE TABLE DriverClass (
    guid VARCHAR(100),
    nameDrives VARCHAR(100) NOT NULL,
    FOREIGN KEY (guid) REFERENCES Class(guid) on delete cascade
);

-- Création de la table Device
CREATE TABLE Device (
    idDevice VARCHAR(100) PRIMARY KEY,
    nameD VARCHAR(100),
    inf TEXT NOT NULL,
    classI_id VARCHAR(100) NOT NULL,
    FOREIGN KEY (classI_id) REFERENCES Class(guid) on delete cascade
);

-- Création de la table DeviceInstalled
CREATE TABLE DeviceInstalled (
    idDevice VARCHAR(100),
    date_installation TIMESTAMP NOT NULL,
    FOREIGN KEY (idDevice) REFERENCES Device(idDevice) on delete cascade
);

-- Création de la table Class_Users
CREATE TABLE Class_Users (
    class_id VARCHAR(100) NOT NULL,
    user_id VARCHAR(254) NOT NULL,
    
    FOREIGN KEY (class_id) REFERENCES Class(guid) on delete cascade,
    FOREIGN KEY (user_id) REFERENCES Users(userName) on delete cascade ,
    PRIMARY KEY (class_id, user_id)
);

-- Création de la table Device_Users
CREATE TABLE Device_Users (
    device_id VARCHAR(100) NOT NULL,
    user_id VARCHAR(100) NOT NULL,
    FOREIGN KEY (device_id) REFERENCES Device(idDevice) on delete cascade, 
    FOREIGN KEY (user_id) REFERENCES Users(userName) on delete cascade,
    PRIMARY KEY (device_id, user_id)
);

CREATE TABLE Logs (
    log_id SERIAL PRIMARY KEY,
    user_id VARCHAR(100) NOT NULL,
    action_type VARCHAR(100) NOT NULL,
    file_name VARCHAR(100),
    file_path VARCHAR(100),
    device_id VARCHAR(100) NOT NULL,
    machine_id VARCHAR(100)NOT NULL,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(userName)  on delete cascade,
    FOREIGN KEY (device_id) REFERENCES Device(idDevice)  on delete cascade,
    FOREIGN KEY (machine_id) REFERENCES Machine(nameM) on delete cascade,
    CONSTRAINT action_type_check CHECK (action_type IN ('transfert', 'creation', 'connexion','modified', 'copie'))
);
CREATE TABLE Admin ( 
     email VARCHAR(100) UNIQUE,
    userName VARCHAR(100)  PRIMARY KEY  ,
    password VARCHAR(100)
);

------------------------ grant to users the privs
grant usage,select on sequence logs_log_id_seq to zflub;
grant select, insert,update on table logs to zflub;
GRANT SELECT, INSERT,UPDATE,DELETE ON ALL TABLES IN SCHEMA public TO zflub;


------------------------ Insertions 

INSERT INTO Users (email, userName, firstTime, password)
VALUES ('user@entreprisemail.dz', 'user', false, NULL);

INSERT INTO Users (email, userName, firstTime, password)
VALUES ('hiba@entreprisemail.dz', 'hiba', false, NULL);


INSERT INTO Class (guid, chemain, type, description) VALUES
('{4d36e967-e325-11ce-bfc1-08002be10318}', 'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR', 'USBSTOR', 'Peripherique de stockage USB'),
('{1ed2bbf9-11f0-4084-b21f-ad83a8e6dcdc}', 'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbprint', 'usbprint','Imprimante USB'),
('{745a17a0-74d3-11d0-b6fe-00a0c90f57da}', 'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HidUsb', 'HidUsb','Peripherique d interface humaine (souris / claviers...)'),
('{eec5ad98-8080-425f-922a-dabf3de3f69a}', 'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WINUSB', 'WINUSB','Disque dur externe'),
('{ca3e7ab9-b4c3-4ae6-8251-579ef933890f}', 'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbvideo', 'webcam','Peripherique video USB (webcam)'),
('{4d36e972-e325-11ce-bfc1-08002be10318}', 'HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RtlWlanu', 'RtlWlanu','Carte réseau sans fil USB'),
('{6bdd1fc6-810f-11d0-bec7-08002be2092f}', 'HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\usbaudio2','USBaudio','les périphériques audio');

INSERT INTO device VALUES 
('USB\\VID_058F&PID_6387', 'qsd', 'usbstor.inf','{4d36e967-e325-11ce-bfc1-08002be10318}');


INSERT INTO  device_users (device_id, user_id) VALUES ('USB\\VID_058F&PID_6387', 'user');


INSERT INTO class_users Values ('{4d36e967-e325-11ce-bfc1-08002be10318}', 'user');


INSERT INTO class_users Values ('{4d36e967-e325-11ce-bfc1-08002be10318}', 'hiba');

INSERT INTO  device_users (device_id, user_id) VALUES ('USB\\VID_058F&PID_6387', 'hiba');


INSERT INTO  device_users (device_id, user_id) VALUES ('USB\\VID_058F&PID_6387', 'user');


Insert into machine values ('DESKTOP-BV51BCO','selectif') On conflict (nameM) DO NOTHING;

Insert into machine values ('PC1','complet') On conflict (nameM) DO NOTHING;

Insert into DriverClass values ('{88BAE032-5A81-49F0-BC3D-A4FF138216D6}','USB\Class_02');
Insert into DriverClass values ('{4d36e967-e325-11ce-bfc1-08002be10318}','STORAGE\VOLUME');
Insert into DriverClass values ('{4d36e967-e325-11ce-bfc1-08002be10318}','GenDisk');
Insert into DriverClass values ('{4d36e967-e325-11ce-bfc1-08002be10318}','USBSTOR\DISK');
Insert into DriverClass values ('{745a17a0-74d3-11d0-b6fe-00a0c90f57da}','HID\DEVICE');
Insert into DriverClass values ('{745a17a0-74d3-11d0-b6fe-00a0c90f57da}','USB\Class_03');

--deletion of the content
delete from ..;


DROP TABLE table_name CASCADE;

--show all tables
SELECT tablename
FROM pg_tables
WHERE schemaname = 'public';


-- select the contraint name
SELECT conname
FROM pg_constraint
WHERE conrelid = 'Logs'::regclass
AND contype = 'f';


-- show content of tables
select * from users;
select * from machine;
select * from class;
 select * from driverclass;
 select * from class_users;
 select * from admin;
select * from device;
select * from deviceinstalled;
select * from device_users;
select * from logs;

-- updating the type of a machine
UPDATE Machine
SET typeM = 'selectif'
WHERE nameM = 'PC1';


