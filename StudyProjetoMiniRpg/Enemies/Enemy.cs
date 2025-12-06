using System.Collections.Generic;

public class Enemy
{
    public int ClasseId { get; set; }
    public string Nome { get; }

    public int Hp { get; private set; }
    public int Ataque { get; }
    public int Defesa { get; }
    public int Alcance { get; }
    public string Descricao { get; }
    public bool Revelado { get; set; } = false;

    public Enemy(EnemyClassStats stats)
    {
        ClasseId = stats.ClasseId;
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
        heroi.TomarDano(Ataque);
    }

    public void TomarDano(int dano)
    {
        double defesaDecimal = Defesa / 100.0;
        double danoBruto = dano * (1 - defesaDecimal);
        int danoRecebido = (int)danoBruto;

        if (danoRecebido <= 0)
        {
            return;
        }

        if (danoRecebido <= 0)
        {
            return;
        }

        Hp -= danoRecebido;

        if (Hp < 0)
        {
            Hp = 0;
        }
    }

    public bool EstaMorto => Hp <= 0;

}

