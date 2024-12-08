import React, { ReactNode } from 'react'

type Props = {
    onClick: React.MouseEventHandler<HTMLButtonElement>,
    type?: 'submit' | 'reset' | 'button',
    theme: 'primary' | 'danger' | 'primary-alternate' | 'alternate',
    children: ReactNode,
    className?: string
}

const THEMES = {
    'primary': 'border text-violet-500 border-violet-500 rounded hover:bg-violet-500 hover:text-white',
    'danger': 'border text-red-600 border-red-600 rounded hover:bg-red-500 hover:text-white',
    'primary-alternate': 'border text-white border-violet-500 rounded bg-violet-500 hover:bg-violet-600 hover:text-white',
    'alternate': 'text-violet-600 font-semibold hover:text-violet-700'
}

function Button({ onClick, type='button', theme, children, className='' }: Props) {
  return (
      <button type={type} onClick={onClick} className={`flex items-center justify-center py-2 px-4 font-semibold transition ${THEMES[theme]} ${className}`} >
          {children}
      </button>
  )
}

export default Button