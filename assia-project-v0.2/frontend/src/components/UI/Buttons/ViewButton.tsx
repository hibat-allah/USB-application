type Props = {
    onClick: () => void
}

function ViewButton({ onClick } : Props) {
  return (
      <button type="button" onClick={onClick} className="w-4 text-green-500 hover:text-green-700 hover:scale-110">
            <i className="fa fa-eye" />
      </button>
  )
}

export default ViewButton