import { ReactNode } from "react"

type AlertProps = {
    color: string,
    children: ReactNode,
    className?: string,
}

function Alert({ color, children, className="" }: AlertProps) {
  return (
    <div className={`relative w-full px-4 py-3 text-white rounded-lg ${color} mb-3 ${className}`}>{children}</div>
  )
}

export default Alert