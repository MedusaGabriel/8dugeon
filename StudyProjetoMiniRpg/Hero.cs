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