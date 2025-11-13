import { Attack } from '@/types/game'

export const attacks: Attack[] = [
  // ===== ATAQUES FÍSICOS =====
  {
    id: 'slash',
    name: 'Golpe Cortante',
    description: 'Um ataque corpo a corpo básico com espada',
    type: 'physical',
    damage: 25,
    manaCost: 0,
    cooldown: 0,
    accuracy: 95,
  },
  {
    id: 'heavy_strike',
    name: 'Golpe Pesado',
    description: 'Um ataque devastador que causa dano massivo',
    type: 'physical',
    damage: 45,
    manaCost: 15,
    cooldown: 2,
    accuracy: 85,
  },
  {
    id: 'backstab',
    name: 'Punhalada nas Costas',
    description: 'Ataque furtivo com alta chance de crítico',
    type: 'physical',
    damage: 40,
    manaCost: 20,
    cooldown: 3,
    accuracy: 90,
  },

  // ===== ATAQUES MÁGICOS =====
  {
    id: 'fireball',
    name: 'Bola de Fogo',
    description: 'Lança uma bola de fogo explosiva no inimigo',
    type: 'magical',
    damage: 50,
    manaCost: 30,
    cooldown: 2,
    accuracy: 90,
  },
  {
    id: 'ice_shard',
    name: 'Fragmento de Gelo',
    description: 'Dispara um projétil de gelo congelante',
    type: 'magical',
    damage: 35,
    manaCost: 20,
    cooldown: 1,
    accuracy: 95,
    statusEffect: {
      id: 'frozen',
      name: 'Congelado',
      description: 'Velocidade reduzida',
      duration: 2,
      type: 'debuff',
      stat: 'speed',
      value: -5,
    },
  },
  {
    id: 'lightning_bolt',
    name: 'Raio',
    description: 'Invoca um raio poderoso do céu',
    type: 'magical',
    damage: 55,
    manaCost: 35,
    cooldown: 3,
    accuracy: 85,
  },

  // ===== CURA =====
  {
    id: 'heal',
    name: 'Cura',
    description: 'Restaura HP do usuário',
    type: 'healing',
    damage: -30, // negativo = cura
    manaCost: 25,
    cooldown: 1,
    accuracy: 100,
  },
  {
    id: 'greater_heal',
    name: 'Cura Superior',
    description: 'Restaura uma grande quantidade de HP',
    type: 'healing',
    damage: -60,
    manaCost: 45,
    cooldown: 3,
    accuracy: 100,
  },

  // ===== BUFFS =====
  {
    id: 'power_up',
    name: 'Aumentar Poder',
    description: 'Aumenta o ataque temporariamente',
    type: 'buff',
    damage: 0,
    manaCost: 20,
    cooldown: 4,
    accuracy: 100,
    statusEffect: {
      id: 'power_up',
      name: 'Poder Aumentado',
      description: 'Ataque aumentado',
      duration: 3,
      type: 'buff',
      stat: 'attack',
      value: 15,
    },
  },
  {
    id: 'shield',
    name: 'Escudo',
    description: 'Aumenta a defesa temporariamente',
    type: 'buff',
    damage: 0,
    manaCost: 15,
    cooldown: 3,
    accuracy: 100,
    statusEffect: {
      id: 'shield',
      name: 'Escudo Mágico',
      description: 'Defesa aumentada',
      duration: 3,
      type: 'buff',
      stat: 'defense',
      value: 20,
    },
  },

  // ===== DEBUFFS =====
  {
    id: 'weaken',
    name: 'Enfraquecer',
    description: 'Reduz o ataque do inimigo',
    type: 'debuff',
    damage: 10,
    manaCost: 15,
    cooldown: 2,
    accuracy: 90,
    statusEffect: {
      id: 'weakened',
      name: 'Enfraquecido',
      description: 'Ataque reduzido',
      duration: 3,
      type: 'debuff',
      stat: 'attack',
      value: -10,
    },
  },
  {
    id: 'poison',
    name: 'Envenenar',
    description: 'Envenena o inimigo causando dano contínuo',
    type: 'debuff',
    damage: 15,
    manaCost: 20,
    cooldown: 3,
    accuracy: 85,
    statusEffect: {
      id: 'poisoned',
      name: 'Envenenado',
      description: 'Perde HP a cada turno',
      duration: 4,
      type: 'debuff',
      stat: 'hp',
      value: -5,
    },
  },

  // ===== ATAQUES DE MONSTROS =====
  {
    id: 'bite',
    name: 'Mordida',
    description: 'Morde o alvo com presas afiadas',
    type: 'physical',
    damage: 20,
    manaCost: 0,
    cooldown: 0,
    accuracy: 90,
  },
  {
    id: 'claw',
    name: 'Garras',
    description: 'Ataca com garras afiadas',
    type: 'physical',
    damage: 18,
    manaCost: 0,
    cooldown: 0,
    accuracy: 95,
  },
  {
    id: 'dark_magic',
    name: 'Magia Sombria',
    description: 'Lança magia das trevas',
    type: 'magical',
    damage: 35,
    manaCost: 25,
    cooldown: 2,
    accuracy: 85,
  },
]

export function getAttackById(id: string): Attack | undefined {
  return attacks.find((a) => a.id === id)
}

export function getAttacksByIds(ids: string[]): Attack[] {
  return ids.map((id) => getAttackById(id)).filter((a) => a !== undefined) as Attack[]
}
