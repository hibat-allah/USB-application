import moment from "moment"
import Button from "../components/UI/Buttons/Button"
import Card from "../components/UI/Card"
import Table from "../components/UI/Tables/Table"
import TableCell from "../components/UI/Tables/TableCell"
import TableRow from "../components/UI/Tables/TableRow"
import Badge from "../components/UI/Badge"
import DeleteButton from "../components/UI/Buttons/DeleteButton"
import ModalExample from "./Modals/ModalExample"
import { useEffect, useState } from "react"
import axios from "axios"
import { baseURL } from "../config"

function UsersPage() {
  const [ openModal, setOpenModal ] = useState<string>("");
  const addButton = <Button type="button" theme="primary-alternate" onClick={() => setOpenModal("add")}>Ajouter</Button>

  const [users, setUsers] = useState<any[]>();
  useEffect(()=>{
      axios.get(`${baseURL}/users`).then((response) => {
        setUsers(response.data)
      })
  }, [])


  return (
    <>
      <h3 className="mb-4"> Utilisateurs </h3>
      <Card title="Liste des utilisateurs" subtitle="Liste des utilisateurs existant dans le système" action={addButton}>
        <Table fields={["#", "email", "username", "Prénom", "Crée le", "Rôle", ""]}>
          {
            users?.map((x, i)=> (
              <TableRow>
                <TableCell>{i+1}</TableCell>
                <TableCell>{x.email}</TableCell>
                <TableCell>{x.userName}</TableCell>
                <TableCell>{x.prénom}</TableCell>
                
                <TableCell>{moment(new Date()).format("DD/MM/YYYY HH:MM")}</TableCell>
                <TableCell><Badge bgColor="#7E57C2" textColor="#EDE7F6">Admin</Badge></TableCell>
                <TableCell>
                  <DeleteButton onClick={() => null} />
                </TableCell>
              </TableRow>
            ))
          }
          
        </Table>
      </Card>
      <ModalExample isOpen={openModal==="add"} close={()=>setOpenModal("")} />
    </>
  )
}

export default UsersPage