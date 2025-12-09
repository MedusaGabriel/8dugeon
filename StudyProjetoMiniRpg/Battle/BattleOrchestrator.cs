using System;

public class BattleOrchestrator
{
    private readonly PositionSystem _positionSystem;
    private readonly TurnExecutor _turnExecutor;
    private readonly EnemyAI _enemyAI;
    private readonly InspectionSystem _inspectionSystem;
    private readonly HeroNarrationRepository _heroNarrations;

    public BattleOrchestrator()
    {
        _heroNarrations = new HeroNarrationRepository();
        _positionSystem = new PositionSystem(0, 3);
        _enemyAI = new EnemyAI();
        _turnExecutor = new TurnExecutor(_enemyAI, _heroNarrations);
        _inspectionSystem = new InspectionSystem();
    }

    public void Executar(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        Console.WriteLine();

        BattleUI.ExibirAparicaoInimigo(inimigo);

        while (heroi.hp > 0 && inimigo.Hp > 0)
        {
            BattleUI.ExibirStatusInicioTurno(heroi, inimigo);

            ComandoJogador comando = PlayerInput.LerComandoJogador(heroi);
            AcaoJogador acao = comando.AcaoBase;

            bool batalhaTerminou = false;

            switch (acao)
            {
                case AcaoJogador.Atacar:
                    batalhaTerminou = _turnExecutor.ExecutarTurnoAtaque(heroi, inimigo, _positionSystem, heroClassKey);
                    break;

                case AcaoJogador.Defender:
                    batalhaTerminou = _turnExecutor.ExecutarTurnoDefesa(heroi, inimigo, heroClassKey);
                    break;

                case AcaoJogador.Analisar:
                    _inspectionSystem.PermitirEscolhaAnalise(heroi, inimigo, heroClassKey);
                    break;

                case AcaoJogador.Fugir:
                    batalhaTerminou = _turnExecutor.ExecutarTurnoFuga(heroi, heroClassKey);
                    break;

                    // Ações futuras de movimento
                    // case AcaoJogador.Aproximar:
                    //     _positionSystem.Aproximar();
                    //     break;
                    //
                    // case AcaoJogador.Recuar:
                    //     _positionSystem.Recuar();
                    //     break;
            }

            if (batalhaTerminou)
                return;
        }
    }
}
