import { StatusEffect } from '@/types/game'

export const statusEffects: Record<string, StatusEffect> = {
  // BUFFS
  power_up: {
    id: 'power_up',
    name: 'Poder Aumentado',
    description: 'Ataque aumentado temporariamente',
    duration: 3,
    type: 'buff',
    stat: 'attack',
    value: 15,
  },
  shield: {
    id: 'shield',
    name: 'Escudo Mágico',
    description: 'Defesa aumentada temporariamente',
    duration: 3,
    type: 'buff',
    stat: 'defense',
    value: 20,
  },
  haste: {
    id: 'haste',
    name: 'Pressa',
    description: 'Velocidade aumentada',
    duration: 3,
    type: 'buff',
    stat: 'speed',
    value: 10,
  },

  // DEBUFFS
  weakened: {
    id: 'weakened',
    name: 'Enfraquecido',
    description: 'Ataque reduzido',
    duration: 3,
    type: 'debuff',
    stat: 'attack',
    value: -10,
  },
  poisoned: {
    id: 'poisoned',
    name: 'Envenenado',
    description: 'Perde HP a cada turno',
    duration: 4,
    type: 'debuff',
    stat: 'hp',
    value: -5,
  },
  frozen: {
    id: 'frozen',
    name: 'Congelado',
    description: 'Velocidade drasticamente reduzida',
    duration: 2,
    type: 'debuff',
    stat: 'speed',
    value: -5,
  },
  burning: {
    id: 'burning',
    name: 'Queimando',
    description: 'Perde HP devido às chamas',
    duration: 3,
    type: 'debuff',
    stat: 'hp',
    value: -8,
  },
}

export function getStatusEffectById(id: string): StatusEffect | undefined {
  return statusEffects[id]
}
