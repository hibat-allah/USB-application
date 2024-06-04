/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import AuthContext from './hooks/AuthContext'
import { AlertsProvider } from './hooks/AlertsContext'
import { RouterProvider } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { getJWTContent } from './hooks/useAuth'
import router from './router';
import axios from 'axios';
axios.defaults.headers.common.Authorization = "Bearer " + (sessionStorage.getItem('jwt') ?? localStorage.getItem('jwt'))

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: (failures_count, error) => failures_count < 3 && (error as any).response.status != 401
    }
  }
})

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AuthContext.Provider value={getJWTContent()}>
      <AlertsProvider>
        <QueryClientProvider client={queryClient}>
          <RouterProvider router={router} />
        </QueryClientProvider>
      </AlertsProvider>
    </AuthContext.Provider>
  </React.StrictMode>,
)
