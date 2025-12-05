using System;

Enemy Inimigo = EnemyFactory.CriarInimigoAleatorio();
Hero Heroi = Hero.CriarHeroi();

Batalha(Heroi, Inimigo);

// Lê ação do jogador 
// Futuramente vai ter um novo arquivo so pra essas funções de ler ação
//Por enquanto vai fica aqui em program.cs
static AcaoJogador LerAcaoJogador()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("O que você quer fazer? (atacar, defender ou fugir)");
        Console.Write("> ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine($"O seu personagem ,não entedeu sua ação, escreva algo que ele entenda");
            continue;
        }

        string texto = input.ToLower();

        if (ContemQualquer(texto, new[] { "atac", "bater", "golpe" }))
        {
            return AcaoJogador.Atacar;
        }

        // Defender: 'defend', 'defesa', 'proteg'
        if (ContemQualquer(texto, new[] { "defend", "defesa", "proteg" }))
        {
            return AcaoJogador.Defender;
        }

        // Fugir: 'fug', 'correr', 'sair', 'escapar'
        if (ContemQualquer(texto, new[] { "fug", "correr", "sair", "escapar" }))
        {
            return AcaoJogador.Fugir;
        }
        if (ContemQualquer(texto, new[] { "analis", "examinar", "observar", "info", "informação" }))
        {
            return AcaoJogador.Analisar;
        }


        Console.WriteLine("Não entendi sua intenção. Tente algo como: 'quero atacar', 'vou defender', 'quero fugir'.");
    }
}

static bool ContemQualquer(string texto, string[] chaves)
{
    foreach (string chave in chaves)
    {
        if (texto.Contains(chave))
        {
            return true;
        }
    }
    return false;
}


// Batalha 
static void Batalha (Hero heroi, Enemy inimigo)
{
    Console.WriteLine();
    Console.WriteLine($"Um {inimigo.Nome} apareceu na sua frente!");
    Console.WriteLine();

    Random rng = new Random();

    while (heroi.hp > 0 && inimigo.Hp > 0)
    {
        Console.WriteLine();
        Console.WriteLine($"A sua vida é atual e: {heroi.hp}");
        Console.WriteLine();
        Console.WriteLine($"Descrição: {inimigo.Descricao}");
        Console.WriteLine();
        Console.WriteLine($"{inimigo.Nome} lhe viu e esta se preparando para lhe atacar");
        Console.WriteLine();

        AcaoJogador acao = LerAcaoJogador();

        switch (acao)
        {
            case AcaoJogador.Atacar:
                Console.WriteLine();
                Console.WriteLine("Você tenta ser mais rápido e parte para o ataque!");
                Console.WriteLine();

                heroi.Atacar(inimigo);

                if (inimigo.Hp <= 0)
                {
                    Console.WriteLine($"O inimigo {inimigo.Nome} antes que ele atacasse!");
                    return;
                }

                Console.WriteLine($"{inimigo.Nome} ainda consegue desferir um golpe!");
                Console.WriteLine();
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
                    Console.WriteLine("Você foi rápido o suficiente! Sua defesa Reduz o dano pela metade!");
                    int danoReduzido = inimigo.Ataque /2;
                    heroi.TomarDano(danoReduzido);
                }
                else
                {
                    Console.WriteLine("Você foi lento demais! Não conseguiu se defender a tempo.");
                    heroi.TomarDano(inimigo.Ataque);
                }
            

                if(heroi.hp <= 0)
                {
                    Console.WriteLine("Mesmo se defendendo, o herói foi derrotado....");
                    return;
                }
                break;
                
                case AcaoJogador.Analisar:
                Console.WriteLine();
                if (!inimigo.Revelado)
                {
                    Console.WriteLine($"Um olho inter-temporal apareceu em sua mente revelando completamente o inimigo a sua frente! ");
                    Console.WriteLine();
                    Console.WriteLine($"O tempo desacelera por um momento enquanto você observa atentamente o inimigo...");
                    inimigo.Revelar();

                    Console.WriteLine($"Agora você entende melhor o inimigo: ");
                    Console.WriteLine($"Nome: {inimigo.Nome}");
                    Console.WriteLine($"HP: {inimigo.Hp}");
                    Console.WriteLine($"Ataque: {inimigo.Ataque}");
                    Console.WriteLine($"Defesa: {inimigo.Defesa}");
                    Console.WriteLine($"Alcance: {inimigo.Alcance}");

                }
                break;
                case AcaoJogador.Fugir:
                Console.WriteLine();
                Console.WriteLine("Você decide recuar e fugir da batalha...");
                return;
        }
    }
}


// ---- Ações do Jogador ----
enum AcaoJogador{
    Atacar,
    Defender,
    Fugir,
    Analisar
}
