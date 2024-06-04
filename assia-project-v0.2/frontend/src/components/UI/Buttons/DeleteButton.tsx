type Props = {
    onClick: () => void
}

function DeleteButton({onClick} : Props) {
  return (
      <button type="button" onClick={onClick} className="w-4 text-red-500 hover:text-red-700 hover:scale-110">
          <i className="fa fa-trash" />
      </button>
  )
}

export default DeleteButton