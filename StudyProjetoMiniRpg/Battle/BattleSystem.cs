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
                batalhaTerminou = ExecutarTurnoAtaque(heroi, inimigo, heroClassKey);
                break;

            case AcaoJogador.Defender:
                batalhaTerminou = ExecutarTurnoDefesa(heroi, inimigo, rng, heroClassKey);
                break;

            case AcaoJogador.Analisar:
                ExecutarTurnoAnalise(heroi, inimigo, heroClassKey);
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
    static EnemyAttackStyle EscolherEstiloAtaque(Enemy inimigo)
    {
        var rng = new Random();

        if (inimigo.StaminaAtual >= 3)
        {
            int roll = rng.Next(0, 3); // 0,1,2
            return roll switch
            {
                0 => EnemyAttackStyle.Rapido,
                1 => EnemyAttackStyle.Fraco,
                _ => EnemyAttackStyle.Forte
            };
        }

        if (inimigo.StaminaAtual == 2)
        {
            int roll = rng.Next(0, 2); // 0,1
            return roll == 0 ? EnemyAttackStyle.Rapido : EnemyAttackStyle.Fraco;
        }

        return EnemyAttackStyle.Rapido;
    }

    static int ObterCustoPorEstilo(EnemyAttackStyle estilo)
    {
        return estilo switch
        {
            EnemyAttackStyle.Rapido => 1,
            EnemyAttackStyle.Fraco => 1,
            EnemyAttackStyle.Forte => 3,
            _ => 1
        };
    }




    static bool InimigoAtacaSeTiverStamina(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        if (inimigo.StaminaAtual < 1)
        {
            Print("A criatura parece exausta e recua por um instante, recuperando o fôlego...");
            inimigo.StaminaAtual += inimigo.RecuperacaoPorTurno;
            if (inimigo.StaminaAtual > inimigo.StaminaMax)
                inimigo.StaminaAtual = inimigo.StaminaMax;

            return false;
        }

        EnemyAttackStyle estilo = EscolherEstiloAtaque(inimigo);
        int custo = ObterCustoPorEstilo(estilo);

        if (inimigo.StaminaAtual < custo)
        {
            Print("A criatura vacila por um instante, sem forças suficientes para atacar.");
            inimigo.StaminaAtual += inimigo.RecuperacaoPorTurno;
            if (inimigo.StaminaAtual > inimigo.StaminaMax)
                inimigo.StaminaAtual = inimigo.StaminaMax;

            return false;
        }

        string? textoAtaqueInimigo =
            NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado, estilo);

        if (!string.IsNullOrWhiteSpace(textoAtaqueInimigo))
            Print(textoAtaqueInimigo);

        int danoBase = inimigo.Ataque;
        int danoFinal = danoBase;

        switch (estilo)
        {
            case EnemyAttackStyle.Rapido:
                danoFinal = (int)Math.Round(danoBase * 0.8);
                break;
            case EnemyAttackStyle.Fraco:
                danoFinal = (int)Math.Round(danoBase * 0.5);
                break;
            case EnemyAttackStyle.Forte:
                danoFinal = (int)Math.Round(danoBase * 1.5);
                break;
        }

        if (danoFinal < 1)
            danoFinal = 1;

        heroi.TomarDano(danoFinal);

        inimigo.StaminaAtual -= custo;
        if (inimigo.StaminaAtual < 0)
            inimigo.StaminaAtual = 0;

        string narracaoTomarDanoHeroi =
            _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.TomarDano);
        Print(narracaoTomarDanoHeroi);

        if (heroi.hp <= 0)
        {
            Print("O herói foi derrotado!");
            return true;
        }

        return false;
    }




    static bool ExecutarTurnoAtaque(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        Console.WriteLine();

        string narracaoAtaqueHeroi =
            _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.CausarDano);
        Print(narracaoAtaqueHeroi);

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

        bool heroiMorreu = InimigoAtacaSeTiverStamina(heroi, inimigo, heroClassKey);
        if (heroiMorreu)
            return true;

        return false;
    }


    static bool ExecutarTurnoDefesa(Hero heroi, Enemy inimigo, Random rng, string heroClassKey)
    {
        Console.WriteLine();

        string narracaoDefesaHeroi =
            _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.Defender);
        Print(narracaoDefesaHeroi);

        if (inimigo.StaminaAtual < inimigo.CustoAtaque)
        {
            Print("A criatura tenta se mover para atacar, mas está exausta demais para reagir ao seu movimento defensivo.");
            inimigo.StaminaAtual += inimigo.RecuperacaoPorTurno;
            if (inimigo.StaminaAtual > inimigo.StaminaMax)
                inimigo.StaminaAtual = inimigo.StaminaMax;

            return false;
        }

        int rolagem = rng.Next(0, 100);

        if (rolagem < 50)
        {
            Console.WriteLine();
            Print("Você foi rápido o suficiente! Sua defesa reduz o dano pela metade!");

            string? textoAtaqueDefendido =
                NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);

            if (!string.IsNullOrWhiteSpace(textoAtaqueDefendido))
                Print(textoAtaqueDefendido);

            int danoReduzido = inimigo.Ataque / 2;
            heroi.TomarDano(danoReduzido);
        }
        else
        {
            Console.WriteLine();
            Print("Você foi lento demais! Não conseguiu se defender a tempo.");

            string? textoAtaqueFalhaDefesa =
                NarrationRepository.GetRandomAttackText(inimigo.ClasseId, inimigo.Revelado);

            if (!string.IsNullOrWhiteSpace(textoAtaqueFalhaDefesa))
                Print(textoAtaqueFalhaDefesa);

            heroi.TomarDano(inimigo.Ataque);
        }

        inimigo.StaminaAtual -= inimigo.CustoAtaque;
        if (inimigo.StaminaAtual < 0)
            inimigo.StaminaAtual = 0;

        string narracaoTomarDanoHeroi =
            _heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.TomarDano);
        Print(narracaoTomarDanoHeroi);

        if (heroi.hp <= 0)
        {
            Console.WriteLine();
            Print("Mesmo se defendendo, o herói foi derrotado....");
            return true;
        }

        return false;
    }

    static void ExecutarTurnoAnalise(Hero heroi, Enemy inimigo, string heroClassKey)
{
    Console.WriteLine();
    Print("Você decide analisar a situação com mais cuidado...");

    while (true)
    {
        Print("O que você deseja analisar?");
        Print("1 - Seu herói");
        Print("2 - A criatura à sua frente");
        Print("Digite 1, 2 ou algo como 'analisar herói' ou 'analisar inimigo':");

        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Print("Não entendi bem. Tente novamente.");
            continue;
        }

        input = input.Trim().ToLowerInvariant();

        // Foca no herói
        if (input == "1" ||
            input.Contains("heroi") ||
            input.Contains("herói") ||
            input.Contains("meu") ||
            input.Contains("minha") ||
            input.Contains("eu"))
        {
            AnalisarHeroi(heroi, heroClassKey);
            return;
        }

        // Foca no inimigo
        if (input == "2" ||
            input.Contains("inim") ||
            input.Contains("criatura") ||
            input.Contains("monstro"))
        {
            AnalisarInimigo(heroi, inimigo);
            return;
        }

        Print("Não ficou claro se você quer analisar o herói ou o inimigo. Tente ser mais específico.");
    }
}

