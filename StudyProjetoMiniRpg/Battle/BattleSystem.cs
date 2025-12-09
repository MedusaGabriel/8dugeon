using System;
using System.Threading;

public static class BattleSystem
{
    private static readonly HeroNarrationRepository _heroNarrations = new HeroNarrationRepository();

    static void Print(string msg, int delay = 1000)
    {
        Console.WriteLine(msg);
        Thread.Sleep(delay);
    }

    public static void Executar(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        Console.WriteLine();

        ExibirAparicaoInimigo(inimigo);

        Random rng = new Random();

        while (heroi.hp > 0 && inimigo.Hp > 0)
        {
            ExibirStatusInicioTurno(heroi, inimigo);

            ComandoJogador comando = PlayerInput.LerComandoJogador(heroi);
            AcaoJogador acao = comando.AcaoBase;

            bool batalhaTerminou = false;

            switch (acao)
            {
                case AcaoJogador.Atacar:
                    // Futuro: usar comando.AtaqueForte aqui
                    batalhaTerminou = ExecutarTurnoAtaque(heroi, inimigo, heroClassKey);
                    break;

                case AcaoJogador.Defender:
                    batalhaTerminou = ExecutarTurnoDefesa(heroi, inimigo, rng, heroClassKey);
                    break;

                case AcaoJogador.Analisar:
                    ExecutarTurnoAnalise(heroi, inimigo);
                    break;

                case AcaoJogador.Fugir:
                    batalhaTerminou = ExecutarTurnoFuga(heroi, heroClassKey);
                    break;
            }

            if (batalhaTerminou)
                return;
        }
    }

    static void ExibirAparicaoInimigo(Enemy inimigo)
    {
        string? textoAparicao = NarrationRepository.GetRandomAppearText(inimigo.ClasseId, inimigo.Revelado);

        if (!string.IsNullOrWhiteSpace(textoAparicao))
            Print(textoAparicao);
        else
            Print("Uma presença desconhecida surge à sua frente...");

        Console.WriteLine();
    }

    static void ExibirStatusInicioTurno(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();
        Console.WriteLine($"Sua vida atual é: {heroi.hp}");

        if (!inimigo.Revelado)
            Print("A criatura desconhecida lhe viu e está se preparando para lhe atacar.");
        else
            Print($"{inimigo.Nome} encara você, pronto para atacar novamente.");

        Console.WriteLine();
    }

    static bool ExecutarTurnoAtaque(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        Console.WriteLine();

        Print(_heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.CausarDano));

        Console.WriteLine();

        heroi.Atacar(inimigo);

        if (inimigo.Hp > 0)
        {
            string? textoHurt = NarrationRepository.GetRandomHurtText(inimigo.ClasseId);
            if (!string.IsNullOrWhiteSpace(textoHurt))
                Print(textoHurt);
        }

        if (inimigo.Hp <= 0)
        {
            string? textoMorte = NarrationRepository.GetRandomDeathText(inimigo.ClasseId);

            if (!string.IsNullOrWhiteSpace(textoMorte))
                Print(textoMorte);
            else
                Print("O inimigo desconhecido foi derrotado antes que pudesse atacar!");

            return true;
        }

        string? textoAtaqueInimigo = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);
        if (!string.IsNullOrWhiteSpace(textoAtaqueInimigo))
            Print(textoAtaqueInimigo);

        heroi.TomarDano(inimigo.Ataque);

        Print(_heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.TomarDano));

        if (heroi.hp <= 0)
        {
            Print("O herói foi derrotado!");
            return true;
        }

        return false;
    }

    static bool ExecutarTurnoDefesa(Hero heroi, Enemy inimigo, Random rng, string heroClassKey)
    {
        Console.WriteLine();

        Print(_heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.Defender));

        int rolagem = rng.Next(0, 100);

        if (rolagem < 50)
        {
            Print("Você foi rápido o suficiente! Sua defesa reduz o dano pela metade!");

            string? textoAtaqueDefendido = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);
            if (!string.IsNullOrWhiteSpace(textoAtaqueDefendido))
                Print(textoAtaqueDefendido);

            heroi.TomarDano(inimigo.Ataque / 2);
        }
        else
        {
            Print("Você foi lento demais! Não conseguiu se defender a tempo.");

            string? textoAtaqueFalhaDefesa = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);
            if (!string.IsNullOrWhiteSpace(textoAtaqueFalhaDefesa))
                Print(textoAtaqueFalhaDefesa);

            heroi.TomarDano(inimigo.Ataque);
        }

        Print(_heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.TomarDano));

        if (heroi.hp <= 0)
        {
            Print("Mesmo se defendendo, o herói foi derrotado....");
            return true;
        }

        return false;
    }

    static void ExecutarTurnoAnalise(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();

        if (!inimigo.Revelado)
        {
            Print("Um olho intertemporal surge em sua mente, revelando completamente a criatura à sua frente!");
            Thread.Sleep(1000);
            Print("O tempo desacelera por um momento enquanto você observa atentamente o inimigo...");
            Thread.Sleep(1000);

            inimigo.Revelar();

            Print("Agora você entende melhor o inimigo:");
            Console.WriteLine($"Nome: {inimigo.Nome}");
            Console.WriteLine($"HP: {inimigo.Hp}");
            Console.WriteLine($"Descrição: {inimigo.Descricao}");
            Console.WriteLine($"Ataque: {inimigo.Ataque}");
            Console.WriteLine($"Defesa: {inimigo.Defesa}");
            Console.WriteLine($"Alcance: {inimigo.Alcance}");
            Console.WriteLine();

            string? textoAparicaoRevel = NarrationRepository.GetRandomAppearText(inimigo.ClasseId, true);
            if (!string.IsNullOrWhiteSpace(textoAparicaoRevel))
                Print(textoAparicaoRevel);
        }
        else
        {
            Print("Você já analisou esse inimigo. Nada novo é revelado.");
        }
    }

    static bool ExecutarTurnoFuga(Hero heroi, string heroClassKey)
    {
        Console.WriteLine();

        Print(_heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.Fugir));

        Print("Você decide recuar e fugir da batalha...");

        return true;
    }
}
