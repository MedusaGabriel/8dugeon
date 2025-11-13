'use client'
import { Box, Typography, LinearProgress, Chip } from '@mui/material'
import { Player, Creature } from '@/types/game'
import FavoriteIcon from '@mui/icons-material/Favorite'
import BoltIcon from '@mui/icons-material/Bolt'

interface StatusBarProps {
  entity: Player | Creature
  label: string
}

export default function StatusBar({ entity, label }: StatusBarProps) {
  const isPlayer = 'class' in entity
  const stats = isPlayer ? (entity as Player).currentStats : (entity as Creature).stats
  const maxStats = isPlayer ? (entity as Player).stats : (entity as Creature).stats

  const hpPercent = (stats.hp / maxStats.maxHp) * 100
  const manaPercent = (stats.mana / maxStats.maxMana) * 100

  return (
    <Box className="p-4 bg-white/5 backdrop-blur-sm rounded-lg border border-white/10">
      <Box className="flex items-center justify-between mb-2">
        <Typography variant="h6" className="font-bold text-white">
          {isPlayer ? (entity as Player).name : (entity as Creature).name}
        </Typography>
        {isPlayer && (
          <Chip
            label={`Nv. ${(entity as Player).level}`}
            size="small"
            sx={{
              background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
              color: 'white',
              fontWeight: 'bold',
            }}
          />
        )}
      </Box>

      {/* HP Bar */}
      <Box className="mb-3">
        <Box className="flex items-center gap-2 mb-1">
          <FavoriteIcon sx={{ fontSize: 18, color: '#ef4444' }} />
          <Typography variant="body2" className="text-white/80">
            HP: {stats.hp} / {maxStats.maxHp}
          </Typography>
        </Box>
        <LinearProgress
          variant="determinate"
          value={hpPercent}
          sx={{
            height: 10,
            borderRadius: 5,
            backgroundColor: 'rgba(255,255,255,0.1)',
            '& .MuiLinearProgress-bar': {
              background: 'linear-gradient(90deg, #ef4444 0%, #dc2626 100%)',
              borderRadius: 5,
            },
          }}
        />
      </Box>

      {/* Mana Bar */}
      {maxStats.maxMana > 0 && (
        <Box>
          <Box className="flex items-center gap-2 mb-1">
            <BoltIcon sx={{ fontSize: 18, color: '#3b82f6' }} />
            <Typography variant="body2" className="text-white/80">
              Mana: {stats.mana} / {maxStats.maxMana}
            </Typography>
          </Box>
          <LinearProgress
            variant="determinate"
            value={manaPercent}
            sx={{
              height: 10,
              borderRadius: 5,
              backgroundColor: 'rgba(255,255,255,0.1)',
              '& .MuiLinearProgress-bar': {
                background: 'linear-gradient(90deg, #3b82f6 0%, #2563eb 100%)',
                borderRadius: 5,
              },
            }}
          />
        </Box>
      )}

      {/* Stats */}
      <Box className="flex gap-2 mt-3 flex-wrap">
        <Chip
          label={`⚔️ ${stats.attack}`}
          size="small"
          sx={{ backgroundColor: 'rgba(239,68,68,0.2)', color: 'white' }}
        />
        <Chip
          label={`🛡️ ${stats.defense}`}
          size="small"
          sx={{ backgroundColor: 'rgba(59,130,246,0.2)', color: 'white' }}
        />
        <Chip
          label={`⚡ ${stats.speed}`}
          size="small"
          sx={{ backgroundColor: 'rgba(234,179,8,0.2)', color: 'white' }}
        />
      </Box>
    </Box>
  )
}
