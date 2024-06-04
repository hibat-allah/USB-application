import moment from "moment"
import Button from "../components/UI/Buttons/Button"
import Card from "../components/UI/Card"
import Table from "../components/UI/Tables/Table"
import TableCell from "../components/UI/Tables/TableCell"
import TableRow from "../components/UI/Tables/TableRow"
import DeleteButton from "../components/UI/Buttons/DeleteButton"
import Badge from "../components/UI/Badge"
import { useState } from "react"
import ModalClassUser from "./Modals/ModalClassUser"
import { useEffect } from "react"
import axios from "axios"
import { baseURL } from "../config"

function ClassesPage() {
  const [openModal, setOpenModal] = useState<string>("");
  const addButton = <Button type="button" theme="primary-alternate" onClick={() => setOpenModal("add")}>Ajouter</Button>
  const [classes, setClasses] = useState<any[]>();
  useEffect(()=>{
      axios.get(`${baseURL}/class`).then((response) => {
        setClasses(response.data)
      })
  }, [])

  return (
    <>
      <h3 className="mb-4"> Classes </h3>
      <Card title="Classes" subtitle="Liste des évenements les plus pertinents récemment" action={addButton}>
        <Table fields={["#", "guids" ,"chemain" , "type"]}>
        {
  classes?.map((x, i)=> (
    <TableRow>
      <TableCell>{i+1}</TableCell>
      <TableCell>{x.guid}</TableCell>
      <TableCell>{x.chemain}</TableCell>

      <TableCell><Badge bgColor="#7E57C2" textColor="#EDE7F6">{x.type}</Badge></TableCell>
      <TableCell>
        <DeleteButton onClick={() => null} />
      </TableCell>
    </TableRow>
  ))
}
        </Table>
      </Card>
      <ModalClassUser isOpen={openModal === "add"} close={() => setOpenModal("")} />
    </>
  )
}



export default ClassesPage