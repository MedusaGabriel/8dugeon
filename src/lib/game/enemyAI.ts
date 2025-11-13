import { Creature, Player, Attack } from '@/types/game'
import { canUseAttack } from './actions'

export type AIStrategy = 'aggressive' | 'defensive' | 'balanced' | 'smart'

/**
 * Seleciona o melhor ataque baseado na estratégia da criatura
 */
export function selectEnemyAttack(
  enemy: Creature,
  player: Player,
  strategy: AIStrategy = 'balanced'
): Attack | null {
  const availableAttacks = enemy.attacks.filter((atk) => canUseAttack(enemy, atk))

  if (availableAttacks.length === 0) {
    return null
  }

  switch (strategy) {
    case 'aggressive':
      return selectAggressiveAttack(availableAttacks, enemy, player)
    case 'defensive':
      return selectDefensiveAttack(availableAttacks, enemy, player)
    case 'smart':
      return selectSmartAttack(availableAttacks, enemy, player)
    case 'balanced':
    default:
      return selectBalancedAttack(availableAttacks, enemy, player)
  }
}

/**
 * Estratégia agressiva: sempre o ataque com maior dano
 */
function selectAggressiveAttack(attacks: Attack[], enemy: Creature, player: Player): Attack {
  return attacks.reduce((best, current) => (current.damage > best.damage ? current : best))
}

/**
 * Estratégia defensiva: prioriza cura e buffs
 */
function selectDefensiveAttack(attacks: Attack[], enemy: Creature, player: Player): Attack {
  // Se HP baixo (< 30%), tenta curar
  const hpPercent = (enemy.stats.hp / enemy.stats.maxHp) * 100
  if (hpPercent < 30) {
    const healAttack = attacks.find((atk) => atk.type === 'healing')
    if (healAttack) return healAttack
  }

  // Prioriza buffs se ainda não tem
  const buffAttack = attacks.find((atk) => atk.statusEffect?.type === 'buff')
  if (buffAttack) {
    return buffAttack
  }

  // Senão, ataque aleatório
  return attacks[Math.floor(Math.random() * attacks.length)]
}

/**
 * Estratégia inteligente: analisa situação e escolhe melhor opção
 */
function selectSmartAttack(attacks: Attack[], enemy: Creature, player: Player): Attack {
  const enemyHpPercent = (enemy.stats.hp / enemy.stats.maxHp) * 100
  const playerHpPercent = (player.currentStats.hp / player.stats.maxHp) * 100

  // Se inimigo está com HP muito baixo (< 20%), tenta curar
  if (enemyHpPercent < 20) {
    const healAttack = attacks.find((atk) => atk.type === 'healing')
    if (healAttack) return healAttack
  }

  // Se player está com HP baixo (< 30%), ataque mais forte para finalizar
  if (playerHpPercent < 30) {
    return attacks.reduce((best, current) => (current.damage > best.damage ? current : best))
  }

  // Se player não tem debuff, tenta aplicar
  const playerHasDebuff = player.statusEffects?.some((e) => e.type === 'debuff')
  if (!playerHasDebuff) {
    const debuffAttack = attacks.find((atk) => atk.statusEffect?.type === 'debuff')
    if (debuffAttack) return debuffAttack
  }

  // Senão, ataque balanceado
  return selectBalancedAttack(attacks, enemy, player)
}

/**
 * Estratégia balanceada: 70% ataque forte, 30% aleatório
 */
function selectBalancedAttack(attacks: Attack[], enemy: Creature, player: Player): Attack {
  const roll = Math.random()

  if (roll < 0.7) {
    // 70% - escolhe um dos ataques mais fortes
    const sortedByDamage = [...attacks].sort((a, b) => b.damage - a.damage)
    const topAttacks = sortedByDamage.slice(0, Math.max(2, Math.ceil(attacks.length / 2)))
    return topAttacks[Math.floor(Math.random() * topAttacks.length)]
  } else {
    // 30% - ataque aleatório para imprevisibilidade
    return attacks[Math.floor(Math.random() * attacks.length)]
  }
}

/**
 * Determina a estratégia baseada no tipo/nível da criatura
 */
export function getEnemyStrategy(enemy: Creature): AIStrategy {
  // Boss ou criaturas de nível alto = smart
  if (enemy.level >= 8) {
    return 'smart'
  }

  // Criaturas médias = balanceado
  if (enemy.level >= 4) {
    return 'balanced'
  }

  // Criaturas fracas podem ser agressivas ou defensivas aleatoriamente
  return Math.random() > 0.5 ? 'aggressive' : 'defensive'
}
