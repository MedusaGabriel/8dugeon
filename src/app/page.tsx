'use client'
import { Container, Paper, Typography, Button, Box } from '@mui/material'
import { SportsEsports } from '@mui/icons-material'

export default function Home() {
  return (
    <main className="min-h-screen flex items-center justify-center">
      <Container maxWidth="sm">
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
          <Box className="mb-4">
            <SportsEsports sx={{ fontSize: 60, color: 'white' }} />
          </Box>
          
          <Typography 
            variant="h2" 
            component="h1" 
            className="mb-4 font-bold text-white"
            sx={{ textShadow: '2px 2px 4px rgba(0,0,0,0.3)' }}
          >
            8dugeon
          </Typography>
          
          <Typography 
            variant="h6" 
            className="mb-6 text-white/90"
          >
            Bem-vindo ao seu projeto Next.js com TypeScript!
          </Typography>
          
          <Button
            variant="contained"
            href="https://vercel.com"
            target="_blank"
            rel="noopener noreferrer"
            className="bg-gradient-badge"
            sx={{
              background: 'linear-gradient(135deg, #667eea 0%, #4299e1 100%)',
              color: 'white',
              fontWeight: 'bold',
              px: 3,
              py: 1.5,
              borderRadius: '25px',
              textTransform: 'none',
              fontSize: '1rem',
              transition: 'all 0.3s ease',
              '&:hover': {
                transform: 'translateY(-2px)',
                boxShadow: '0 4px 12px rgba(0,0,0,0.2)',
              },
            }}
          >
            Powered by Vercel
          </Button>
          
          <Box className="flex justify-center gap-2 mt-8">
            {[0, 1, 2].map((i) => (
              <Box
                key={i}
                className="w-3 h-3 rounded-full bg-white"
                sx={{
                  animation: 'pulse 2s infinite',
                  animationDelay: `${i * 0.3}s`,
                  '@keyframes pulse': {
                    '0%, 100%': {
                      opacity: 0.3,
                      transform: 'scale(0.8)',
                    },
                    '50%': {
                      opacity: 1,
                      transform: 'scale(1.2)',
                    },
                  },
                }}
              />
            ))}
          </Box>
        </Paper>
      </Container>
    </main>
  )
}
