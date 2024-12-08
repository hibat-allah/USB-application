import Sidebar from './Sidebar/Sidebar'
import Header from './Header/Header'
import { ReactNode, useEffect, useState } from 'react'

type ScaffoldProps = {
  children : ReactNode
}

function Scaffold({ children } : ScaffoldProps) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  
  useEffect(()=>{
    document.getElementById('sidenav_close_button')?.classList.toggle("hidden");
    document.getElementById('sidenav')?.classList.toggle("translate-x-0");
    document.getElementById('sidenav')?.classList.toggle("shadow-soft-xl");
    document.getElementById('top_bread')?.classList.toggle("translate-x-[5px]");
    document.getElementById('bottom_bread')?.classList.toggle("translate-x-[5px]");
  }, [sidebarOpen]);

  return (
    <>
      <Sidebar setOpen={() => setSidebarOpen(!sidebarOpen)} />
      <Header />
      <main className='min-h-[90.75vh] flex flex-col justify-between'>
        <div className='w-full px-2 sm:px-6 py-6 mx-auto flex flex-col justify-center flex-wrap'>
          { children }
        </div>
      </main>
    </>
  )
}

export default Scaffold