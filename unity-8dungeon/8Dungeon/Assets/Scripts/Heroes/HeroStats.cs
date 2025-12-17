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
    public string ClassKey;
    public string ClassDescription;

    public int MaxHp;
    public int CurrentHp;
    public int Attack;
    public int Defense;
}
