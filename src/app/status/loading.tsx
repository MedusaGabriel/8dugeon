import { Box, CircularProgress, Typography } from '@mui/material'

export default function Loading() {
  return (
    <Box className="min-h-screen flex flex-col items-center justify-center">
      <CircularProgress
        size={60}
        sx={{
          color: '#667eea',
          mb: 3,
        }}
      />
      <Typography variant="h6" className="text-white/80">
        Carregando status...
      </Typography>
    </Box>
  )
}
