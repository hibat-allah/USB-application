import { ReactNode } from "react"

type TabContent = {
    icon?: string
    text: string
    children: ReactNode,
    className?: string
}
function TabContent({ children, className='' }: TabContent) {
  return (
      <div className={`p-6 w-full bg-gray-50/50 col-span-4 ${className}`}>
          {children}
      </div>
  )
}

export default TabContent