using System;

NarrationRepository.LoadFromJson("Data/enemy_narrations.json");
Enemy Inimigo = EnemyFactory.CriarInimigoAleatorio();
Hero Heroi = Hero.CriarHeroi();
Batalha(Heroi, Inimigo);


static void Batalha(Hero heroi, Enemy inimigo)
{
    Console.WriteLine();

    // Narração de aparição, respeitando se o inimigo está revelado ou não
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

    Random rng = new Random();

    while (heroi.hp > 0 && inimigo.Hp > 0)
    {
        Console.WriteLine();
        Console.WriteLine($"Sua vida atual é: {heroi.hp}");
        Console.WriteLine();

        if (!inimigo.Revelado)
        {
            Console.WriteLine("A criatura desconhecida lhe viu e está se preparando para lhe atacar.");
        }
        else
        {
            Console.WriteLine($"{inimigo.Nome} encara você, pronto para atacar novamente.");
        }
        Console.WriteLine();

        AcaoJogador acao = PlayerInput.LerAcaoJogador();

        switch (acao)
        {
            case AcaoJogador.Atacar:
                Console.WriteLine();
                Console.WriteLine("Você tenta ser mais rápido e parte para o ataque!");
                Console.WriteLine();

                // Futuro: PlayerNarrations aqui
                heroi.Atacar(inimigo);

                // Se ainda estiver vivo, narração de dano
                if (inimigo.Hp > 0)
                {
                    string? textoHurt = NarrationRepository.GetRandomHurtText(inimigo.ClasseId);
                    if (!string.IsNullOrWhiteSpace(textoHurt))
                    {
                        Console.WriteLine(textoHurt);
                    }
                }

                // Verifica se o inimigo morreu
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
                    return;
                }

                // Contra-ataque do inimigo
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
                    return;
                }
                break;

            case AcaoJogador.Defender:
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
                    return;
                }
                break;

            case AcaoJogador.Analisar:
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

                    // Opcional: se quiser reforçar a revelação com uma fala específica,
                    // você pode chamar aqui um texto de aparição revelado:
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
                break;

            case AcaoJogador.Fugir:
                Console.WriteLine();
                Console.WriteLine("Você decide recuar e fugir da batalha...");
                return;
        }
    }
}