static void AnalisarInimigo(Hero heroi, Enemy inimigo)
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

static void AnalisarHeroi(Hero heroi, string heroClassKey)
{
    Console.WriteLine();
    Print("Você volta sua atenção para si mesmo, avaliando suas próprias capacidades...");

    Console.WriteLine($"Nome: {heroi.nome}");
    Console.WriteLine($"Classe: {heroi.classe}");
    Console.WriteLine($"HP atual: {heroi.hp}");
    Console.WriteLine($"Ataque base: {heroi.ataque}");
    Console.WriteLine($"Defesa base: {heroi.defesa}");
    Console.WriteLine();

    Print("Habilidades de classe conhecidas:");

    string classe = heroClassKey.ToLowerInvariant();
//MEDIDA  TEMPORARIA APENAS PARA TESTE, DEPOIS IREMOS COLOCA ISSO EM JSON SEU SAFADO
// VAI FICA COM LOGICA X DADOS SEPARADINHOS
// UI UI UI 
// PARA OS CURIOSOS DE PLANTAO ISSO E TUDO NARRATIVO AINDA NAO FOI IMPLEMENTADO
    switch (classe)
    {
        case "guerreiro":
            Print("- Golpe pesado: você concentra toda a força em um único ataque, causando muito dano, mas fica exausto e perde a próxima chance de atacar.");
            break;

        case "mago":
            Print("- Magia concentrada: você canaliza energia arcana para um feitiço mais poderoso, aumentando o dano, mas deixando sua defesa temporariamente comprometida.");
            break;

        case "druida":
            Print("- Chamado da natureza: você invoca forças naturais para apoiar ataque ou defesa, mas não pode usar essa habilidade em turnos consecutivos.");
            break;

        default:
            Print("Você ainda não identificou nenhuma habilidade especial clara da sua classe.");
            break;
    }

    Console.WriteLine();
    Print("Essas habilidades ainda são apenas potencial — você precisará desenvolvê-las para usá-las de fato em batalha.");
}


    static bool ExecutarTurnoFuga(Hero heroi, string heroClassKey)
    {
        Console.WriteLine();

        Print(_heroNarrations.GetRandomNarration(heroClassKey, HeroNarrationEvent.Fugir));

        Print("Você decide recuar e fugir da batalha...");

        return true;
    }
}
