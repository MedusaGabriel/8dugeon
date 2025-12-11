public class EnemyStats
{
    public EnemyArchetype Archetype;
    public string Name;

    public int MaxHp;
    public int CurrentHp;

    public int Attack;
    public int Defense;

    public float DodgeChance;
    public float CounterChance;

    public EnemyStats(
        EnemyArchetype archetype,
        string name,
        int maxHp,
        int attack,
        int defense,
        float dodgeChance,
        float counterChance
    )
    {
        Archetype = archetype;
        Name = name;

        MaxHp = maxHp;
        CurrentHp = maxHp;

        Attack = attack;
        Defense = defense;

        DodgeChance = dodgeChance;
        CounterChance = counterChance;
    }

    // Construtor fallback para compatibilidade
    public EnemyStats(EnemyArchetype archetype)
    {
        Archetype = archetype;
        Name = "Criatura Desconhecida";
        MaxHp = 60;
        CurrentHp = 60;
        Attack = 10;
        Defense = 3;
        DodgeChance = 0.05f;
        CounterChance = 0.10f;
    }
}
