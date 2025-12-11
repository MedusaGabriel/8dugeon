using System;

public class BattleController
{
    private readonly HeroStats _hero;
    private readonly EnemyStats _enemy;
    private readonly BattleSystem _battleSystem;

    public HeroStats Hero => _hero;
    public EnemyStats Enemy => _enemy;

    public BattleController(
        HeroStats hero,
        EnemyStats enemy,
        float enemyDodgeChance,
        float enemyCounterChanceOnDodge,
        float playerPerfectBlockChance
    )
    {
        _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));

        _battleSystem = new BattleSystem(
            _hero,
            _enemy,
            enemyDodgeChance,
            enemyCounterChanceOnDodge,
            playerPerfectBlockChance
        );
    }

    /// <summary>
    /// Executa a ação do jogador e delega para o BattleSystem.
    /// Aqui fica a lógica de "se atacar faz X, se defender faz Y, etc."
    /// </summary>
    public BattleRoundResult ExecutePlayerAction(AcaoJogador acao)
    {
        BattleRoundResult result = null;

        switch (acao)
        {
            case AcaoJogador.Atacar:
                result = _battleSystem.PlayerAttack();
                break;

            case AcaoJogador.Defender:
                result = _battleSystem.PlayerDefend();
                break;

            case AcaoJogador.Fugir:
                result = _battleSystem.PlayerFlee();
                break;

            default:
                // De propósito: se ação não reconhecida chegar até aqui, é bug de quem chamou.
                throw new InvalidOperationException($"Ação de jogador inválida: {acao}");
        }

        return result;
    }
}
