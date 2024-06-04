/* eslint-disable @typescript-eslint/no-explicit-any */
import Button from "../components/UI/Buttons/Button"
import login_img from "../assets/login.jpg"
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import axios from "axios";
import { login } from "../hooks/useAuth";
import { baseURL } from "../config"

function LoginPage() {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  async function handleLogin(e: any) {
    e.preventDefault();
    // submit login
    const body = { username: username, password: password, type: "personnel" };
    try {
      const response = await axios.post(`${baseURL}/login`, body);
      const data = response.data;

      login(data, true)
      navigate(0);
    } catch (err: any) {
      if (err.response)
        alert(err.response.data)
      else if (err.request)
        alert("Échec Réseau");
    }
  }

  return (
    <div className="flex items-center min-h-screen p-6 bg-gray-50 dark:bg-gray-900">
      <div className="flex-1 h-full max-w-4xl mx-auto overflow-hidden bg-white rounded-lg shadow-xl dark:bg-gray-800">
        <div className="flex flex-col overflow-y-auto md:flex-row">
          <div className="h-32 md:h-auto md:w-1/2 min-h-[550px]">
            <img
              aria-hidden="true"
              className="object-cover w-full h-full dark:hidden"
              src={login_img}
              alt="Office"
            />
          </div>
          <div className="flex items-center justify-center p-6 sm:p-12 md:w-1/2">
            <div className="w-full">
              <h1
                className="mb-4 text-xl font-semibold text-gray-700 dark:text-gray-200"
              >
                Login
              </h1>
              <label className="block text-sm">
                <span className="text-gray-700 dark:text-gray-400">Username</span>
                <input
                  className="block w-full mt-1 text-sm dark:border-gray-600 dark:bg-gray-700 focus:border-purple-400 focus:outline-none focus:shadow-outline-purple dark:text-gray-300 dark:focus:shadow-outline-gray form-input"
                  placeholder="Jane Doe"
                  value={username}
                  onChange={(e) => setUsername!(e.target.value)}
                />
              </label>
              <label className="block mt-4 text-sm">
                <span className="text-gray-700 dark:text-gray-400">Password</span>
                <input
                  className="block w-full mt-1 text-sm dark:border-gray-600 dark:bg-gray-700 focus:border-purple-400 focus:outline-none focus:shadow-outline-purple dark:text-gray-300 dark:focus:shadow-outline-gray form-input"
                  placeholder="***************"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword!(e.target.value)}
                />
              </label>

              <Button
                type="submit"
                onClick={handleLogin}
                theme="primary-alternate"
                className="w-full mt-4"
              >
                Log in
              </Button>

              <hr className="my-8" />

              <p className="mt-4">
                <a
                  className="text-sm font-medium text-purple-600 dark:text-purple-400 hover:underline"
                  href="./forgot-password.html"
                >
                  Forgot your password?
                </a>
              </p>
              <p className="mt-1">
                <a
                  className="text-sm font-medium text-purple-600 dark:text-purple-400 hover:underline"
                  href="./create-account.html"
                >
                  Create account
                </a>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default LoginPage