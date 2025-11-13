'use client'
import { Container, Paper, Typography, Box, Divider, Grid } from '@mui/material'
import { useParams } from 'next/navigation'
import Link from 'next/link'
import { getClassBySlug } from '@/data/classes'
import { PlayerClass } from '@/types/game'
import Button from '@/components/ui/Button'

export default function ClassDetailPage() {
  const params = useParams()
  const classData = getClassBySlug(params.slug as PlayerClass)

  if (!classData) {
    return (
      <main className="min-h-screen flex items-center justify-center">
        <Container maxWidth="md">
          <Paper className="p-8 bg-white/5 backdrop-blur-sm border border-white/10 text-center">
            <Typography variant="h4" className="text-white mb-4">
              Classe não encontrada
            </Typography>
            <Link href="/classes" passHref legacyBehavior>
              <Button
                variant="contained"
                sx={{
                  background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                  color: 'white',
                }}
              >
                Voltar às Classes
              </Button>
            </Link>
          </Paper>
        </Container>
      </main>
    )
  }

  return (
    <main className="min-h-screen py-12">
      <Container maxWidth="md">
        <Paper elevation={0} className="p-8 bg-white/5 backdrop-blur-sm border border-white/10">
          <Box className="text-center mb-6">
            <Typography variant="h1" className="mb-2">
              {classData.icon}
            </Typography>
            <Typography variant="h3" className="text-white font-bold">
              {classData.name}
            </Typography>
          </Box>

          <Typography variant="body1" className="text-white/90 mb-6 text-center">
            {classData.description}
          </Typography>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          <Typography variant="h5" className="text-white font-bold mb-3">
            📊 Status Base
          </Typography>

          <Grid container spacing={2}>
            <Grid item xs={6}>
              <Box className="p-3 bg-white/5 rounded-lg border border-white/10 text-center">
                <Typography variant="h4" className="text-red-400">
                  ❤️
                </Typography>
                <Typography variant="h6" className="text-white font-bold">
                  {classData.baseStats.maxHp}
                </Typography>
                <Typography variant="caption" className="text-white/60">
                  HP
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={6}>
              <Box className="p-3 bg-white/5 rounded-lg border border-white/10 text-center">
                <Typography variant="h4" className="text-blue-400">
                  💙
                </Typography>
                <Typography variant="h6" className="text-white font-bold">
                  {classData.baseStats.maxMana}
                </Typography>
                <Typography variant="caption" className="text-white/60">
                  Mana
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={4}>
              <Box className="p-3 bg-white/5 rounded-lg border border-white/10 text-center">
                <Typography variant="h4">⚔️</Typography>
                <Typography variant="h6" className="text-white font-bold">
                  {classData.baseStats.attack}
                </Typography>
                <Typography variant="caption" className="text-white/60">
                  Ataque
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={4}>
              <Box className="p-3 bg-white/5 rounded-lg border border-white/10 text-center">
                <Typography variant="h4">🛡️</Typography>
                <Typography variant="h6" className="text-white font-bold">
                  {classData.baseStats.defense}
                </Typography>
                <Typography variant="caption" className="text-white/60">
                  Defesa
                </Typography>
              </Box>
            </Grid>
            <Grid item xs={4}>
              <Box className="p-3 bg-white/5 rounded-lg border border-white/10 text-center">
                <Typography variant="h4">⚡</Typography>
                <Typography variant="h6" className="text-white font-bold">
                  {classData.baseStats.speed}
                </Typography>
                <Typography variant="caption" className="text-white/60">
                  Velocidade
                </Typography>
              </Box>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          <Typography variant="h5" className="text-white font-bold mb-3">
            ✨ Habilidade Passiva
          </Typography>

          <Box className="p-4 bg-gradient-to-r from-purple-500/20 to-blue-500/20 rounded-lg border border-white/20">
            <Typography variant="h6" className="text-white font-bold mb-2">
              {classData.passive.name}
            </Typography>
            <Typography variant="body2" className="text-white/80">
              {classData.passive.description}
            </Typography>
          </Box>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          <Typography variant="h5" className="text-white font-bold mb-3">
            ⚔️ Ataques Iniciais
          </Typography>

          <Box className="space-y-2">
            {classData.attacks.map((attack, index) => (
              <Box key={index} className="p-3 bg-white/5 rounded-lg border border-white/10">
                <Typography variant="subtitle1" className="text-white font-semibold">
                  {attack.name}
                </Typography>
                <Typography variant="body2" className="text-white/70">
                  💥 Dano: {attack.damage} | 🎯 Precisão: {attack.accuracy}%
                  {attack.manaCost > 0 && ` | 💙 Custo: ${attack.manaCost} Mana`}
                </Typography>
              </Box>
            ))}
          </Box>

          <Box className="mt-6 flex gap-3">
            <Link href="/classes" passHref legacyBehavior>
              <Button
                variant="outlined"
                sx={{
                  borderColor: 'rgba(255,255,255,0.3)',
                  color: 'white',
                }}
              >
                ← Voltar
              </Button>
            </Link>
            <Link href="/dungeon" passHref legacyBehavior>
              <Button
                variant="contained"
                fullWidth
                sx={{
                  background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                  color: 'white',
                }}
              >
                Começar Aventura
              </Button>
            </Link>
          </Box>
        </Paper>
      </Container>
    </main>
  )
}
