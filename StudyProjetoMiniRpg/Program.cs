Console.WriteLine("8dugeon MiniRpg - Study");

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
        player = new Hero(HeroiNome, HeroiClasse, 150, 20, 40);
    }
    else if (HeroiClasse == "mago")
    {
        player = new Hero(HeroiNome, HeroiClasse, 100, 40, 20);
    }
    else if (HeroiClasse == "arqueiro")
    {
        player = new Hero(HeroiNome, HeroiClasse, 100, 50, 10);
    }
    else
    {
        player = new Hero(HeroiNome, HeroiClasse, 100, 15, 15);
    }
    return player;
}
Hero heroi = HeroiPlayer();
Console.WriteLine($"Nome: {heroi.nome}");
Console.WriteLine ($"Classe: {heroi.classe}");
Console.WriteLine ($"HP: {heroi.hp}");
Console.WriteLine ($"Ataque: {heroi.ataque}");
Console.WriteLine ($"Defesa: {heroi.defesa}");


