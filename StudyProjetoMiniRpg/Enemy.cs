public class Enemy
{
    public string ClasseId {get;}
    public string Nome {get;}

    public int Hp {get; private set;}
    public int Ataque {get;}
    public int Defesa {get;}
    public int Alcance {get;}
    public string Descricao {get;}
    public bool Revelado {get; set;} = false;
    


    public Enemy(EnemyClassStats stats)
    {
        ClasseId = stats.ClasseId.ToString();
        Nome = stats.Nome;
        Hp = stats.Hp;
        Ataque = stats.Ataque;
        Defesa = stats.Defesa;
        Descricao = stats.Descricao.ToString();
        Alcance = stats.Alcance;
    }
    public void Revelar()
    {
        Revelado = true;
    }

    public void Atacar(Hero heroi)
    {
        Console.WriteLine($"O {Nome} Atacou o herói {heroi.nome}");
        heroi.TomarDano(Ataque);
    }

    public void TomarDano(int dano)
    {
        double defesaDecimal = Defesa / 100.0;
        double danoBruto = dano * (1 - defesaDecimal);
        int danoRecebido = (int)danoBruto;

        if (danoRecebido <= 0)
        {
            Console.WriteLine($"{Nome} não tomou dano devido à sua defesa.");
            return;
        }

        Hp -= danoRecebido;

        if (Hp > 0)
            if (Revelado)
            {
                Console.WriteLine($"{Nome} tomou {danoRecebido} de dano e agora tem {Hp} de HP.");
            }
            else
        {
            Console.WriteLine($"{Nome} tomou {danoRecebido} de dano.");
        }
        else
        {
            Hp = 0;
            Console.WriteLine($"{Nome} tomou {danoRecebido} de dano e MORREU!");
        }
    }

    public bool EstaMorto => Hp <= 0;


}





// public class Enemy
// {
//     public string nome;
//     public int hp;
//     public int ataque;
//     public int defesa;

//     public Enemy(string nome, int hp, int ataque, int defesa)
//     {
//         this.nome = nome;
//         this.hp = hp;
//         this.ataque = ataque;
//         this.defesa = defesa;
//     }

//     public void Atacar(Hero heroi)
//     {
//         Console.WriteLine($"O {nome} Atacou o herói {heroi.nome}");
//         heroi.TomarDano(ataque);
//     }
//     public void TomarDano(int dano)
//     {
//         double defesaDecimal = defesa/100.0;
//         double danoBruto = dano *(1 - defesaDecimal);
//         int danoRecebido = (int)danoBruto;
//         if (danoRecebido <= 0)
//         {
//             Console.WriteLine ($"{nome} não tomou dano devido á sua defesa.");
//             return;
//         }
//         hp -=danoRecebido;
//         if (hp > 0)
//         {
//             Console.WriteLine($"{nome} tomou {danoRecebido} de dano agora tem {hp} de Hp.");
//         }
//         else
//         {
//             hp = 0;
//             Console.WriteLine($"{nome} tomou {danoRecebido} de dano e MORREU!");
//         }
//     }
// }