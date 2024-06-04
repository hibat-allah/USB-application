import { ReactNode, createContext, useState } from "react";
import Alert from "../components/UI/Alert";
import { Transition } from "@headlessui/react";
type Props = {
    children: ReactNode
}
type Alert = {
    show: boolean,
    theme: 'error' | 'warning' | 'success',
    message: string,
}
type AlertsContextExport = {
    showAlert: (arg0: Alert['theme'], arg1: Alert['message']) => void
    clearAlert: () => void
}

const AlertsContext = createContext<AlertsContextExport>({
    showAlert: () => {},
    clearAlert: () => {}
});
const THEMES = {
    "error": "bg-red-400",
    "warning": "bg-yellow-400",
    "success": "bg-green-400"
}

export const AlertsProvider = (({children} : Props) => {
    const [alert, setAlert] = useState<Alert>({
        show: false,
        theme: "error",
        message: "",
    });

    const showAlert = (theme: Alert['theme'], message: Alert["message"]) => {
        setAlert({show:true, theme, message})
        setTimeout(clearAlert, 3000)
    }
    const clearAlert = () => {
        setAlert(a => ({...a, show:false}))
    }
    
    return (
        <AlertsContext.Provider value={{showAlert, clearAlert} }>
            <Transition className="fixed z-100 max-w-5xl left-0 right-0 mx-auto top-[2vh] p-0 m-0 shadow-2xl rounded-lg"
                enter="transition ease duration-300 transform"
                enterFrom="-translate-y-24"
                enterTo="translate-y-0"
                leave="transition ease duration-300 transform"
                leaveFrom="translate-y-0"
                leaveTo="-translate-y-24"
                show={alert.show}>
                <Alert color={THEMES[alert.theme]} className="!m-0 font-semibold">
                    {alert?.message}
                    <button className="text-white absolute right-4" onClick={clearAlert}>
                        <i className="fa fa-close text-xl"/>
                    </button>
                </Alert>
            </Transition>
            {children}
        </AlertsContext.Provider>
    )
})

export default AlertsContext