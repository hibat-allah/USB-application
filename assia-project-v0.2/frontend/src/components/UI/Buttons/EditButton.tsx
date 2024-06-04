type Props = {
    onClick: () => void
}

function EditButton({ onClick } : Props) {
  return (
      <button type="button" onClick={onClick} className="w-4 text-yellow-500 hover:text-yellow-700 hover:scale-110">
            <i className="fa fa-edit" />
      </button>
  )
}

export default EditButton