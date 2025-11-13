'use client'
import {
  Container,
  Paper,
  Typography,
  Box,
  Grid,
  LinearProgress,
  Chip,
  Divider,
} from '@mui/material'
import { useGameStore } from '@/store/gameStore'
import { useRouter } from 'next/navigation'
import Link from 'next/link'
import Button from '@/components/ui/Button'
import { useEffect } from 'react'

export default function StatusPage() {
  const router = useRouter()
  const { player, gameStarted } = useGameStore()

  useEffect(() => {
    if (!gameStarted || !player) {
      router.push('/dungeon')
    }
  }, [gameStarted, player, router])

  if (!player) {
    return null
  }

  const xpPercent = (player.xp / player.xpToNextLevel) * 100

  return (
    <main className="min-h-screen py-12">
      <Container maxWidth="md">
        <Paper elevation={0} className="p-8 bg-white/5 backdrop-blur-sm border border-white/10">
          <Typography variant="h3" className="mb-6 text-white font-bold text-center">
            📊 Status do Personagem
          </Typography>

          {/* Nome e Classe */}
          <Box className="text-center mb-6">
            <Typography variant="h4" className="text-white font-bold mb-2">
              {player.name}
            </Typography>
            <Chip
              label={`${player.class.toUpperCase()} - Nível ${player.level}`}
              sx={{
                background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                color: 'white',
                fontWeight: 'bold',
                fontSize: '1rem',
                px: 2,
              }}
            />
          </Box>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          {/* XP e Gold */}
          <Box className="mb-6">
            <Box className="flex justify-between items-center mb-2">
              <Typography variant="h6" className="text-white font-bold">
                💰 {player.gold} Gold
              </Typography>
              <Typography variant="body2" className="text-white/70">
                ⭐ {player.xp} / {player.xpToNextLevel} XP
              </Typography>
            </Box>
            <LinearProgress
              variant="determinate"
              value={xpPercent}
              sx={{
                height: 12,
                borderRadius: 6,
                backgroundColor: 'rgba(255,255,255,0.1)',
                '& .MuiLinearProgress-bar': {
                  background: 'linear-gradient(90deg, #f59e0b 0%, #d97706 100%)',
                  borderRadius: 6,
                },
              }}
            />
            <Typography variant="caption" className="text-white/60 block text-right mt-1">
              {Math.floor(xpPercent)}% para próximo nível
            </Typography>
          </Box>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          {/* Stats */}
          <Typography variant="h5" className="text-white font-bold mb-3">
            📈 Atributos
          </Typography>

          <Grid container spacing={2}>
            <Grid item xs={6}>
              <Box className="p-4 bg-white/5 rounded-lg border border-white/10">
                <Typography variant="h3" className="text-red-400 text-center mb-2">
                  ❤️
                </Typography>
                <Typography variant="h5" className="text-white font-bold text-center">
                  {player.stats.maxHp}
                </Typography>
                <Typography variant="body2" className="text-white/60 text-center">
                  HP Máximo
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={6}>
              <Box className="p-4 bg-white/5 rounded-lg border border-white/10">
                <Typography variant="h3" className="text-blue-400 text-center mb-2">
                  💙
                </Typography>
                <Typography variant="h5" className="text-white font-bold text-center">
                  {player.stats.maxMana}
                </Typography>
                <Typography variant="body2" className="text-white/60 text-center">
                  Mana Máxima
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={4}>
              <Box className="p-4 bg-white/5 rounded-lg border border-white/10">
                <Typography variant="h3" className="text-center mb-2">
                  ⚔️
                </Typography>
                <Typography variant="h5" className="text-white font-bold text-center">
                  {player.stats.attack}
                </Typography>
                <Typography variant="body2" className="text-white/60 text-center">
                  Ataque
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={4}>
              <Box className="p-4 bg-white/5 rounded-lg border border-white/10">
                <Typography variant="h3" className="text-center mb-2">
                  🛡️
                </Typography>
                <Typography variant="h5" className="text-white font-bold text-center">
                  {player.stats.defense}
                </Typography>
                <Typography variant="body2" className="text-white/60 text-center">
                  Defesa
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={4}>
              <Box className="p-4 bg-white/5 rounded-lg border border-white/10">
                <Typography variant="h3" className="text-center mb-2">
                  ⚡
                </Typography>
                <Typography variant="h5" className="text-white font-bold text-center">
                  {player.stats.speed}
                </Typography>
                <Typography variant="body2" className="text-white/60 text-center">
                  Velocidade
                </Typography>
              </Box>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          {/* Ataques */}
          <Typography variant="h5" className="text-white font-bold mb-3">
            ⚔️ Ataques Conhecidos
          </Typography>

          <Box className="space-y-2">
            {player.attacks.map((attack, index) => (
              <Box key={index} className="p-3 bg-white/5 rounded-lg border border-white/10">
                <Box className="flex justify-between items-start mb-1">
                  <Typography variant="subtitle1" className="text-white font-semibold">
                    {attack.name}
                  </Typography>
                  <Chip
                    label={attack.type}
                    size="small"
                    sx={{
                      backgroundColor: 'rgba(102,126,234,0.3)',
                      color: 'white',
                      fontSize: '0.7rem',
                    }}
                  />
                </Box>
                <Typography variant="body2" className="text-white/70">
                  💥 Dano: {attack.damage} | 🎯 Precisão: {attack.accuracy}%
                  {attack.manaCost > 0 && ` | 💙 Custo: ${attack.manaCost} Mana`}
                </Typography>
              </Box>
            ))}
          </Box>

          {/* Status Effects */}
          {player.statusEffects.length > 0 && (
            <>
              <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />
              <Typography variant="h5" className="text-white font-bold mb-3">
                ✨ Efeitos Ativos
              </Typography>
              <Box className="space-y-2">
                {player.statusEffects.map((effect) => (
                  <Box
                    key={effect.id}
                    className="p-3 bg-purple-500/10 rounded-lg border border-purple-500/30"
                  >
                    <Typography variant="subtitle1" className="text-white font-semibold">
                      {effect.name}
                    </Typography>
                    <Typography variant="body2" className="text-white/70">
                      {effect.description} - {effect.duration} turnos restantes
                    </Typography>
                  </Box>
                ))}
              </Box>
            </>
          )}

          <Box className="mt-6">
            <Link href="/dungeon" passHref legacyBehavior>
              <Button
                variant="contained"
                fullWidth
                sx={{
                  background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                  color: 'white',
                  py: 1.5,
                }}
              >
                ← Voltar para Masmorra
              </Button>
            </Link>
          </Box>
        </Paper>
      </Container>
    </main>
  )
}
