public class Enemy
{
    public string nome;
    public int hp;
    public int ataque;
    public int defesa;

    public Enemy(string nome, int hp, int ataque, int defesa)
    {
        this.nome = nome;
        this.hp = hp;
        this.ataque = ataque;
        this.defesa = defesa;
    }

    public void Atacar(Hero heroi)
    {
        Console.WriteLine($"O {nome} Atacou o herói {heroi.nome}");
        heroi.TomarDano(ataque);
    }
    public void TomarDano(int dano)
    {
        double defesaDecimal = defesa/100.0;
        double danoBruto = dano *(1 - defesaDecimal);
        int danoRecebido = (int)danoBruto;
        if (danoRecebido <= 0)
        {
            Console.WriteLine ($"{nome} não tomou dano devido á sua defesa.");
            return;
        }
        hp -=danoRecebido;
        if (hp > 0)
        {
            Console.WriteLine($"{nome} tomou {danoRecebido} de dano agora tem {hp} de Hp.");
        }
        else
        {
            hp = 0;
            Console.WriteLine($"{nome} tomou {danoRecebido} de dano e MORREU!");
        }
    }
}