using System;

public class InspectionSystem
{
    public void AnalisarInimigo(Enemy inimigo)
    {
        Console.WriteLine();

        if (!inimigo.Revelado)
        {
            inimigo.Revelar();
            BattleUI.ExibirInformacoesInimigo(inimigo, reveladoAgora: true);
        }
        else
        {
            BattleUI.Print("Você já analisou esse inimigo. Nada novo é revelado.");
        }
    }

    public void AnalisarHeroi(Hero heroi, string heroClassKey)
    {
        BattleUI.ExibirInformacoesHeroi(heroi, heroClassKey);
    }

    public void PermitirEscolhaAnalise(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        Console.WriteLine();
        BattleUI.Print("Você decide analisar a situação com mais cuidado...");

        while (true)
        {
            BattleUI.Print("O que você deseja analisar?");
            BattleUI.Print("1 - Seu herói");
            BattleUI.Print("2 - A criatura à sua frente");
            BattleUI.Print("Digite 1, 2 ou algo como 'analisar herói' ou 'analisar inimigo':");

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                BattleUI.Print("Não entendi bem. Tente novamente.");
                continue;
            }

            input = input.Trim().ToLowerInvariant();

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

            if (input == "2" ||
                input.Contains("inim") ||
                input.Contains("criatura") ||
                input.Contains("monstro"))
            {
                AnalisarInimigo(inimigo);
                return;
            }

            BattleUI.Print("Não ficou claro se você quer analisar o herói ou o inimigo. Tente ser mais específico.");
        }
    }
}
