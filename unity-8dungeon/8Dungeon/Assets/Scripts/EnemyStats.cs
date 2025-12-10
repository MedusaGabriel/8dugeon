public enum EnemyType
{
    Slime,
    Esqueleto
}

public class EnemyStats
{
    public string Name;
    public EnemyType Type;

    public int MaxHp;
    public int CurrentHp;
    public int Attack;
    public int Defense;

    public EnemyStats(EnemyType type)
    {
        ApplyType(type);
    }

    public void ApplyType(EnemyType type)
    {
        Type = type;

        switch (type)
        {
            case EnemyType.Slime:
                Name = "Slime";
                MaxHp = 50;
                Attack = 10;
                Defense = 2;
                break;

            case EnemyType.Esqueleto:
                Name = "Esqueleto";
                MaxHp = 60;
                Attack = 12;
                Defense = 4;
                break;
        }

        CurrentHp = MaxHp;
    }
}
