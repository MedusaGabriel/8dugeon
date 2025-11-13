import { Room, Encounter, Creature } from '@/types/game'
import { getRandomCreature } from '@/data/creatures'

export function generateRoom(id: string, playerLevel: number): Room {
  const roomTypes = [
    'Uma sala escura com pilares antigos.',
    'Um corredor estreito coberto de teias de aranha.',
    'Uma câmara ampla com ecos distantes.',
    'Um salão com paredes de pedra úmidas.',
    'Uma passagem iluminada por tochas bruxuleantes.',
    'Uma sala com ruínas de uma civilização antiga.',
    'Um túnel natural com estalactites.',
    'Uma cripta com sarcófagos quebrados.',
  ]

  const description = roomTypes[Math.floor(Math.random() * roomTypes.length)]

  // 60% chance de encontro, 30% vazio, 10% tesouro
  const encounterRoll = Math.random() * 100
  let encounter: Encounter | undefined

  if (encounterRoll < 60) {
    // Batalha
    const creature = getRandomCreature(playerLevel)
    encounter = {
      type: 'battle',
      creature,
      description: `Você encontra um ${creature.name}!`,
    }
  } else if (encounterRoll < 70) {
    // Tesouro
    encounter = {
      type: 'treasure',
      description: 'Você encontra um baú com tesouros!',
    }
  }

  return {
    id,
    description,
    connections: generateConnections(),
    encounter,
    visited: false,
  }
}

function generateConnections(): Room['connections'] {
  const connections: Room['connections'] = {}

  // Sempre tem pelo menos uma saída
  const directions: Array<'left' | 'right' | 'forward' | 'back'> = [
    'left',
    'right',
    'forward',
    'back',
  ]

  // Número aleatório de saídas (1-3)
  const numExits = Math.floor(Math.random() * 3) + 1

  // Embaralha direções
  const shuffled = directions.sort(() => Math.random() - 0.5)

  for (let i = 0; i < numExits; i++) {
    connections[shuffled[i]] = crypto.randomUUID()
  }

  return connections
}

export function createInitialRoom(): Room {
  return {
    id: 'entrance',
    description: 'Você está na entrada da masmorra. A escuridão se estende à sua frente...',
    connections: {
      forward: crypto.randomUUID(),
    },
    visited: true,
  }
}

export function getRoomDescription(room: Room): string {
  let desc = room.description

  const directions = Object.keys(room.connections)
  if (directions.length > 0) {
    desc += `\n\nVocê vê ${directions.length} saída(s): ${directions.join(', ')}.`
  }

  if (room.encounter && !room.visited) {
    desc += `\n\n⚠️ ${room.encounter.description}`
  }

  return desc
}

export function getAvailableDirections(room: Room): Array<'left' | 'right' | 'forward' | 'back'> {
  return Object.keys(room.connections) as Array<'left' | 'right' | 'forward' | 'back'>
}
