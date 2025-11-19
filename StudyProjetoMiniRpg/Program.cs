Console.WriteLine("8dugeon MiniRpg - Study");

// Hero heroi = new Hero("Medusa", "Mago", 100, 30, 10);

// Console.WriteLine($"Nome: {heroi.nome}");
// Console.WriteLine ($"Classe: {heroi.classe}");
// Console.WriteLine ($"HP: {heroi.hp}");
// Console.WriteLine ($"Ataque: {heroi.ataque}");
// Console.WriteLine ($"Defesa: {heroi.defesa}");
// heroi.Atacar();
// heroi.TomarDano(50);

static Hero HeroiPlayer()
{
    Console.WriteLine("Digite o nome do seu heroi:");
    string HeroiNome = Console.ReadLine();
    Console.WriteLine("Digite a classe do seu heroi:");
    string HeroiClasse = Console.ReadLine();
    Hero player;

    if (HeroiClasse == "Guerreiro")
    {
        player = new Hero(HeroiNome, HeroiClasse, 150, 20, 40);
    }
    else if (HeroiClasse == "Mago")
    {
        player = new Hero(HeroiNome, HeroiClasse, 100, 40, 20);
    }
    else if (HeroiClasse == "Arqueiro")
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


