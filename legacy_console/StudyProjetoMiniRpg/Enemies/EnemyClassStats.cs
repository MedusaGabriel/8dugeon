public class EnemyClassStats
{
    public string Nome {get; set;} = "";
    public string Descricao { get; set; } = "";
    public int Hp {get; set;}
    public int Ataque {get; set;}
    public int Defesa {get; set;}
    public int Alcance {get; set;}
    public int ClasseId {get; set;}

    public int StaminaMax { get; set; } = 3;
    public int StaminaRecuperacao { get; set; } = 1;

}