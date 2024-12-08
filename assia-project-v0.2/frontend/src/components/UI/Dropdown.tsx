import { ChevronDownIcon } from "@heroicons/react/24/outline"
import { ReactNode, useEffect, useRef, useState } from "react"
import ElementPopper from "react-element-popper"
type Props = {
    children: ReactNode
    text: string
}

export default function Dropdown({children, text} : Props) {
  const [active, setActive] = useState<any>(false)
  const ref = useRef<any>()
  const toggleDropDown = () => setActive(!active)

  useEffect(() => {
    function handleClickOutside(e : any) {
        if (ref.current && !ref.current.contains(e.target)) {
        setActive(false)
        }
    }
  
    document.addEventListener("click", handleClickOutside)
    return () => document.removeEventListener("click", handleClickOutside)
  }, [])

  const button = (
    <button onClick={toggleDropDown} className="flex w-full w-[8.5rem] items-center justify-between rounded-md bg-cyan-400 px-4 py-2 text-white focus:outline-none focus-visible:ring-2 focus-visible:ring-white/75 font-semibold">
        {text}
        <ChevronDownIcon className="-mr-1 ml-2 h-5 w-5 text-white" aria-hidden="true"/>
    </button>
  )

  return (
    <ElementPopper ref={ref} element={button} popper={active && children} position="bottom-left"/>
  )
}