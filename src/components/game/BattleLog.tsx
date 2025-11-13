'use client'
import { Box, Typography, Paper } from '@mui/material'
import { BattleLogEntry } from '@/types/game'
import { useEffect, useRef } from 'react'

interface BattleLogProps {
  logs: BattleLogEntry[]
}

export default function BattleLog({ logs }: BattleLogProps) {
  const logEndRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    logEndRef.current?.scrollIntoView({ behavior: 'smooth' })
  }, [logs])

  const getLogColor = (type: BattleLogEntry['type']) => {
    switch (type) {
      case 'attack':
        return '#f59e0b'
      case 'damage':
        return '#ef4444'
      case 'critical':
        return '#dc2626'
      case 'heal':
        return '#10b981'
      case 'status':
        return '#8b5cf6'
      case 'info':
        return '#6b7280'
      default:
        return '#ffffff'
    }
  }

  return (
    <Paper
      elevation={0}
      className="p-4 bg-black/30 backdrop-blur-sm border border-white/10 h-64 overflow-y-auto"
    >
      <Typography variant="h6" className="mb-3 text-white font-bold">
        ⚔️ Log de Batalha
      </Typography>
      <Box className="space-y-2">
        {logs.length === 0 ? (
          <Typography variant="body2" className="text-white/50 italic">
            Nenhuma ação ainda...
          </Typography>
        ) : (
          logs.map((log) => (
            <Box
              key={log.id}
              className="p-2 rounded bg-white/5 border-l-4"
              sx={{ borderLeftColor: getLogColor(log.type) }}
            >
              <Typography
                variant="body2"
                className="text-white/90"
                sx={{ color: getLogColor(log.type) }}
              >
                {log.message}
              </Typography>
            </Box>
          ))
        )}
        <div ref={logEndRef} />
      </Box>
    </Paper>
  )
}
