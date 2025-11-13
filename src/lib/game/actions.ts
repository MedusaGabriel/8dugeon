import { Attack, ActionResult, Player, Creature, BattleLogEntry } from '@/types/game'

export function calculateDamage(
  attacker: Player | Creature,
  defender: Player | Creature,
  attack: Attack,
  isCritical: boolean = false
): number {
  const baseAttack = 'class' in attacker ? attacker.currentStats.attack : attacker.stats.attack
  const defense = 'class' in defender ? defender.currentStats.defense : defender.stats.defense

  let damage = attack.damage + baseAttack - defense * 0.5

  // Crítico
  if (isCritical) {
    damage *= 2
  }

  // Mínimo de 1 de dano (exceto cura)
  if (attack.type !== 'healing' && damage < 1) {
    damage = 1
  }

  return Math.floor(damage)
}

export function checkHit(attack: Attack): boolean {
  const roll = Math.random() * 100
  return roll <= attack.accuracy
}

export function checkCritical(attacker: Player | Creature): boolean {
  let critChance = 5 // 5% base

  // Ladino tem +20% de crítico
  if ('class' in attacker && attacker.class === 'rogue') {
    critChance += 20
  }

  const roll = Math.random() * 100
  return roll <= critChance
}

export function executeAttack(
  attacker: Player | Creature,
  defender: Player | Creature,
  attack: Attack
): ActionResult {
  // Verifica se acertou
  if (!checkHit(attack)) {
    return {
      success: false,
      message: `${getActorName(attacker)} errou o ataque!`,
    }
  }

  // Verifica crítico
  const isCritical = attack.type === 'physical' && checkCritical(attacker)

  // Calcula dano
  const damage = calculateDamage(attacker, defender, attack, isCritical)

  // Aplica dano ou cura
  if (attack.type === 'healing') {
    const healing = Math.abs(damage)
    const isPlayer = 'class' in attacker
    if (isPlayer) {
      const player = attacker as Player
      player.currentStats.hp = Math.min(player.currentStats.hp + healing, player.stats.maxHp)
    }
    return {
      success: true,
      message: `${getActorName(attacker)} se curou!`,
      healing,
    }
  }

  // Aplica dano ao defensor
  const isDefenderPlayer = 'class' in defender
  if (isDefenderPlayer) {
    const player = defender as Player
    player.currentStats.hp = Math.max(0, player.currentStats.hp - damage)
  } else {
    const creature = defender as Creature
    creature.stats.hp = Math.max(0, creature.stats.hp - damage)
  }

  return {
    success: true,
    message: `${getActorName(attacker)} usou ${attack.name}!`,
    damage,
    critical: isCritical,
    statusEffect: attack.statusEffect,
  }
}

export function applyStatusEffects(entity: Player | Creature): BattleLogEntry[] {
  const logs: BattleLogEntry[] = []

  // Verifica se é player ou criatura
  const isPlayer = 'class' in entity

  if (isPlayer) {
    const player = entity as Player
    player.statusEffects.forEach((effect) => {
      if (effect.stat === 'hp') {
        player.currentStats.hp = Math.max(0, player.currentStats.hp + effect.value)
        logs.push({
          id: crypto.randomUUID(),
          timestamp: Date.now(),
          message: `${player.name} perdeu ${Math.abs(effect.value)} HP de ${effect.name}`,
          type: 'damage',
          actor: 'player',
        })
      }

      // Reduz duração
      effect.duration--
    })

    // Remove efeitos expirados
    player.statusEffects = player.statusEffects.filter((e) => e.duration > 0)
  }

  return logs
}

export function canUseAttack(attacker: Player | Creature, attack: Attack): boolean {
  const isPlayer = 'class' in attacker
  const mana = isPlayer ? (attacker as Player).currentStats.mana : (attacker as Creature).stats.mana

  return mana >= attack.manaCost
}

export function useAttack(attacker: Player | Creature, attack: Attack): void {
  const isPlayer = 'class' in attacker
  if (isPlayer) {
    const player = attacker as Player
    player.currentStats.mana = Math.max(0, player.currentStats.mana - attack.manaCost)
  } else {
    const creature = attacker as Creature
    creature.stats.mana = Math.max(0, creature.stats.mana - attack.manaCost)
  }
}

export function getActorName(actor: Player | Creature): string {
  if ('class' in actor) {
    return (actor as Player).name
  }
  return (actor as Creature).name
}

export function isDefeated(entity: Player | Creature): boolean {
  const isPlayer = 'class' in entity
  const hp = isPlayer ? (entity as Player).currentStats.hp : (entity as Creature).stats.hp
  return hp <= 0
}

export function gainXP(player: Player, xp: number): boolean {
  player.xp += xp

  if (player.xp >= player.xpToNextLevel) {
    // Level up!
    player.level++
    player.xp -= player.xpToNextLevel
    player.xpToNextLevel = Math.floor(player.xpToNextLevel * 1.5)

    // Aumenta stats
    player.stats.maxHp += 10
    player.stats.maxMana += 5
    player.stats.attack += 2
    player.stats.defense += 2
    player.stats.speed += 1

    // Cura completo no level up
    player.currentStats = { ...player.stats }

    return true
  }

  return false
}
