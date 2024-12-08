import moment from "moment"
import Card from "../components/UI/Card"
import Table from "../components/UI/Tables/Table"
import TableCell from "../components/UI/Tables/TableCell"
import TableRow from "../components/UI/Tables/TableRow"

function LogsPage() {
  return (
    <>
      <h3 className="mb-4"> Logs </h3>
      <Card title="Logs" subtitle="Liste des évenements les plus pertinents récemment">
        <Table fields={["#", "Username", "Machine", "Équipment", "Action", "Timestamp"]}>
          <TableRow>
            <TableCell>1</TableCell>
            <TableCell>mdjassia</TableCell>
            <TableCell>DESKTOP-KSQD651</TableCell>
            <TableCell>PID=0351&VID=5606</TableCell>
            <TableCell className="!text-left">Lorem ipsum dolor sit, amet consectetur adipisicing elit. Deserunt voluptatibus at debitis voluptate provident illo repellendus placeat! Laboriosam nulla quia voluptatum fugiat veniam itaque mollitia nihil beatae cumque, a laudantium?</TableCell>
            <TableCell>{moment(new Date()).format("DD/MM/YYYY HH:MM")}</TableCell>
          </TableRow>
        </Table>
      </Card>
    </>
  )
}

export default LogsPage