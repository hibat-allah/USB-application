import { ReactNode } from "react"

type CardProps = {
    title?: string,
    subtitle?: string,
    children: ReactNode,
    action?: ReactNode,
    className?: string,
}

function Card({ title, subtitle, children, action, className='' }: CardProps) {
  return (
      <div className={`relative flex flex-col rounded-lg bg-white break-words shadow-soft-xl px-6 py-4 mb-3 ${className}`}>
          {(title || subtitle) &&
          <div className="flex justify-between items-center mb-2">
                <div className="border-black/12.5 mb-0 rounded-lg border-b-0 border-solid bg-white pb-0">
                    {title && <h2 className="mb-0 text-xl">{title}</h2>}
                    {subtitle && <p className="leading-normal text-sm mb-0">{subtitle}</p>}
                </div>
                {action}
          </div>
          }
          <div className="block w-full">
              {children}
          </div>
      </div>
  )
}

export default Card