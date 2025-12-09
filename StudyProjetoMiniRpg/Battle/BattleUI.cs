using System;
using System.Threading;

public static class BattleUI
{
    public static void Print(string msg, int delay = 1000)
    {
        Console.WriteLine(msg);
        Thread.Sleep(delay);
    }

    public static void ExibirAparicaoInimigo(Enemy inimigo)
    {
        string? textoAparicao = NarrationRepository.GetRandomAppearText(inimigo.ClasseId, inimigo.Revelado);

        if (!string.IsNullOrWhiteSpace(textoAparicao))
            Print(textoAparicao);
        else
            Print("Uma presença desconhecida surge à sua frente...");

        Console.WriteLine();
    }

    public static void ExibirStatusInicioTurno(Hero heroi, Enemy inimigo)
    {
        Console.WriteLine();
        Console.WriteLine($"Sua vida atual é: {heroi.hp}");

        if (!inimigo.Revelado)
            Print("A criatura desconhecida lhe viu e está se preparando para lhe atacar.");
        else
            Print($"{inimigo.Nome} encara você, pronto para atacar novamente.");

        Console.WriteLine();
    }

    public static void ExibirInformacoesInimigo(Enemy inimigo, bool reveladoAgora = false)
    {
        Console.WriteLine();

        if (reveladoAgora)
        {
            Print("Um olho intertemporal surge em sua mente, revelando completamente a criatura à sua frente!");
            Thread.Sleep(1000);
            Print("O tempo desacelera por um momento enquanto você observa atentamente o inimigo...");
            Thread.Sleep(1000);
        }

        Print("Agora você entende melhor o inimigo:");
        Console.WriteLine($"Nome: {inimigo.Nome}");
        Console.WriteLine($"HP: {inimigo.Hp}");
        Console.WriteLine($"Descrição: {inimigo.Descricao}");
        Console.WriteLine($"Ataque: {inimigo.Ataque}");
        Console.WriteLine($"Defesa: {inimigo.Defesa}");
        Console.WriteLine($"Alcance: {inimigo.Alcance}");
        Console.WriteLine();

        if (reveladoAgora)
        {
            string? textoAparicaoRevel = NarrationRepository.GetRandomAppearText(inimigo.ClasseId, true);
            if (!string.IsNullOrWhiteSpace(textoAparicaoRevel))
                Print(textoAparicaoRevel);
        }
    }

    public static void ExibirInformacoesHeroi(Hero heroi, string heroClassKey)
    {
        Console.WriteLine();
        Print("Você volta sua atenção para si mesmo, avaliando suas próprias capacidades...");

        Console.WriteLine($"Nome: {heroi.nome}");
        Console.WriteLine($"Classe: {heroi.classe}");
        Console.WriteLine($"HP atual: {heroi.hp}");
        Console.WriteLine($"Ataque base: {heroi.ataque}");
        Console.WriteLine($"Defesa base: {heroi.defesa}");
        Console.WriteLine($"Alcance: {heroi.alcance}");
        Console.WriteLine();

        Print("Habilidades de classe conhecidas:");

        string classe = heroClassKey.ToLowerInvariant();

        //LEMBRANDO QUE ISSO VAI SAIR PARA IR PAR AUMA PASTA .JSON
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

    public static void ExibirMensagemAtaqueImpossivel()
    {
        Print("Você tenta atacar, mas o inimigo ainda está longe demais para o seu golpe alcançar.");
    }

    public static void ExibirMensagemHeroiMorreu()
    {
        Print("O herói foi derrotado!");
    }

    public static void ExibirMensagemInimigoMorreu(Enemy inimigo)
    {
        string? textoMorte = NarrationRepository.GetRandomDeathText(inimigo.ClasseId);
        if (!string.IsNullOrWhiteSpace(textoMorte))
            Print(textoMorte);
        else
            Print("O inimigo desconhecido foi derrotado antes que pudesse atacar!");
    }
}
