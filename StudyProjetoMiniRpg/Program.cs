
static Hero HeroiPlayer()
{
    Console.WriteLine("Digite o nome do seu heroi:");
    string? inputNome = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(inputNome))
    {
        Console.WriteLine("Você não pode deixa seu nome em branco!. Digite novamente: ");
        inputNome = Console.ReadLine();
    }
    string HeroiNome = inputNome;
    Console.WriteLine("Digite a classe do seu heroi:");
    string? inputClasse = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(inputClasse))
    {
        Console.Write("Você não pode deixa em branco! Digite novamente: ");
        inputClasse = Console.ReadLine();
    }
    string HeroiClasse = inputClasse.ToLower();

    Hero player;

    if (HeroiClasse == "guerreiro")
    {
        player = new Hero(HeroiNome, HeroiClasse, 150, 15, 10);
    }
    else if (HeroiClasse == "mago")
    {
        player = new Hero(HeroiNome, HeroiClasse, 100, 20, 3);
    }
    else if (HeroiClasse == "arqueiro")
    {
        player = new Hero(HeroiNome, HeroiClasse, 100, 22, 4);
    }
    else
    {
        player = new Hero(HeroiNome, HeroiClasse, 50, 15, 5);
    }
    return player;
}

Enemy slime = new Enemy ("Slime", 50, 10, 2);

static void Batalha(Hero heroi, Enemy inimigo)
{
    while (heroi.hp > 0 && inimigo.hp > 0)
    {
        inimigo.Atacar(heroi);
        if (heroi.hp <= 0)
        {
            Console.WriteLine("Heroi foi derrotado!");
            break;
        }
        Console.WriteLine();
        Console.WriteLine("Aperte ENTER para o herói atacar...");
        Console.ReadLine();

        heroi.Atacar(inimigo);
        if(inimigo.hp <=0)
        {
            Console.WriteLine("O Inimigo foi derrotado!");
            break;
        }
        Console.WriteLine("Próximo turno.. Aperte ENTER pra continuar!");
        Console.ReadLine();
    }
}


Hero heroi = HeroiPlayer();
Console.WriteLine($"Nome do Heroi: {heroi.nome} Classe do Heroi: {heroi.classe} Seu HP: {heroi.hp} Seu Ataque: {heroi.ataque} Sua Defesa: {heroi.defesa}");
Console.WriteLine($"Um Inimigo: {slime.nome}  Hp: {slime.hp}  Ataque: {slime.ataque} Defesa: {slime.defesa}");
Batalha(heroi, slime);




