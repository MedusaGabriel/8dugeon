using System;

public class TurnExecutor
{
    private readonly EnemyAI _enemyAI;
    private readonly HeroNarrationRepository _heroNarrations;

    public TurnExecutor(EnemyAI enemyAI, HeroNarrationRepository heroNarrations)
    {
        _enemyAI = enemyAI;
        _heroNarrations = heroNarrations;
    }

    public bool ExecutarTurnoAtaque(Hero heroi, Enemy inimigo, PositionSystem positionSystem, string heroClassKey)
    {
        Console.WriteLine();

        if (!positionSystem.HeroiPodeAtacar(heroi))
        {
            BattleUI.ExibirMensagemAtaqueImpossivel();
            return _enemyAI.TentarAtacar(heroi, inimigo, positionSystem, heroClassKey, _heroNarrations);
        }

        string narracaoAtaqueHeroi = _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.CausarDano);
        BattleUI.Print(narracaoAtaqueHeroi);

        Console.WriteLine();

        heroi.Atacar(inimigo);

        if (inimigo.Hp > 0)
        {
            string? textoHurt = NarrationRepository.GetRandomHurtText(inimigo.ClasseId);
            if (!string.IsNullOrWhiteSpace(textoHurt))
                BattleUI.Print(textoHurt);
        }

        if (inimigo.Hp <= 0)
        {
            BattleUI.ExibirMensagemInimigoMorreu(inimigo);
            return true;
        }

        bool heroiMorreu = _enemyAI.TentarAtacar(heroi, inimigo, positionSystem, heroClassKey, _heroNarrations);
        return heroiMorreu;
    }

    public bool ExecutarTurnoDefesa(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        Console.WriteLine();

        string narracaoDefesaHeroi = _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.Defender);
        BattleUI.Print(narracaoDefesaHeroi);

        if (inimigo.StaminaAtual < inimigo.CustoAtaque)
        {
            BattleUI.Print("A criatura tenta se mover para atacar, mas está exausta demais para reagir ao seu movimento defensivo.");
            inimigo.StaminaAtual += inimigo.RecuperacaoPorTurno;
            if (inimigo.StaminaAtual > inimigo.StaminaMax)
                inimigo.StaminaAtual = inimigo.StaminaMax;

            return false;
        }

        Random rng = new Random();
        int rolagem = rng.Next(0, 100);

        if (rolagem < 50)
        {
            Console.WriteLine();
            BattleUI.Print("Você foi rápido o suficiente! Sua defesa reduz o dano pela metade!");

            string? textoAtaqueDefendido = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);
            if (!string.IsNullOrWhiteSpace(textoAtaqueDefendido))
                BattleUI.Print(textoAtaqueDefendido);

            int danoReduzido = inimigo.Ataque / 2;
            heroi.TomarDano(danoReduzido);
        }
        else
        {
            Console.WriteLine();
            BattleUI.Print("Você foi lento demais! Não conseguiu se defender a tempo.");

            string? textoAtaqueFalhaDefesa = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);
            if (!string.IsNullOrWhiteSpace(textoAtaqueFalhaDefesa))
                BattleUI.Print(textoAtaqueFalhaDefesa);

            heroi.TomarDano(inimigo.Ataque);
        }

        inimigo.StaminaAtual -= inimigo.CustoAtaque;
        if (inimigo.StaminaAtual < 0)
            inimigo.StaminaAtual = 0;

        string narracaoTomarDanoHeroi = _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.TomarDano);
        BattleUI.Print(narracaoTomarDanoHeroi);

        if (heroi.hp <= 0)
        {
            Console.WriteLine();
            BattleUI.Print("Mesmo se defendendo, o herói foi derrotado....");
            return true;
        }

        return false;
    }

    public bool ExecutarTurnoFuga(Hero heroi, string heroClassKey)
    {
        Console.WriteLine();

        string narracaoFuga = _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.Fugir);
        BattleUI.Print(narracaoFuga);

        BattleUI.Print("Você decide recuar e fugir da batalha...");

        return true;
    }
}
