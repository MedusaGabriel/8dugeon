'use client'
import { Button as MuiButton, ButtonProps as MuiButtonProps } from '@mui/material'
import { ReactNode } from 'react'

interface ButtonProps extends MuiButtonProps {
  children: ReactNode
}

export default function Button({ children, className, ...props }: ButtonProps) {
  return (
    <MuiButton className={`transition-all duration-300 ${className || ''}`} {...props}>
      {children}
    </MuiButton>
  )
}
