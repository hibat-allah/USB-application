import { Link } from "react-router-dom"

type SidebarButtonProps = {
    icon: string,
    text: string,
    route: string,
    closeSidebar?: () => void
}
function SidebarButton({icon, text, route, closeSidebar} : SidebarButtonProps){
    const isActive = window.location.pathname === route
    return (
        <li className={`w-full relative transition-colors ${isActive? 'active' : ''}`}>
            <Link className="py-1 text-sm ease-nav-brand my-1 mx-2 flex items-center whitespace-nowrap px-4" to={route} onClick={closeSidebar}>
                <div className="mr-2 flex h-8 w-8 items-center justify-center rounded-lg bg-center stroke-0 text-center xl:p-2.5">
                    <i className={`cursor-pointer ${icon} text-lg`} />
                </div>
                <span className="ml-1 duration-300 opacity-100 pointer-events-none ease-soft">{text}</span>
            </Link>
        </li>
    )
}

export default SidebarButton