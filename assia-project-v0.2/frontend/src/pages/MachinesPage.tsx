import moment from "moment"
import Card from "../components/UI/Card"
import Table from "../components/UI/Tables/Table"
import TableCell from "../components/UI/Tables/TableCell"
import TableRow from "../components/UI/Tables/TableRow"
import Badge from "../components/UI/Badge"
import DeleteButton from "../components/UI/Buttons/DeleteButton"
import Button from "../components/UI/Buttons/Button"
import { useState } from "react"
import ModalExample from "./Modals/ModalExample"

function MachinesPage() {
  const [openModal, setOpenModal] = useState<string>("");
  const addButton = <Button type="button" theme="primary-alternate" onClick={() => setOpenModal("add")}>Ajouter</Button>

  return (
    <>
      <h3 className="mb-4"> Machines </h3>
      <Card title="Machines" subtitle="Liste des machines" action={addButton}>
        <Table fields={["#", "Nom du machine", "Type de blocage", "Ajouté le", ""]}>
          <TableRow>
            <TableCell>1</TableCell>
            <TableCell>DESKTOP-KSQD651</TableCell>
            <TableCell><Badge bgColor="#7E57C2" textColor="#EDE7F6">Séléctif</Badge></TableCell>
            <TableCell>{moment(new Date()).format("DD/MM/YYYY HH:MM")}</TableCell>
            <TableCell>
              <DeleteButton onClick={() => null} />
            </TableCell>
          </TableRow>
        </Table>
      </Card>

      <ModalExample isOpen={openModal === "add"} close={() => setOpenModal("")} />
    </>
  )
}

export default MachinesPage