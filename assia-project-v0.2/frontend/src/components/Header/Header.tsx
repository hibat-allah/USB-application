import { useContext } from 'react';
import AuthContext from '../../hooks/AuthContext';

function Header() {
  const auth = useContext(AuthContext);

  return <nav className="pr-2 relative flex bg-violet-500 flex-wrap items-center justify-between py-2 transition-all shadow-none duration-250 ease-soft-in lg:flex-nowrap lg:justify-start">
    <div className="flex items-center justify-between w-full px-4 py-1 mx-auto flex-wrap-inherit">
      <div className="flex justify-start mt-2 grow sm:mt-0 lg:flex lg:basis-auto text-white">
        <span><span className="font-semibold">Hello,</span>{" "}{auth?.username}</span>
      </div>
      
    </div>
  </nav>
}

export default Header;