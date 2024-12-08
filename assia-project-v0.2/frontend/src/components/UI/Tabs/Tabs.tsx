import { ReactNode, useState } from "react"

type TabsProps = {
    type?: string,
    children: ReactNode,
    keepVisible?: boolean,
}

function Tabs({type="vertical", children, keepVisible=false} : TabsProps) {
    const [selected, setSelected] = useState(0);
    
    const activeStyle = 'text-cyan-500 hover:text-cyan-600 border-cyan-500 hover:border-cyan-600'
    const inactiveStyle = 'text-gray-600 hover:text-gray-600 hover:border-gray-300 hover:border-gray-500'
    return (
        <div className={`${type == "vertical"? "grid grid-cols-5" : ""}`}>
            <ul className={`${type == "vertical" ? "col-span-1 space-y-2" : "flex items-center justify-start"} mb-3`}>
                {
                    Array.isArray(children) &&
                    children.map((tab, i)=>{
                        return <li key={`tab${i}`}>
                            <button type="button" className={`${selected == i ? activeStyle : inactiveStyle} pb-4 h-12 flex justify-start items-center text-left w-full ps-2 pe-6 py-3 border-b-2 text-sm`} onClick={() => setSelected(i)}>
                                {tab.props.icon && <i className={`${tab.props.icon} text-[1rem] w-5 sm:me-2`} />}
                                {
                                    type=="horizontal"?
                                    <span className={`${(selected == i) ? "block" : "hidden lg:block"}`}>{tab.props.text}</span>:
                                    <span className={`block`}>{tab.props.text}</span>
                                }
                            </button>
                        </li>
                    })
                }
            </ul>
            {
                Array.isArray(children) && 
                (
                    keepVisible?
                    children.map((x, i) => <div key={i} className={`${i==selected? "" : "hidden"} w-full col-span-4`}>{x}</div>) :
                    children[selected]
                )
            }
        </div>


  )
}

export default Tabs