// Pré-carrega apenas os dados essenciais
export const preloadGameData = () => {
  // Força o carregamento prévio apenas quando necessário
  if (typeof window !== 'undefined') {
    // Carrega dados básicos em background
    import('@/data/classes')
    import('@/data/creatures')
  }
}

// Cache de dados do jogo
let cachedCreatures: any = null
let cachedClasses: any = null
let cachedAttacks: any = null

export const getCreatures = () => {
  if (!cachedCreatures) {
    cachedCreatures = require('@/data/creatures').creatures
  }
  return cachedCreatures
}

export const getClasses = () => {
  if (!cachedClasses) {
    cachedClasses = require('@/data/classes').classes
  }
  return cachedClasses
}

export const getAttacks = () => {
  if (!cachedAttacks) {
    cachedAttacks = require('@/data/attacks').attacks
  }
  return cachedAttacks
}
