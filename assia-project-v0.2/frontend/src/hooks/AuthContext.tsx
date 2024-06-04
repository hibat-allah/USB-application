import { createContext } from "react";
import { JWTToken } from "./useAuth";

const AuthContext = createContext<JWTToken | null>(null);

export default AuthContext
