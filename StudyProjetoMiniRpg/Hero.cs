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

    public void Atacar()
    {
        Console.WriteLine ($"O {nome} Atacou");

    }
    public void TomarDano(int dano)
    {
        int DanoRecebido = dano - defesa;
        if  (DanoRecebido > 0)
        {
            hp -= DanoRecebido;
            Console.WriteLine ($"O Heroi {nome} tomou {DanoRecebido} de dano e agora tem {hp} de HP.");
        }
        else
        {
            Console.WriteLine ($"O Heroi {nome} não tomou dano devido à sua defesa.");
        }
        
    }

}