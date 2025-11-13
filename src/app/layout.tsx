import type { Metadata } from 'next'
import { AppRouterCacheProvider } from '@mui/material-nextjs/v14-appRouter'
import ThemeRegistry from '@/components/ThemeRegistry'
import './globals.css'

export const metadata: Metadata = {
  title: '8dugeon',
  description: 'Um projeto simples com Next.js e TypeScript',
}

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="pt-BR">
      <body id="__next">
        <AppRouterCacheProvider>
          <ThemeRegistry>{children}</ThemeRegistry>
        </AppRouterCacheProvider>
      </body>
    </html>
  )
}
