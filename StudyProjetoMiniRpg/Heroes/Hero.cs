using System;

public class Hero
{
    public string nome;
    public string classe;
    public int hp;
    public int ataque;
    public int defesa;

    public Hero(string nome, string classe, int hp, int ataque, int defesa)
    {
        this.nome = nome;
        this.classe = classe;
        this.hp = hp;
        this.ataque = ataque;
        this.defesa = defesa;
    }

    public static Hero CriarHeroi()
    {
        Console.WriteLine("Digite o nome do seu heroi: ");
        string? inputNome = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(inputNome))
        {
            Console.WriteLine("Vocêo não pode deixa o nome do seu heroi vazio! escreva de novo");
            Console.WriteLine();
            inputNome = Console.ReadLine();
        }
        string heroiNome = inputNome;

        Console.WriteLine("Digite a classe do seu heroi: (Guerreiro, Mago, Arqueiro ou Algo diferente...)");
        string? inputClasse = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(inputClasse))
        {
            Console.Write("Você e Burro? já falei que não pode deixa em branco! ");
            Console.WriteLine();
            inputClasse = Console.ReadLine();
        }

        string heroiClasse = inputClasse.Trim();
        if (!HeroClassConfig.Classes.TryGetValue(heroiClasse, out HeroClassStats? stats))
        {
            Console.WriteLine($"Classe '{heroiClasse}'... estranho...mas tudo bem, vou te dar algumas estatisticas.");
            Console.WriteLine();
            stats = HeroClassConfig.DefaultClassStats;
        }
        Hero player = new Hero(
            heroiNome,
            heroiClasse,
            stats.Hp,
            stats.Ataque,
            stats.Defesa
        );
        return player;
    }
    public void Atacar(Enemy inimigo)
    {
        Console.WriteLine ($"O {nome} Atacou");
        inimigo.TomarDano(ataque);
    }
    public void TomarDano(int dano)
    {
        double defesaDecimal = defesa / 100.0;
        double danoBruto = dano * (1 - defesaDecimal);
        int danoRecebido = (int)danoBruto;
        if (danoRecebido <= 0)
        {
            Console.WriteLine($"O herói {nome} não tomou dano devido á sua defesa.");
            return;
        }

        hp -= danoRecebido;

        if( hp > 0)
        {
            Console.WriteLine ($"O herói {nome} tomou {danoRecebido} de dano e agora tem {hp} de HP");
        }
        else
        {
            hp = 0;
            Console.WriteLine ($"Fim da jornada do heroi {nome} morreu em batalhar.");
        }
    }

}