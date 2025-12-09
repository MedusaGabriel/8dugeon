using System;
using System.Threading;

public static class BattleSystem
{
    public static void Executar(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();

        ExibirAparicaoInimigo(inimigo);

        Random rng = new Random();

        while (heroi.hp > 0 && inimigo.Hp > 0)
        {
            ExibirStatusInicioTurno(heroi, inimigo);

            AcaoJogador acao = PlayerInput.LerAcaoJogador();

            bool batalhaTerminou = false;

            switch (acao)
            {
                case AcaoJogador.Atacar:
                    batalhaTerminou = ExecutarTurnoAtaque(heroi, inimigo);
                    break;

                case AcaoJogador.Defender:
                    batalhaTerminou = ExecutarTurnoDefesa(heroi, inimigo, rng);
                    break;

                case AcaoJogador.Analisar:
                    ExecutarTurnoAnalise(heroi, inimigo);
                    break;

                case AcaoJogador.Fugir:
                    batalhaTerminou = ExecutarTurnoFuga(heroi);
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
        {
            Console.WriteLine(textoAparicao);
        }
        else
        {
            Console.WriteLine("Uma presença desconhecida surge à sua frente...");
        }
        Console.WriteLine();
    }
    static void ExibirStatusInicioTurno(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();
        Console.WriteLine($"Sua vida atual é: {heroi.hp}");

        if (!inimigo.Revelado)
        {
            Console.WriteLine("A criatura desconhecida lhe viu e está se preparando para lhe atacar.");
        }
        else
        {
            Console.WriteLine($"{inimigo.Nome} encara você, pronto para atacar novamente.");
        }
        Console.WriteLine();
    }
    static bool ExecutarTurnoAtaque(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();
        Console.WriteLine("Você tenta ser mais rápido e parte para o ataque!");
        Console.WriteLine();
        heroi.Atacar(inimigo);

        if (inimigo.Hp > 0)
        {
            string? textoHurt = NarrationRepository.GetRandomHurtText(inimigo.ClasseId);
            if (!string.IsNullOrWhiteSpace(textoHurt))
            {
                Console.WriteLine(textoHurt);
            }
        }
        if (inimigo.Hp <= 0)
        {
            string? textoMorte = NarrationRepository.GetRandomDeathText(inimigo.ClasseId);
            if (!string.IsNullOrWhiteSpace(textoMorte))
            {
                Console.WriteLine(textoMorte);
            }
            else
            {
                Console.WriteLine("O inimigo desconhecido foi derrotado antes que pudesse atacar!");
            }
            return true;
        }

        string? textoAtaqueInimigo = NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);
        if (!string.IsNullOrWhiteSpace(textoAtaqueInimigo))
        {
            Console.WriteLine();
            Console.WriteLine(textoAtaqueInimigo);
        }
        heroi.TomarDano(inimigo.Ataque);
        if (heroi.hp <= 0)
        {
            Console.WriteLine("O herói foi derrotado!");
            return true;
        }
        return false;
    }
    static bool ExecutarTurnoDefesa(Hero heroi, Enemy inimigo, Random rng)
    {
        Console.WriteLine();
        Console.WriteLine("Você tenta erguer a defesa a tempo...");

        int rolagem = rng.Next(0, 100);

        if (rolagem < 50)
        {
            Console.WriteLine();
            Console.WriteLine("Você foi rápido o suficiente! Sua defesa reduz o dano pela metade!");

            string? textoAtaqueDefendido =
                NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);

            if (!string.IsNullOrWhiteSpace(textoAtaqueDefendido))
            {
                Console.WriteLine(textoAtaqueDefendido);
            }

            int danoReduzido = inimigo.Ataque / 2;
            heroi.TomarDano(danoReduzido);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Você foi lento demais! Não conseguiu se defender a tempo.");

            string? textoAtaqueFalhaDefesa =
                NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);

            if (!string.IsNullOrWhiteSpace(textoAtaqueFalhaDefesa))
            {
                Console.WriteLine(textoAtaqueFalhaDefesa);
            }

            heroi.TomarDano(inimigo.Ataque);
        }

        if (heroi.hp <= 0)
        {
            Console.WriteLine();
            Console.WriteLine("Mesmo se defendendo, o herói foi derrotado....");
            return true;
        }

        return false;
    }


    static void ExecutarTurnoAnalise(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();

        if (!inimigo.Revelado)
        {
            Console.WriteLine();
            Console.WriteLine("Um olho intertemporal surge em sua mente, revelando completamente a criatura à sua frente!");
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine("O tempo desacelera por um momento enquanto você observa atentamente o inimigo...");
            Thread.Sleep(1000);

            inimigo.Revelar();

            Console.WriteLine();
            Console.WriteLine("Agora você entende melhor o inimigo:");
            Console.WriteLine();
            Console.WriteLine($"Nome: {inimigo.Nome}");
            Console.WriteLine();
            Console.WriteLine($"HP: {inimigo.Hp}");
            Console.WriteLine();
            Console.WriteLine($"Descrição: {inimigo.Descricao}");
            Console.WriteLine();
            Console.WriteLine($"Ataque: {inimigo.Ataque}");
            Console.WriteLine();
            Console.WriteLine($"Defesa: {inimigo.Defesa}");
            Console.WriteLine();
            Console.WriteLine($"Alcance: {inimigo.Alcance}");
            Console.WriteLine();

            string? textoAparicaoRevel = NarrationRepository.GetRandomAppearText(inimigo.ClasseId, true);
            if (!string.IsNullOrWhiteSpace(textoAparicaoRevel))
            {
                Console.WriteLine(textoAparicaoRevel);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Você já analisou esse inimigo. Nada novo é revelado.");
        }
    }

    static bool ExecutarTurnoFuga(Hero heroi)
    {
        Console.WriteLine();
        Console.WriteLine("Você decide recuar e fugir da batalha...");


        return true;
    }


}
