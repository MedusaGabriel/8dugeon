'use client'
import { useEffect, useState } from 'react'
import dynamic from 'next/dynamic'
import {
  Container,
  Paper,
  Typography,
  Box,
  Grid,
  Dialog,
  DialogTitle,
  DialogContent,
  TextField,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from '@mui/material'
import { useGameStore } from '@/store/gameStore'
import { useRouter } from 'next/navigation'
import Button from '@/components/ui/Button'
import { PlayerClass, Attack } from '@/types/game'
import { playerAttack, enemyTurn, attemptFlee } from '@/lib/game/battleEngine'
import Link from 'next/link'
import { classes } from '@/data/classes'

// Lazy load dos componentes pesados do jogo
const DungeonView = dynamic(() => import('@/components/game/DungeonView'), {
  loading: () => (
    <Box className="h-64 flex items-center justify-center text-white">Carregando masmorra...</Box>
  ),
})
const StatusBar = dynamic(() => import('@/components/game/StatusBar'), {
  loading: () => (
    <Box className="h-32 flex items-center justify-center text-white">Carregando status...</Box>
  ),
})
const BattleLog = dynamic(() => import('@/components/game/BattleLog'), {
  loading: () => (
    <Box className="h-64 flex items-center justify-center text-white">Carregando log...</Box>
  ),
})
const ActionButtons = dynamic(() => import('@/components/game/ActionButtons'), {
  loading: () => (
    <Box className="h-32 flex items-center justify-center text-white">Carregando ações...</Box>
  ),
})

export default function DungeonPage() {
  const router = useRouter()
  const { player, currentRoom, battle, gameStarted, startGame, moveToRoom, setBattle, endBattle, resetGame } =
    useGameStore()

  const [showCharacterCreation, setShowCharacterCreation] = useState(false)
  const [playerName, setPlayerName] = useState('')
  const [selectedClass, setSelectedClass] = useState<PlayerClass>('warrior')

  useEffect(() => {
    if (!gameStarted) {
      setShowCharacterCreation(true)
    }
  }, [gameStarted])

  // Executa turno do inimigo automaticamente
  useEffect(() => {
    if (battle && battle.isActive && battle.turn === 'enemy') {
      const timer = setTimeout(() => {
        const newBattle = enemyTurn(battle)
        setBattle(newBattle)

        if (!newBattle.isActive) {
          setTimeout(() => endBattle(), 2000)
        }
      }, 1500)

      return () => clearTimeout(timer)
    }
  }, [battle, setBattle, endBattle])

  const handleStartGame = () => {
    if (playerName.trim()) {
      startGame(playerName, selectedClass)
      setShowCharacterCreation(false)
    }
  }

  const handleMove = (direction: 'left' | 'right' | 'forward' | 'back') => {
    moveToRoom(direction)
  }

  const handleAttack = (attack: Attack) => {
    if (!battle || !player || battle.turn !== 'player') return

    const newBattle = playerAttack(battle, attack)
    setBattle(newBattle)

    if (!newBattle.isActive) {
      setTimeout(() => endBattle(), 2000)
    }
  }

  const handleFlee = () => {
    if (!battle || battle.turn !== 'player') return
    const newBattle = attemptFlee(battle)
    setBattle(newBattle)

    if (!newBattle.isActive) {
      setTimeout(() => endBattle(), 1500)
    }
  }

  if (!gameStarted || !player || !currentRoom) {
    return (
      <Dialog open={showCharacterCreation} maxWidth="sm" fullWidth>
        <DialogTitle>
          <Typography variant="h5" className="font-bold">
            🎮 Criar Personagem
          </Typography>
        </DialogTitle>
        <DialogContent>
          <Box className="space-y-4 pt-2">
            <TextField
              fullWidth
              label="Nome do Personagem"
              value={playerName}
              onChange={(e) => setPlayerName(e.target.value)}
              variant="outlined"
            />

            <FormControl fullWidth>
              <InputLabel>Classe</InputLabel>
              <Select
                value={selectedClass}
                label="Classe"
                onChange={(e) => setSelectedClass(e.target.value as PlayerClass)}
              >
                {classes.map((classData) => (
                  <MenuItem key={classData.id} value={classData.id}>
                    {classData.icon} {classData.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <Button
              fullWidth
              variant="contained"
              onClick={handleStartGame}
              disabled={!playerName.trim()}
              sx={{
                background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                color: 'white',
                py: 1.5,
              }}
            >
              Começar Aventura
            </Button>

            <Link href="/classes" passHref legacyBehavior>
              <Button
                fullWidth
                variant="outlined"
                sx={{
                  borderColor: 'rgba(102,126,234,0.5)',
                  color: '#667eea',
                }}
              >
                Ver Todas as Classes
              </Button>
            </Link>
          </Box>
        </DialogContent>
      </Dialog>
    )
  }

  return (
    <main className="min-h-screen py-4 sm:py-8">
      <Container maxWidth="xl">
        <Box className="mb-4 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-3">
          <Typography variant="h4" className="text-white font-bold text-2xl sm:text-3xl">
            🏰 A Masmorra
          </Typography>
          <Box className="flex flex-wrap gap-2">
            <Button
              variant="contained"
              onClick={() => {
                if (confirm('Deseja resetar o jogo e voltar ao início?')) {
                  resetGame()
                  setShowCharacterCreation(true)
                }
              }}
              sx={{ 
                background: 'rgba(239, 68, 68, 0.8)',
                color: 'white',
                fontSize: { xs: '0.75rem', sm: '0.875rem' },
                padding: { xs: '6px 12px', sm: '8px 16px' }
              }}
            >
              🔄 Resetar
            </Button>
            <Link href="/status" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{ 
                  borderColor: 'rgba(255,255,255,0.3)', 
                  color: 'white',
                  fontSize: { xs: '0.75rem', sm: '0.875rem' },
                  padding: { xs: '6px 12px', sm: '8px 16px' }
                }}
              >
                📊 Status
              </Button>
            </Link>
            <Link href="/creatures" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{ 
                  borderColor: 'rgba(255,255,255,0.3)', 
                  color: 'white',
                  fontSize: { xs: '0.75rem', sm: '0.875rem' },
                  padding: { xs: '6px 12px', sm: '8px 16px' }
                }}
              >
                👾 Bestiário
              </Button>
            </Link>
            <Link href="/classes" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{ 
                  borderColor: 'rgba(255,255,255,0.3)', 
                  color: 'white',
                  fontSize: { xs: '0.75rem', sm: '0.875rem' },
                  padding: { xs: '6px 12px', sm: '8px 16px' }
                }}
              >
                🎭 Classes
              </Button>
            </Link>
          </Box>
        </Box>

        {battle && battle.isActive ? (
          // MODO BATALHA
          <Grid container spacing={{ xs: 2, sm: 3 }}>
            <Grid item xs={12} md={6}>
              <StatusBar entity={player} label="Você" />
            </Grid>
            <Grid item xs={12} md={6}>
              <StatusBar entity={battle.enemy} label="Inimigo" />
            </Grid>
            <Grid item xs={12} md={6}>
              <BattleLog logs={battle.battleLog} />
            </Grid>
            <Grid item xs={12} md={6}>
              <Paper className="p-3 sm:p-4 bg-white/5 backdrop-blur-sm border border-white/10">
                <Typography variant="h6" className="mb-3 text-white font-bold text-base sm:text-lg">
                  Suas Ações
                </Typography>
                <ActionButtons
                  attacks={player.attacks}
                  currentMana={player.currentStats.mana}
                  onAttack={handleAttack}
                  onFlee={handleFlee}
                  disabled={battle.turn !== 'player'}
                />
                {battle.turn === 'enemy' && (
                  <Typography variant="body2" className="text-yellow-400 mt-3 text-center text-sm">
                    Turno do inimigo...
                  </Typography>
                )}
              </Paper>
            </Grid>
          </Grid>
        ) : (
          // MODO EXPLORAÇÃO
          <Grid container spacing={{ xs: 2, sm: 3 }}>
            <Grid item xs={12} md={8}>
              <DungeonView room={currentRoom} onMove={handleMove} />
            </Grid>
            <Grid item xs={12} md={4}>
              <StatusBar entity={player} label="Seu Personagem" />
            </Grid>
          </Grid>
        )}

        {battle && !battle.isActive && battle.result && (
          <Paper className="p-4 sm:p-6 mt-3 sm:mt-4 bg-white/5 backdrop-blur-sm border border-white/10 text-center">
            {battle.result === 'victory' && (
              <>
                <Typography variant="h4" className="text-green-400 font-bold mb-2 text-2xl sm:text-3xl">
                  🎉 Vitória!
                </Typography>
                <Typography variant="body1" className="text-white/80 text-sm sm:text-base">
                  Você derrotou {battle.enemy.name}!
                </Typography>
              </>
            )}
            {battle.result === 'defeat' && (
              <>
                <Typography variant="h4" className="text-red-400 font-bold mb-2 text-2xl sm:text-3xl">
                  💀 Derrota...
                </Typography>
                <Typography variant="body1" className="text-white/80 text-sm sm:text-base">
                  Você foi derrotado por {battle.enemy.name}
                </Typography>
              </>
            )}
            {battle.result === 'fled' && (
              <>
                <Typography variant="h4" className="text-yellow-400 font-bold mb-2 text-2xl sm:text-3xl">
                  🏃 Fuga
                </Typography>
                <Typography variant="body1" className="text-white/80 text-sm sm:text-base">
                  Você fugiu da batalha!
                </Typography>
              </>
            )}
          </Paper>
        )}
      </Container>
    </main>
  )
}
