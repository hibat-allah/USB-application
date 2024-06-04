import { ReactNode } from "react";
import { Navigate, NavigateFunction } from 'react-router-dom'
import { jwtDecode } from "jwt-decode";
export type JWTToken = {
    username: string,
}

export function getJWTContent(){
    const jwt = sessionStorage.getItem("jwt") ?? localStorage.getItem('jwt')
    return jwt? jwtDecode<JWTToken>(jwt) : null
}

export function isAuthenticated(){
    return getJWTContent()?.username !== undefined
}
export async function login(jwt : string, rememberme: boolean){
    if(!rememberme)
        sessionStorage.setItem("jwt", jwt);
    else
        localStorage.setItem("jwt", jwt);
}

export async function logout(navigate : NavigateFunction){
    sessionStorage.removeItem('jwt');
    localStorage.removeItem('jwt');
    navigate(0);
}

type Props = {
    children: ReactNode
}

export function PrivateRouteOnly({ children } : Props) {
    return isAuthenticated() ? children : <Navigate to="/" />;
}

export function PublicOrPrivateRoute({ loggedIn, notLoggedIn } : {loggedIn: ReactNode, notLoggedIn: ReactNode}) {
    return isAuthenticated() ? loggedIn : notLoggedIn;
}