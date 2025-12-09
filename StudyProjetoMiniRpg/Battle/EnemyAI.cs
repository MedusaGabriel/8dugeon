using System;

public class EnemyAI
{
    private readonly Random _rng = new Random();

    public EnemyAttackStyle EscolherEstiloAtaque(Enemy inimigo)
    {
        if (inimigo.StaminaAtual >= 3)
        {
            int roll = _rng.Next(0, 3); 
            return roll switch
            {
                0 => EnemyAttackStyle.Rapido,
                1 => EnemyAttackStyle.Fraco,
                _ => EnemyAttackStyle.Forte
            };
        }

        if (inimigo.StaminaAtual == 2)
        {
            int roll = _rng.Next(0, 2); 
            return roll == 0 ? EnemyAttackStyle.Rapido : EnemyAttackStyle.Fraco;
        }

        return EnemyAttackStyle.Rapido;
    }

    public int ObterCustoPorEstilo(EnemyAttackStyle estilo)
    {
        return estilo switch
        {
            EnemyAttackStyle.Rapido => 1,
            EnemyAttackStyle.Fraco => 1,
            EnemyAttackStyle.Forte => 3,
            _ => 1
        };
    }

    public int CalcularDanoDoEstilo(int danoBase, EnemyAttackStyle estilo)
    {
        int danoFinal = estilo switch
        {
            EnemyAttackStyle.Rapido => (int)Math.Round(danoBase * 0.8),
            EnemyAttackStyle.Fraco => (int)Math.Round(danoBase * 0.5),
            EnemyAttackStyle.Forte => (int)Math.Round(danoBase * 1.5),
            _ => danoBase
        };

        return Math.Max(1, danoFinal); 
    }

    public bool TentarAtacar(Hero heroi, Enemy inimigo, PositionSystem positionSystem, string heroClassKey, HeroNarrationRepository heroNarrations)
    {
        if (!positionSystem.InimigoPodeAtacar(inimigo))
        {
            positionSystem.InimigoAproxima();
            BattleUI.Print($"{inimigo.Nome} se aproxima, tentando entrar em alcance para atacar.");
            return false;
        }

        if (inimigo.StaminaAtual < 1)
        {
            BattleUI.Print("A criatura parece exausta e recua por um instante, recuperando o fôlego...");
            RecuperarStamina(inimigo);
            return false;
        }

        EnemyAttackStyle estilo = EscolherEstiloAtaque(inimigo);
        int custo = ObterCustoPorEstilo(estilo);

        if (inimigo.StaminaAtual < custo)
        {
            BattleUI.Print("A criatura vacila por um instante, sem forças suficientes para atacar.");
            RecuperarStamina(inimigo);
            return false;
        }

        string? textoAtaqueInimigo = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado, estilo);
        if (!string.IsNullOrWhiteSpace(textoAtaqueInimigo))
            BattleUI.Print(textoAtaqueInimigo);

        int danoFinal = CalcularDanoDoEstilo(inimigo.Ataque, estilo);
        heroi.TomarDano(danoFinal);

        inimigo.StaminaAtual -= custo;
        if (inimigo.StaminaAtual < 0)
            inimigo.StaminaAtual = 0;

        string narracaoTomarDanoHeroi = heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.TomarDano);
        BattleUI.Print(narracaoTomarDanoHeroi);

        if (heroi.hp <= 0)
        {
            BattleUI.ExibirMensagemHeroiMorreu();
            return true;
        }

        return false;
    }

    private void RecuperarStamina(Enemy inimigo)
    {
        inimigo.StaminaAtual += inimigo.RecuperacaoPorTurno;
        if (inimigo.StaminaAtual > inimigo.StaminaMax)
            inimigo.StaminaAtual = inimigo.StaminaMax;
    }
}
