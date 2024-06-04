type Props = {
    onClick: () => void
    icon: string
    className?: string
}

function IconButton({ onClick, icon, className="" } : Props) {
  return (
      <button type="button" onClick={onClick} className={`w-4 hover:scale-110 ${className}`}>
          <i className={`${icon}`} />
      </button>
  )
}

export default IconButton