'use client'
import { Box, Grid, Tooltip } from '@mui/material'
import Button from '@/components/ui/Button'
import { Attack } from '@/types/game'
import SportsKabaddiIcon from '@mui/icons-material/SportsKabaddi'
import AutoFixHighIcon from '@mui/icons-material/AutoFixHigh'
import FavoriteIcon from '@mui/icons-material/Favorite'
import ShieldIcon from '@mui/icons-material/Shield'

interface ActionButtonsProps {
  attacks: Attack[]
  currentMana: number
  onAttack: (attack: Attack) => void
  onFlee: () => void
  disabled?: boolean
}

export default function ActionButtons({
  attacks,
  currentMana,
  onAttack,
  onFlee,
  disabled = false,
}: ActionButtonsProps) {
  const getAttackIcon = (type: Attack['type']) => {
    switch (type) {
      case 'physical':
        return <SportsKabaddiIcon />
      case 'magical':
        return <AutoFixHighIcon />
      case 'healing':
        return <FavoriteIcon />
      case 'buff':
      case 'debuff':
        return <ShieldIcon />
    }
  }

  const getAttackColor = (type: Attack['type']) => {
    switch (type) {
      case 'physical':
        return 'linear-gradient(135deg, #ef4444 0%, #dc2626 100%)'
      case 'magical':
        return 'linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)'
      case 'healing':
        return 'linear-gradient(135deg, #10b981 0%, #059669 100%)'
      case 'buff':
        return 'linear-gradient(135deg, #f59e0b 0%, #d97706 100%)'
      case 'debuff':
        return 'linear-gradient(135deg, #6b7280 0%, #4b5563 100%)'
    }
  }

  return (
    <Box>
      <Grid container spacing={2}>
        {attacks.map((attack) => {
          const canUse = currentMana >= attack.manaCost
          return (
            <Grid item xs={12} sm={6} key={attack.id}>
              <Tooltip
                title={
                  canUse
                    ? `${attack.description} | Custo: ${attack.manaCost} Mana | Dano: ${attack.damage}`
                    : 'Mana insuficiente'
                }
                arrow
              >
                <span>
                  <Button
                    variant="contained"
                    fullWidth
                    onClick={() => onAttack(attack)}
                    disabled={disabled || !canUse}
                    startIcon={getAttackIcon(attack.type)}
                    sx={{
                      background: canUse ? getAttackColor(attack.type) : undefined,
                      color: 'white',
                      py: 1.5,
                      '&:hover': {
                        transform: 'translateY(-2px)',
                        boxShadow: '0 4px 12px rgba(0,0,0,0.3)',
                      },
                      '&:disabled': {
                        opacity: 0.5,
                      },
                    }}
                  >
                    {attack.name}
                    {attack.manaCost > 0 && (
                      <Box component="span" className="ml-2 text-xs opacity-75">
                        ({attack.manaCost} MP)
                      </Box>
                    )}
                  </Button>
                </span>
              </Tooltip>
            </Grid>
          )
        })}
      </Grid>

      <Button
        variant="outlined"
        fullWidth
        onClick={onFlee}
        disabled={disabled}
        sx={{
          mt: 2,
          borderColor: 'rgba(255,255,255,0.3)',
          color: 'white',
          '&:hover': {
            borderColor: 'rgba(255,255,255,0.5)',
            backgroundColor: 'rgba(255,255,255,0.05)',
          },
        }}
      >
        🏃 Fugir
      </Button>
    </Box>
  )
}
