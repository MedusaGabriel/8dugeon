// ============= TIPOS BASE =============
export type PlayerClass = 'warrior' | 'mage' | 'rogue' | 'cleric' | 'bard'

export type AttackType = 'physical' | 'magical' | 'healing' | 'buff' | 'debuff'

export type CreatureType = 'beast' | 'undead' | 'demon' | 'dragon' | 'humanoid'

// ============= STATUS E EFEITOS =============
export interface StatusEffect {
  id: string
  name: string
  description: string
  duration: number
  type: 'buff' | 'debuff'
  stat: 'attack' | 'defense' | 'speed' | 'hp'
  value: number
}

// ============= STATS =============
export interface Stats {
  hp: number
  maxHp: number
  attack: number
  defense: number
  speed: number
  mana: number
  maxMana: number
}

// ============= ATAQUE =============
export interface Attack {
  id: string
  name: string
  description: string
  type: AttackType
  damage: number
  manaCost: number
  cooldown: number
  statusEffect?: StatusEffect
  accuracy: number // 0-100
}

// ============= CLASSE DO JOGADOR =============
export interface ClassData {
  id: string
  name: string
  slug: PlayerClass
  description: string
  icon: string
  baseStats: Stats
  attacks: Attack[]
  passive: {
    name: string
    description: string
    effect: string
  }
}

// ============= CRIATURA/MONSTRO =============
export interface Creature {
  id: string
  name: string
  type: CreatureType
  description: string
  level: number
  stats: Stats
  attacks: Attack[]
  xpReward: number
  goldReward: number
  image?: string
}

// ============= PLAYER =============
export interface Player {
  name: string
  class: PlayerClass
  level: number
  xp: number
  xpToNextLevel: number
  gold: number
  stats: Stats
  currentStats: Stats
  attacks: Attack[]
  statusEffects: StatusEffect[]
  inventory: Item[]
}

// ============= ITEM =============
export interface Item {
  id: string
  name: string
  description: string
  type: 'weapon' | 'armor' | 'potion' | 'accessory'
  statBonus?: Partial<Stats>
  price: number
  rarity: 'common' | 'uncommon' | 'rare' | 'epic' | 'legendary'
}

// ============= DUNGEON/WORLD =============
export interface Room {
  id: string
  description: string
  connections: {
    left?: string
    right?: string
    forward?: string
    back?: string
  }
  encounter?: Encounter
  treasure?: Item[]
  visited: boolean
}

export interface Encounter {
  type: 'battle' | 'treasure' | 'event' | 'boss'
  creature?: Creature
  description: string
}

// ============= BATALHA =============
export interface BattleState {
  player: Player
  enemy: Creature
  turn: 'player' | 'enemy'
  round: number
  battleLog: BattleLogEntry[]
  isActive: boolean
  result?: 'victory' | 'defeat' | 'fled'
}

export interface BattleLogEntry {
  id: string
  timestamp: number
  message: string
  type: 'attack' | 'damage' | 'heal' | 'status' | 'info' | 'critical'
  actor: 'player' | 'enemy'
}

// ============= GAME STATE =============
export interface GameState {
  player: Player | null
  currentRoom: Room | null
  rooms: Map<string, Room>
  battle: BattleState | null
  gameStarted: boolean
  difficulty: 'easy' | 'normal' | 'hard'
}

// ============= ACTIONS =============
export type ActionResult = {
  success: boolean
  message: string
  damage?: number
  healing?: number
  statusEffect?: StatusEffect
  critical?: boolean
}
