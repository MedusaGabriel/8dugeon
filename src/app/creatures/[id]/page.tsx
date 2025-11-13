'use client'
import { Container, Paper, Typography, Box, Chip, Divider } from '@mui/material'
import { useParams } from 'next/navigation'
import Link from 'next/link'
import { getCreatureById } from '@/data/creatures'
import Button from '@/components/ui/Button'
import StatusBar from '@/components/game/StatusBar'

export default function CreatureDetailPage() {
  const params = useParams()
  const creature = getCreatureById(params.id as string)

  if (!creature) {
    return (
      <main className="min-h-screen flex items-center justify-center">
        <Container maxWidth="md">
          <Paper className="p-8 bg-white/5 backdrop-blur-sm border border-white/10 text-center">
            <Typography variant="h4" className="text-white mb-4">
              Criatura não encontrada
            </Typography>
            <Link href="/creatures" passHref legacyBehavior>
              <Button
                variant="contained"
                sx={{
                  background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                  color: 'white',
                }}
              >
                Voltar ao Bestiário
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
          <Box className="flex items-center justify-between mb-4">
            <Typography variant="h3" className="text-white font-bold">
              {creature.name}
            </Typography>
            <Chip
              label={`Nível ${creature.level}`}
              sx={{
                background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                color: 'white',
                fontWeight: 'bold',
              }}
            />
          </Box>

          <Chip
            label={creature.type}
            sx={{
              mb: 3,
              backgroundColor: 'rgba(102,126,234,0.3)',
              color: 'white',
            }}
          />

          <Typography variant="body1" className="text-white/90 mb-6">
            {creature.description}
          </Typography>

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          <Typography variant="h5" className="text-white font-bold mb-3">
            📊 Status
          </Typography>

          <StatusBar entity={creature} label="Criatura" />

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          <Typography variant="h5" className="text-white font-bold mb-3">
            ⚔️ Ataques
          </Typography>

          <Box className="space-y-2">
            {creature.attacks.map((attack, index) => (
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

          <Divider sx={{ my: 3, borderColor: 'rgba(255,255,255,0.1)' }} />

          <Typography variant="h5" className="text-white font-bold mb-3">
            🎁 Recompensas
          </Typography>

          <Box className="flex gap-4">
            <Chip
              label={`💰 ${creature.goldReward} Gold`}
              sx={{
                backgroundColor: 'rgba(234,179,8,0.2)',
                color: '#fbbf24',
                fontWeight: 'bold',
              }}
            />
            <Chip
              label={`⭐ ${creature.xpReward} XP`}
              sx={{
                backgroundColor: 'rgba(139,92,246,0.2)',
                color: '#a78bfa',
                fontWeight: 'bold',
              }}
            />
          </Box>

          <Box className="mt-6 flex gap-3">
            <Link href="/creatures" passHref legacyBehavior>
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
                sx={{
                  background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                  color: 'white',
                }}
              >
                Ir para Masmorra
              </Button>
            </Link>
          </Box>
        </Paper>
      </Container>
    </main>
  )
}
