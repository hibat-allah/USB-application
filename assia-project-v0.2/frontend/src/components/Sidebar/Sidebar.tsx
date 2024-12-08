import { Link, useNavigate } from "react-router-dom";
import logo from "../../assets/react.svg";
import SidebarButton from "./SidebarButton";
import { logout } from "../../hooks/useAuth";

type SidebarProps = {
  setOpen: () => void;
};

function Sidebar({ setOpen }: SidebarProps) {
  const navigate = useNavigate();
  return (
    <aside id="sidenav" className="max-w-62.5 shadow-lg ease-nav-brand z-10 fixed inset-y-0 block w-full -translate-x-full flex-wrap items-center justify-between overflow-y-auto border-0 bg-white p-0 antialiased shadow-none transition-transform duration-200 xl:left-0 xl:translate-x-0 xl:bg-white">
      <div className="flex items-center justify-between flex-shrink-0 px-3 ml-4">
        <Link to="/" className="inline-flex items-center gap-2 mt-6 mb-2 h-6" >
          <img src={logo} className="w-10" />
          <span className="font-semibold transition-all duration-200 ease-nav-brand">
            USB Defender
          </span>
        </Link>
        <i id="sidenav_close_button" className="absolute top-0 right-0 hidden p-4 opacity-50 cursor-pointer fas fa-times text-slate-400 xl:hidden" onClick={setOpen} />
      </div>
      <hr className="h-px mt-0 bg-transparent bg-gradient-to-r from-transparent via-black/40 to-transparent" />
      <div className="block w-auto overflow-auto flex flex-col justify-between items-start">
        <ul className="flex flex-col pl-0 pt-2 pb-4 w-full h-[87vh] overflow-auto">
          <SidebarButton text="Dashboard" icon="fa-brands fa-windows" route="/" closeSidebar={setOpen} />
          <SidebarButton text="Utilisateurs" icon="fa fa-user" route="/users" closeSidebar={setOpen} />
          <SidebarButton text="Machines" icon="fa fa-computer" route="/machines" closeSidebar={setOpen} />
          <SidebarButton text="Périphériques" icon="fa-brands fa-usb" route="/peripherals" closeSidebar={setOpen} />
          <SidebarButton text="Classes" icon="fa fa-book" route="/classes" closeSidebar={setOpen} />
          <SidebarButton text="Logs" icon="fa fa-clock-rotate-left" route="/logs" closeSidebar={setOpen} />
        </ul>
        <button className="self-center py-1 text-sm ease-nav-brand my-1 mx-2 flex items-center whitespace-nowrap px-4" onClick={() => logout(navigate)}> Logout </button>
      </div>
    </aside>
  );
}

export default Sidebar;
