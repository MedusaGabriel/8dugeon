using System.Collections.Generic;

public class Enemy
{
    public string Nome { get; private set; } = "";
    public string Descricao { get; private set; } = "";
    public int Hp { get; private set; }
    public int HpAtual { get; private set; }
    public int Ataque { get; private set; }
    public int Defesa { get; private set; }
    public int Alcance { get; private set; }
    public int ClasseId { get; private set; }    
    public bool Revelado { get; set; } = false;

    public Enemy(EnemyClassStats stats)
    {
        ClasseId = stats.ClasseId;
        Nome = stats.Nome;
        Descricao = stats.Descricao;
        Hp = stats.Hp;
        HpAtual = stats.Hp;
        Ataque = stats.Ataque;
        Defesa = stats.Defesa;
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

