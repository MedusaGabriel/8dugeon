'use client'
import { Container, Paper, Typography, Box, Grid } from '@mui/material'
import dynamic from 'next/dynamic'
import Link from 'next/link'
import Button from '@/components/ui/Button'

// Lazy load dos ícones
const Castle = dynamic(() => import('@mui/icons-material/Castle'))
const SportsEsports = dynamic(() => import('@mui/icons-material/SportsEsports'))
const Book = dynamic(() => import('@mui/icons-material/Book'))
const Group = dynamic(() => import('@mui/icons-material/Group'))

export default function Home() {
  return (
    <main className="min-h-screen flex items-center justify-center p-4">
      <Container maxWidth="md">
        <Paper
          elevation={0}
          className="p-8 text-center backdrop-blur-lg bg-white/10 border border-white/20"
          sx={{
            animation: 'fadeIn 0.6s ease-in',
            '@keyframes fadeIn': {
              from: { opacity: 0, transform: 'translateY(20px)' },
              to: { opacity: 1, transform: 'translateY(0)' },
            },
          }}
        >
          <Box className="mb-6">
            <Typography
              variant="h1"
              component="h1"
              className="mb-2 font-bold text-white"
              sx={{
                textShadow: '3px 3px 6px rgba(0,0,0,0.5)',
                fontSize: { xs: '3rem', md: '4rem' },
              }}
            >
              🎮 8dugeon
            </Typography>
            <Typography variant="h5" className="text-white/80 mb-6">
              RPG por Turno
            </Typography>
          </Box>

          <Typography variant="body1" className="mb-8 text-white/90 max-w-lg mx-auto">
            Explore masmorras perigosas, enfrente criaturas mortais e torne-se uma lenda! Escolha
            sua classe, aprimore suas habilidades e conquiste tesouros épicos.
          </Typography>

          <Grid container spacing={2} className="mb-6">
            <Grid item xs={12} sm={6}>
              <Link href="/dungeon" passHref legacyBehavior>
                <Button
                  variant="contained"
                  fullWidth
                  startIcon={<Castle />}
                  sx={{
                    background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
                    color: 'white',
                    fontWeight: 'bold',
                    py: 2,
                    fontSize: '1.1rem',
                    '&:hover': {
                      transform: 'translateY(-2px)',
                      boxShadow: '0 6px 20px rgba(102,126,234,0.4)',
                    },
                  }}
                >
                  Entrar na Masmorra
                </Button>
              </Link>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Link href="/classes" passHref legacyBehavior>
                <Button
                  variant="outlined"
                  fullWidth
                  startIcon={<Group />}
                  sx={{
                    borderColor: 'rgba(255,255,255,0.5)',
                    color: 'white',
                    fontWeight: 'bold',
                    py: 2,
                    fontSize: '1.1rem',
                    '&:hover': {
                      borderColor: 'rgba(255,255,255,0.8)',
                      backgroundColor: 'rgba(255,255,255,0.1)',
                    },
                  }}
                >
                  Ver Classes
                </Button>
              </Link>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Link href="/creatures" passHref legacyBehavior>
                <Button
                  variant="outlined"
                  fullWidth
                  startIcon={<SportsEsports />}
                  sx={{
                    borderColor: 'rgba(255,255,255,0.5)',
                    color: 'white',
                    fontWeight: 'bold',
                    py: 2,
                    fontSize: '1.1rem',
                    '&:hover': {
                      borderColor: 'rgba(255,255,255,0.8)',
                      backgroundColor: 'rgba(255,255,255,0.1)',
                    },
                  }}
                >
                  Bestiário
                </Button>
              </Link>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Button
                variant="outlined"
                fullWidth
                startIcon={<Book />}
                sx={{
                  borderColor: 'rgba(255,255,255,0.5)',
                  color: 'white',
                  fontWeight: 'bold',
                  py: 2,
                  fontSize: '1.1rem',
                  '&:hover': {
                    borderColor: 'rgba(255,255,255,0.8)',
                    backgroundColor: 'rgba(255,255,255,0.1)',
                  },
                }}
              >
                Como Jogar
              </Button>
            </Grid>
          </Grid>

          <Box className="pt-6 border-t border-white/10">
            <Typography variant="caption" className="text-white/50">
              Desenvolvido com Next.js, TypeScript, Tailwind CSS & Material-UI
            </Typography>
          </Box>
        </Paper>
      </Container>
    </main>
  )
}
