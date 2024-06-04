import { createBrowserRouter } from "react-router-dom";
import { PrivateRouteOnly, PublicOrPrivateRoute } from "./hooks/useAuth";
import LoginPage from "./pages/LoginPage";
import Dashboard from "./pages/Dashboard";
import Scaffold from "./components/Scaffold";
import MachinesPage from "./pages/MachinesPage";
import UsersPage from "./pages/UsersPage";
import LogsPage from "./pages/LogsPage";
import PeripheralsPage from "./pages/PeripheralsPage";
import ClassesPage from "./pages/ClassesPage";

const router = createBrowserRouter([
  { path: "/", element:(
    <PublicOrPrivateRoute 
      notLoggedIn={(<LoginPage />)}
      loggedIn={<Scaffold> <Dashboard /> </Scaffold>}
    />)
  },

  { path: "/users", element: (
    <PrivateRouteOnly>
      <Scaffold> <UsersPage /> </Scaffold>
    </PrivateRouteOnly>
  ) },
  { path: "/machines", element: (
    <PrivateRouteOnly>
      <Scaffold> <MachinesPage /> </Scaffold>
    </PrivateRouteOnly>
  ) },
  {
    path: "/peripherals", element: (
      <PrivateRouteOnly>
        <Scaffold> <PeripheralsPage /> </Scaffold>
      </PrivateRouteOnly>
    )
  },
  { path: "/classes", element: (
      <PrivateRouteOnly>
        <Scaffold> <ClassesPage /> </Scaffold>
      </PrivateRouteOnly>
    )
  },
  { path: "/logs", element: (
    <PrivateRouteOnly>
      <Scaffold> <LogsPage /> </Scaffold>
    </PrivateRouteOnly>
  ) },
]);

export default router;
