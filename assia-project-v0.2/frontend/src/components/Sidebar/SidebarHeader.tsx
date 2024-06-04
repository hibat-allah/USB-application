type SidebarTitleProps = {
    text: string
}
function TitleSidebar({text} : SidebarTitleProps){
    return (
        <li className="w-full mt-4">
            <h6 className="pl-6 font-bold leading-tight uppercase text-xs opacity-60">{text}</h6>
        </li>
    )
}

export default TitleSidebar