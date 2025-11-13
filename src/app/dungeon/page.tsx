'use client'
import { useEffect, useState } from 'react'
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
import DungeonView from '@/components/game/DungeonView'
import StatusBar from '@/components/game/StatusBar'
import BattleLog from '@/components/game/BattleLog'
import ActionButtons from '@/components/game/ActionButtons'
import Button from '@/components/ui/Button'
import { PlayerClass, Attack } from '@/types/game'
import { playerAttack, enemyTurn, attemptFlee } from '@/lib/game/battleEngine'
import Link from 'next/link'

export default function DungeonPage() {
  const router = useRouter()
  const { player, currentRoom, battle, gameStarted, startGame, moveToRoom, setBattle, endBattle } =
    useGameStore()

  const [showCharacterCreation, setShowCharacterCreation] = useState(false)
  const [playerName, setPlayerName] = useState('')
  const [selectedClass, setSelectedClass] = useState<PlayerClass>('warrior')

  useEffect(() => {
    if (!gameStarted) {
      setShowCharacterCreation(true)
    }
  }, [gameStarted])

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
    if (!battle || !player) return

    let newBattle = playerAttack(battle, attack)
    setBattle(newBattle)

    if (newBattle.isActive && newBattle.turn === 'enemy') {
      setTimeout(() => {
        newBattle = enemyTurn(newBattle)
        setBattle(newBattle)

        if (!newBattle.isActive) {
          setTimeout(() => endBattle(), 2000)
        }
      }, 1000)
    } else if (!newBattle.isActive) {
      setTimeout(() => endBattle(), 2000)
    }
  }

  const handleFlee = () => {
    if (!battle) return
    const newBattle = attemptFlee(battle)
    setBattle(newBattle)

    if (!newBattle.isActive) {
      setTimeout(() => endBattle(), 1500)
    } else if (newBattle.turn === 'enemy') {
      setTimeout(() => {
        const afterFlee = enemyTurn(newBattle)
        setBattle(afterFlee)
      }, 1000)
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
                <MenuItem value="warrior">⚔️ Guerreiro</MenuItem>
                <MenuItem value="mage">🔮 Mago</MenuItem>
                <MenuItem value="rogue">🗡️ Ladino</MenuItem>
                <MenuItem value="cleric">✨ Clérigo</MenuItem>
                <MenuItem value="bard">🎵 Bardo</MenuItem>
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
    <main className="min-h-screen py-8">
      <Container maxWidth="xl">
        <Box className="mb-4 flex justify-between items-center">
          <Typography variant="h4" className="text-white font-bold">
            🏰 A Masmorra
          </Typography>
          <Box className="flex gap-2">
            <Link href="/status" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{ borderColor: 'rgba(255,255,255,0.3)', color: 'white' }}
              >
                📊 Status
              </Button>
            </Link>
            <Link href="/creatures" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{ borderColor: 'rgba(255,255,255,0.3)', color: 'white' }}
              >
                👾 Bestiário
              </Button>
            </Link>
            <Link href="/classes" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{ borderColor: 'rgba(255,255,255,0.3)', color: 'white' }}
              >
                🎭 Classes
              </Button>
            </Link>
          </Box>
        </Box>

        {battle && battle.isActive ? (
          // MODO BATALHA
          <Grid container spacing={3}>
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
              <Paper className="p-4 bg-white/5 backdrop-blur-sm border border-white/10">
                <Typography variant="h6" className="mb-3 text-white font-bold">
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
                  <Typography variant="body2" className="text-yellow-400 mt-3 text-center">
                    Turno do inimigo...
                  </Typography>
                )}
              </Paper>
            </Grid>
          </Grid>
        ) : (
          // MODO EXPLORAÇÃO
          <Grid container spacing={3}>
            <Grid item xs={12} md={8}>
              <DungeonView room={currentRoom} onMove={handleMove} />
            </Grid>
            <Grid item xs={12} md={4}>
              <StatusBar entity={player} label="Seu Personagem" />
            </Grid>
          </Grid>
        )}

        {battle && !battle.isActive && battle.result && (
          <Paper className="p-6 mt-4 bg-white/5 backdrop-blur-sm border border-white/10 text-center">
            {battle.result === 'victory' && (
              <>
                <Typography variant="h4" className="text-green-400 font-bold mb-2">
                  🎉 Vitória!
                </Typography>
                <Typography variant="body1" className="text-white/80">
                  Você derrotou {battle.enemy.name}!
                </Typography>
              </>
            )}
            {battle.result === 'defeat' && (
              <>
                <Typography variant="h4" className="text-red-400 font-bold mb-2">
                  💀 Derrota...
                </Typography>
                <Typography variant="body1" className="text-white/80">
                  Você foi derrotado por {battle.enemy.name}
                </Typography>
              </>
            )}
            {battle.result === 'fled' && (
              <>
                <Typography variant="h4" className="text-yellow-400 font-bold mb-2">
                  🏃 Fuga
                </Typography>
                <Typography variant="body1" className="text-white/80">
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
