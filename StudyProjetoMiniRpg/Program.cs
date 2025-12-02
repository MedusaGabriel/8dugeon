using System;


Enemy Slime = CriarInimigo();
Hero Heroi = Hero.CriarHeroi();

VerStatus(Heroi, Slime);
Batalha(Heroi, Slime);

// --------- Funções ---------

// Função para criar o herói


// Mostra Status 
static void VerStatus(Hero heroi, Enemy inimigo)
{
    Console.WriteLine();
    Console.WriteLine("Status do seu Heroi!");
    Console.WriteLine($"Nome do Heroi: {heroi.nome} | Classe do Heroi: {heroi.classe} | HP: {heroi.hp} | Ataque: {heroi.ataque} | Defesa: {heroi.defesa}");

    Console.WriteLine();
    Console.WriteLine("Status do seu Inimigo!");
    Console.WriteLine($"Inimigo: {inimigo.nome} | HP: {inimigo.hp} | Ataque: {inimigo.ataque} | Defesa: {inimigo.defesa}");
}

// Criar Inimigo

static Enemy CriarInimigo()
{
    return new Enemy("Slime", 50, 10, 2);
}

// Lê ação do jogador 
// Futuramente vai ter um novo arquivo so pra essas funções de ler ação
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

        Console.WriteLine("Não entendi sua intenção. Tente algo como: 'quero atacar', 'vou defender', 'quero fugir'.");
    }
}

// verifica se o texto contém qualquer uma das palavras-chaves
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
    Console.WriteLine($"Um {inimigo.nome} apareceu na sua frente!");
    Console.WriteLine();

    Random rng = new Random();
    while (heroi.hp > 0 && inimigo.hp > 0)
    {
        Console.WriteLine();
        Console.WriteLine($"Hp do Herói: {heroi.hp} | Hp do Inimigo: {inimigo.hp}");
        Console.WriteLine();
        Console.WriteLine($"{inimigo.nome} lhe viu e esta se preparando para lhe atacar, Slime possui tentanculos pegajoso que causa dano em contato!");
        Console.WriteLine();

        AcaoJogador acao = LerAcaoJogador();

        switch (acao)
        {
            case AcaoJogador.Atacar:
                Console.WriteLine();
                Console.WriteLine("Você tenta ser mais rápido e parte para o ataque!");
                Console.WriteLine();

                heroi.Atacar(inimigo);

                if (inimigo.hp <= 0)
                {
                    Console.WriteLine($"O inimigo {inimigo.nome} antes que ele atacasse!");
                    return;
                }

                Console.WriteLine($"{inimigo.nome} ainda consegue desferir um golpe!");
                Console.WriteLine();
                heroi.TomarDano(inimigo.ataque);

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
                    int danoReduzido = inimigo.ataque /2;
                    heroi.TomarDano(danoReduzido);
                }
                else
                {
                    Console.WriteLine("Você foi lento demais! Não conseguiu se defender a tempo.");
                    heroi.TomarDano(inimigo.ataque);
                }
            

                if(heroi.hp <= 0)
                {
                    Console.WriteLine("Mesmo se defendendo, o herói foi derrotado....");
                    return;
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
    Fugir
}


//Antigo código do RPG em C#
// static Hero HeroiPlayer()
// {
//     Console.WriteLine("Digite o nome do seu heroi:");
//     string? inputNome = Console.ReadLine();
//     while (string.IsNullOrWhiteSpace(inputNome))
//     {
//         Console.WriteLine("Você não pode deixa seu nome em branco!. Digite novamente: ");
//         inputNome = Console.ReadLine();
//     }
//     string HeroiNome = inputNome;
//     Console.WriteLine("Digite a classe do seu heroi:");
//     string? inputClasse = Console.ReadLine();
//     while (string.IsNullOrWhiteSpace(inputClasse))
//     {
//         Console.Write("Você não pode deixa em branco! Digite novamente: ");
//         inputClasse = Console.ReadLine();
//     }
//     string HeroiClasse = inputClasse.ToLower();

//     Hero player;

//     if (HeroiClasse == "guerreiro")
//     {
//         player = new Hero(HeroiNome, HeroiClasse, 150, 15, 10);
//     }
//     else if (HeroiClasse == "mago")
//     {
//         player = new Hero(HeroiNome, HeroiClasse, 100, 20, 3);
//     }
//     else if (HeroiClasse == "arqueiro")
//     {
//         player = new Hero(HeroiNome, HeroiClasse, 100, 22, 4);
//     }
//     else
//     {
//         player = new Hero(HeroiNome, HeroiClasse, 50, 15, 5);
//     }
//     return player;
// }

// Enemy slime = new Enemy ("Slime", 50, 10, 2);

// static void Batalha(Hero heroi, Enemy inimigo)
// {
//     while (heroi.hp > 0 && inimigo.hp > 0)
//     {
//         inimigo.Atacar(heroi);
//         if (heroi.hp <= 0)
//         {
//             Console.WriteLine("Heroi foi derrotado!");
//             break;
//         }
//         Console.WriteLine();
//         Console.WriteLine("Aperte ENTER para o herói atacar...");
//         Console.ReadLine();

//         heroi.Atacar(inimigo);
//         if(inimigo.hp <=0)
//         {
//             Console.WriteLine("O Inimigo foi derrotado!");
//             break;
//         }
//         Console.WriteLine("Próximo turno.. Aperte ENTER pra continuar!");
//         Console.ReadLine();
//     }
// }


// Hero heroi = HeroiPlayer();
// Console.WriteLine($"Nome do Heroi: {heroi.nome} Classe do Heroi: {heroi.classe} Seu HP: {heroi.hp} Seu Ataque: {heroi.ataque} Sua Defesa: {heroi.defesa}");
// Console.WriteLine($"Um Inimigo: {slime.nome}  Hp: {slime.hp}  Ataque: {slime.ataque} Defesa: {slime.defesa}");
// Batalha(heroi, slime);




