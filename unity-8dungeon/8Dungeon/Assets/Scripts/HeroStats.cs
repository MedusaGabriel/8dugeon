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

    public void ApplyClass(HeroClass heroClass)
    {
        Class = heroClass;

        switch (heroClass)
        {
            case HeroClass.Mago:
                MaxHp = 100;
                Attack = 20;
                Defense = 3;
                break;

            case HeroClass.Guerreiro:
                MaxHp = 150;
                Attack = 15;
                Defense = 10;
                break;

            default:
                MaxHp = 120;
                Attack = 12;
                Defense = 5;
                break;
        }

        CurrentHp = MaxHp;
    }
}
