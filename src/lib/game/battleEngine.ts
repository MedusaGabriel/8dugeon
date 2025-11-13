import { BattleState, Player, Creature, BattleLogEntry, Attack } from '@/types/game'
import {
  executeAttack,
  applyStatusEffects,
  canUseAttack,
  useAttack,
  isDefeated,
  getActorName,
  gainXP,
} from './actions'
import { selectEnemyAttack, getEnemyStrategy } from './enemyAI'

export function createBattle(player: Player, enemy: Creature): BattleState {
  return {
    player,
    enemy,
    turn: player.stats.speed >= enemy.stats.speed ? 'player' : 'enemy',
    round: 1,
    battleLog: [
      {
        id: crypto.randomUUID(),
        timestamp: Date.now(),
        message: `Um ${enemy.name} apareceu!`,
        type: 'info',
        actor: 'enemy',
      },
    ],
    isActive: true,
  }
}

export function addBattleLog(
  battle: BattleState,
  message: string,
  type: BattleLogEntry['type'],
  actor: 'player' | 'enemy'
): void {
  battle.battleLog.push({
    id: crypto.randomUUID(),
    timestamp: Date.now(),
    message,
    type,
    actor,
  })
}

export function playerAttack(battle: BattleState, attack: Attack): BattleState {
  if (!battle.isActive || battle.turn !== 'player') {
    return battle
  }

  // Verifica se pode usar o ataque
  if (!canUseAttack(battle.player, attack)) {
    addBattleLog(battle, 'Mana insuficiente!', 'info', 'player')
    return battle
  }

  // Usa o ataque
  useAttack(battle.player, attack)

  // Executa o ataque
  const result = executeAttack(battle.player, battle.enemy, attack)

  // Adiciona ao log
  addBattleLog(battle, result.message, 'attack', 'player')

  if (result.success) {
    if (result.damage) {
      const damageMsg = result.critical
        ? `CRÍTICO! ${result.damage} de dano!`
        : `${result.damage} de dano`
      addBattleLog(battle, damageMsg, result.critical ? 'critical' : 'damage', 'player')
    }

    if (result.healing) {
      addBattleLog(battle, `Curou ${result.healing} HP`, 'heal', 'player')
    }

    if (result.statusEffect) {
      addBattleLog(
        battle,
        `${battle.enemy.name} está ${result.statusEffect.name}!`,
        'status',
        'player'
      )
    }
  }

  // Verifica se o inimigo foi derrotado
  if (isDefeated(battle.enemy)) {
    battle.isActive = false
    battle.result = 'victory'
    addBattleLog(battle, `${battle.enemy.name} foi derrotado!`, 'info', 'enemy')

    // Ganha XP e Gold
    const leveledUp = gainXP(battle.player, battle.enemy.xpReward)
    battle.player.gold += battle.enemy.goldReward

    addBattleLog(
      battle,
      `+${battle.enemy.xpReward} XP, +${battle.enemy.goldReward} Gold`,
      'info',
      'player'
    )

    if (leveledUp) {
      addBattleLog(
        battle,
        `🎉 LEVEL UP! Você é agora nível ${battle.player.level}!`,
        'info',
        'player'
      )
    }

    return battle
  }

  // Aplica efeitos de status no player
  const statusLogs = applyStatusEffects(battle.player)
  statusLogs.forEach((log) => battle.battleLog.push(log))

  // Passa o turno para o inimigo
  battle.turn = 'enemy'

  return battle
}

export function enemyTurn(battle: BattleState): BattleState {
  console.log('🤖 Enemy Turn - Starting', {
    isActive: battle.isActive,
    turn: battle.turn,
    enemyName: battle.enemy.name,
    enemyHp: battle.enemy.stats.hp,
  })

  if (!battle.isActive) {
    console.log('❌ Battle not active')
    return battle
  }

  if (battle.turn !== 'enemy') {
    console.log('❌ Not enemy turn:', battle.turn)
    return battle
  }

  // Determina estratégia da IA baseada na criatura
  const strategy = getEnemyStrategy(battle.enemy)
  console.log('🎯 Strategy:', strategy)

  // Seleciona o ataque usando IA
  const attack = selectEnemyAttack(battle.enemy, battle.player, strategy)

  if (!attack) {
    console.log('⚠️ No available attacks')
    addBattleLog(battle, `${battle.enemy.name} não pode atacar!`, 'info', 'enemy')
    battle.turn = 'player'
    battle.round++
    return battle
  }

  console.log('⚔️ Selected attack:', attack.name)

  // Usa o ataque
  useAttack(battle.enemy, attack)

  // Executa o ataque
  const result = executeAttack(battle.enemy, battle.player, attack)
  console.log('💥 Attack result:', result)

  addBattleLog(battle, result.message, 'attack', 'enemy')

  if (result.success && result.damage) {
    const damageMsg = result.critical
      ? `CRÍTICO! Você recebeu ${result.damage} de dano!`
      : `Você recebeu ${result.damage} de dano`
    addBattleLog(battle, damageMsg, result.critical ? 'critical' : 'damage', 'enemy')
  }

  // Verifica se o player foi derrotado
  if (isDefeated(battle.player)) {
    console.log('💀 Player defeated')
    battle.isActive = false
    battle.result = 'defeat'
    addBattleLog(battle, 'Você foi derrotado...', 'info', 'player')
    return battle
  }

  // Aplica efeitos de status no inimigo
  const statusLogs = applyStatusEffects(battle.enemy)
  statusLogs.forEach((log) => battle.battleLog.push(log))

  // Passa o turno para o player
  console.log('✅ Ending enemy turn, switching to player')
  battle.turn = 'player'
  battle.round++

  return battle
}

export function attemptFlee(battle: BattleState): BattleState {
  if (!battle.isActive || battle.turn !== 'player') {
    return battle
  }

  // 50% de chance de fugir
  const fleeChance = 50
  const roll = Math.random() * 100

  if (roll <= fleeChance) {
    battle.isActive = false
    battle.result = 'fled'
    addBattleLog(battle, 'Você fugiu da batalha!', 'info', 'player')
  } else {
    addBattleLog(battle, 'Não conseguiu fugir!', 'info', 'player')
    // Inimigo ataca
    battle.turn = 'enemy'
  }

  return battle
}
