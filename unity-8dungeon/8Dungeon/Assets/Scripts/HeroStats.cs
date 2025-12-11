public enum HeroClass
{
    Mago,
    Guerreiro,
    Default
}

public class HeroStats
{
    public string Name;
    public HeroClass Class;

    public int MaxHp;
    public int CurrentHp;
    public int Attack;
    public int Defense;
}
