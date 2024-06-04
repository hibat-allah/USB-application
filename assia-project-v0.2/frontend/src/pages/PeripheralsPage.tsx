import moment from "moment";
import Card from "../components/UI/Card";
import Table from "../components/UI/Tables/Table";
import TableCell from "../components/UI/Tables/TableCell";
import TableRow from "../components/UI/Tables/TableRow";
import Badge from "../components/UI/Badge";
import DeleteButton from "../components/UI/Buttons/DeleteButton";
import Button from "../components/UI/Buttons/Button";
import { useState, useEffect } from "react";
import { baseURL } from "../config";
import axios from "axios";
import ModalExample from "./Modals/ModalDevices";

function PeripheralsPage() {
  const [openModal, setOpenModal] = useState<string>("");
  const addButton = <Button type="button" theme="primary-alternate" onClick={() => setOpenModal("add")}>Ajouter</Button>;

  const [peripherique, setPeripherique] = useState<any[]>([]);
  useEffect(() => {
    axios.get(`${baseURL}/peripherique`).then((response) => {
      setPeripherique(response.data);
    });
  }, []);


  

  const [deviceUsers, setDeviceUsers] = useState<any[]>([]);
  useEffect(() => {
    axios.get(`${baseURL}/DeviceUsers`).then((response) => {
      setDeviceUsers(response.data);
    });
  }, []);

   const [users, setUsers] = useState<any[]>([]);
  useEffect(() => {
    axios.get(`${baseURL}/users`).then((response) => {
      setUsers(response.data);
    });
  }, []);

  const getUsersForDevice = (deviceId: string) => {
    return deviceUsers
      .filter(du => du.device_id === deviceId)
      .map(du => users.find(user => user.email === du.user_id)?.username || "Unknown User")
      .join(", ");
  };
  
  

  return (
    <>
      <h3 className="mb-4"> Périphériques </h3>
      <Card title="Périphériques" subtitle="Liste des périphériques" action={addButton}>
        <Table fields={["#", "id", "nom peripherique", "classe peripherique", "utilisateurs"]}>
          {
            peripherique.map((x, i) => (
              <TableRow >
                <TableCell>{i + 1}</TableCell>
                <TableCell>{x.iddevice}</TableCell>
                <TableCell>{x.named}</TableCell>
                <TableCell>{x.classi_id}</TableCell>
                <TableCell>{getUsersForDevice(x.iddevice)}</TableCell>
                <TableCell>
                  <DeleteButton onClick={() => null} />
                </TableCell>
              </TableRow>
            ))
          }
        </Table>
      </Card>
      <ModalExample isOpen={openModal === "add"} close={() => setOpenModal("")} />
    </>
  );
}

export default PeripheralsPage;
