import { create } from 'zustand'
import { GameState, Player, Room, BattleState, PlayerClass } from '@/types/game'
import { getClassBySlug } from '@/data/classes'
import { createInitialRoom, generateRoom } from '@/lib/game/world'
import { createBattle } from '@/lib/game/battleEngine'

interface GameStore extends GameState {
  // Actions
  startGame: (playerName: string, playerClass: PlayerClass) => void
  moveToRoom: (direction: 'left' | 'right' | 'forward' | 'back') => void
  startBattle: () => void
  setBattle: (battle: BattleState | null) => void
  endBattle: () => void
  resetGame: () => void
}

const initialState: GameState = {
  player: null,
  currentRoom: null,
  rooms: new Map(),
  battle: null,
  gameStarted: false,
  difficulty: 'normal',
}

export const useGameStore = create<GameStore>((set, get) => ({
  ...initialState,

  startGame: (playerName: string, playerClass: PlayerClass) => {
    const classData = getClassBySlug(playerClass)
    if (!classData) return

    const player: Player = {
      name: playerName,
      class: playerClass,
      level: 1,
      xp: 0,
      xpToNextLevel: 100,
      gold: 50,
      stats: { ...classData.baseStats },
      currentStats: { ...classData.baseStats },
      attacks: classData.attacks,
      statusEffects: [],
      inventory: [],
    }

    const entrance = createInitialRoom()
    const rooms = new Map<string, Room>()
    rooms.set(entrance.id, entrance)

    set({
      player,
      currentRoom: entrance,
      rooms,
      gameStarted: true,
      battle: null,
    })
  },

  moveToRoom: (direction) => {
    const { currentRoom, rooms, player } = get()
    if (!currentRoom || !player) return

    const nextRoomId = currentRoom.connections[direction]
    if (!nextRoomId) return

    let nextRoom = rooms.get(nextRoomId)

    // Gera nova sala se não existe
    if (!nextRoom) {
      nextRoom = generateRoom(nextRoomId, player.level)
      rooms.set(nextRoomId, nextRoom)
    }

    // Marca sala atual como visitada
    currentRoom.visited = true

    set({ currentRoom: nextRoom, rooms: new Map(rooms) })

    // Se tem encontro de batalha, inicia automaticamente
    if (nextRoom.encounter?.type === 'battle' && nextRoom.encounter.creature && !nextRoom.visited) {
      const battle = createBattle(player, nextRoom.encounter.creature)
      set({ battle })
    }
  },

  startBattle: () => {
    const { currentRoom, player } = get()
    if (!currentRoom || !player || !currentRoom.encounter?.creature) return

    const battle = createBattle(player, currentRoom.encounter.creature)
    set({ battle })
  },

  setBattle: (battle) => {
    set({ battle })
  },

  endBattle: () => {
    const { currentRoom } = get()
    if (currentRoom) {
      currentRoom.visited = true
    }
    set({ battle: null })
  },

  resetGame: () => {
    set(initialState)
  },
}))
