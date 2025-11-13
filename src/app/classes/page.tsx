'use client'
import { Container, Paper, Typography, Grid, Box, Chip } from '@mui/material'
import Link from 'next/link'
import { classes } from '@/data/classes'
import { Card, CardContent, CardActions } from '@mui/material'
import Button from '@/components/ui/Button'

export default function ClassesPage() {
  return (
    <main className="min-h-screen py-12">
      <Container maxWidth="lg">
        <Paper elevation={0} className="p-8 bg-white/5 backdrop-blur-sm border border-white/10">
          <Typography variant="h3" className="mb-2 text-white font-bold">
            🎭 Classes
          </Typography>
          <Typography variant="body1" className="mb-6 text-white/70">
            Escolha sua classe e embarque na aventura
          </Typography>

          <Grid container spacing={3}>
            {classes.map((classData) => (
              <Grid item xs={12} sm={6} md={4} key={classData.id}>
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
                    <Box className="text-center mb-3">
                      <Typography variant="h2" className="mb-2">
                        {classData.icon}
                      </Typography>
                      <Typography variant="h5" className="text-white font-bold">
                        {classData.name}
                      </Typography>
                    </Box>

                    <Typography variant="body2" className="text-white/80 mb-3">
                      {classData.description}
                    </Typography>

                    <Box className="space-y-1 mb-3">
                      <Typography variant="caption" className="text-white/60 block">
                        ❤️ HP: {classData.baseStats.maxHp} | ⚔️ ATK: {classData.baseStats.attack}
                      </Typography>
                      <Typography variant="caption" className="text-white/60 block">
                        🛡️ DEF: {classData.baseStats.defense} | ⚡ SPD: {classData.baseStats.speed}
                      </Typography>
                      <Typography variant="caption" className="text-white/60 block">
                        💙 Mana: {classData.baseStats.maxMana}
                      </Typography>
                    </Box>

                    <Box className="p-2 bg-white/5 rounded-lg">
                      <Typography variant="caption" className="text-white/90 font-semibold">
                        ✨ {classData.passive.name}
                      </Typography>
                      <Typography variant="caption" className="text-white/60 block">
                        {classData.passive.description}
                      </Typography>
                    </Box>
                  </CardContent>

                  <CardActions>
                    <Link href={`/classes/${classData.slug}`} passHref legacyBehavior>
                      <Button
                        size="small"
                        fullWidth
                        variant="contained"
                        sx={{
                          background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                          color: 'white',
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
                variant="outlined"
                sx={{
                  borderColor: 'rgba(255,255,255,0.3)',
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
