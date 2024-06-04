import Card from "../components/UI/Card"

function Dashboard() {
  return (
    <>
      <h3 className="mb-4"> Dashboard </h3>
      <div className="grid grid-cols-4 gap-3 mb-4">
        <Card title="Utilisateurs" className="flex justify-end">
          <h3>
            5
          </h3>
        </Card>
        <Card title="Machines">
          <h3>
            5
          </h3>
        </Card>
        <Card title="Périphériques">
          <h3>
            5
          </h3>
        </Card>
        <Card title="Classes">
          <h3>
            5
          </h3>
        </Card>
      </div>
      <Card title="Graph">
        Test
      </Card>
    </>
  )
}

export default Dashboard