'use client'
import { Box, Typography, Paper } from '@mui/material'
import { Room } from '@/types/game'
import Button from '@/components/ui/Button'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import ArrowForwardIcon from '@mui/icons-material/ArrowForward'
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward'
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward'

interface DungeonViewProps {
  room: Room
  onMove: (direction: 'left' | 'right' | 'forward' | 'back') => void
}

export default function DungeonView({ room, onMove }: DungeonViewProps) {
  const getDirectionIcon = (direction: string) => {
    switch (direction) {
      case 'left':
        return <ArrowBackIcon />
      case 'right':
        return <ArrowForwardIcon />
      case 'forward':
        return <ArrowUpwardIcon />
      case 'back':
        return <ArrowDownwardIcon />
    }
  }

  const getDirectionLabel = (direction: string) => {
    switch (direction) {
      case 'left':
        return '⬅️ Esquerda'
      case 'right':
        return '➡️ Direita'
      case 'forward':
        return '⬆️ Frente'
      case 'back':
        return '⬇️ Trás'
    }
  }

  return (
    <Paper elevation={0} className="p-6 bg-white/5 backdrop-blur-sm border border-white/10">
      <Typography variant="h5" className="mb-4 text-white font-bold">
        🏰 Sala Atual
      </Typography>

      <Typography variant="body1" className="mb-6 text-white/90 whitespace-pre-line">
        {room.description}
      </Typography>

      {room.encounter && !room.visited && (
        <Box className="p-4 mb-4 bg-red-500/20 border border-red-500/50 rounded-lg">
          <Typography variant="body1" className="text-white font-semibold">
            {room.encounter.description}
          </Typography>
        </Box>
      )}

      <Typography variant="h6" className="mb-3 text-white">
        Para onde você quer ir?
      </Typography>

      <Box className="grid grid-cols-2 gap-3">
        {Object.keys(room.connections).map((direction) => (
          <Button
            key={direction}
            variant="contained"
            onClick={() => onMove(direction as any)}
            startIcon={getDirectionIcon(direction)}
            sx={{
              background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
              color: 'white',
              py: 1.5,
              '&:hover': {
                transform: 'translateY(-2px)',
                boxShadow: '0 4px 12px rgba(0,0,0,0.3)',
              },
            }}
          >
            {getDirectionLabel(direction)}
          </Button>
        ))}
      </Box>
    </Paper>
  )
}
