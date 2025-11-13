'use client'
import { Container, Paper, Typography, Grid, Box, Chip } from '@mui/material'
import Link from 'next/link'
import { creatures } from '@/data/creatures'
import { Card, CardContent, CardActions } from '@mui/material'
import Button from '@/components/ui/Button'

export default function CreaturesPage() {
  const getTypeColor = (type: string) => {
    switch (type) {
      case 'beast':
        return '#10b981'
      case 'undead':
        return '#8b5cf6'
      case 'demon':
        return '#ef4444'
      case 'dragon':
        return '#f59e0b'
      case 'humanoid':
        return '#3b82f6'
      default:
        return '#6b7280'
    }
  }

  return (
    <main className="min-h-screen py-12">
      <Container maxWidth="lg">
        <Paper elevation={0} className="p-8 bg-white/5 backdrop-blur-sm border border-white/10">
          <Typography variant="h3" className="mb-2 text-white font-bold">
            👾 Bestiário
          </Typography>
          <Typography variant="body1" className="mb-6 text-white/70">
            Todas as criaturas que habitam a masmorra
          </Typography>

          <Grid container spacing={3}>
            {creatures.map((creature) => (
              <Grid item xs={12} sm={6} md={4} key={creature.id}>
                <Card
                  sx={{
                    background: 'rgba(255,255,255,0.05)',
                    backdropFilter: 'blur(10px)',
                    border: '1px solid rgba(255,255,255,0.1)',
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                  }}
                >
                  <CardContent sx={{ flexGrow: 1 }}>
                    <Box className="flex items-center justify-between mb-2">
                      <Typography variant="h6" className="text-white font-bold">
                        {creature.name}
                      </Typography>
                      <Chip
                        label={`Nv. ${creature.level}`}
                        size="small"
                        sx={{
                          backgroundColor: 'rgba(102,126,234,0.3)',
                          color: 'white',
                        }}
                      />
                    </Box>

                    <Chip
                      label={creature.type}
                      size="small"
                      sx={{
                        mb: 2,
                        backgroundColor: getTypeColor(creature.type) + '33',
                        color: getTypeColor(creature.type),
                      }}
                    />

                    <Typography variant="body2" className="text-white/80 mb-3">
                      {creature.description}
                    </Typography>

                    <Box className="space-y-1">
                      <Typography variant="caption" className="text-white/60">
                        ❤️ HP: {creature.stats.maxHp} | ⚔️ ATK: {creature.stats.attack}
                      </Typography>
                      <Typography variant="caption" className="text-white/60 block">
                        🛡️ DEF: {creature.stats.defense} | ⚡ SPD: {creature.stats.speed}
                      </Typography>
                      <Typography variant="caption" className="text-white/60 block">
                        💰 {creature.goldReward} Gold | ⭐ {creature.xpReward} XP
                      </Typography>
                    </Box>
                  </CardContent>

                  <CardActions>
                    <Link href={`/creatures/${creature.id}`} passHref legacyBehavior>
                      <Button
                        size="small"
                        fullWidth
                        variant="outlined"
                        sx={{
                          borderColor: 'rgba(255,255,255,0.3)',
                          color: 'white',
                          '&:hover': {
                            borderColor: 'rgba(255,255,255,0.5)',
                          },
                        }}
                      >
                        Ver Detalhes
                      </Button>
                    </Link>
                  </CardActions>
                </Card>
              </Grid>
            ))}
          </Grid>

          <Box className="mt-6">
            <Link href="/dungeon" passHref legacyBehavior>
              <Button
                variant="contained"
                sx={{
                  background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                  color: 'white',
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
