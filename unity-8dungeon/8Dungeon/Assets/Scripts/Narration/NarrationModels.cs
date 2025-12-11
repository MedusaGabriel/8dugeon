using System.Collections.Generic;

[System.Serializable]
public class NarrationEntry
{
    public string key;
    public List<string> templates;
}

[System.Serializable]
public class NarrationDatabaseData
{
    public List<NarrationEntry> entries;
}

public struct NarrationEvent
{
    public NarrationKey Key;
    public HeroStats Hero;
    public EnemyStats Enemy;
    public int Damage;

    public NarrationEvent(NarrationKey key, HeroStats hero, EnemyStats enemy, int damage = 0)
    {
        Key = key;
        Hero = hero;
        Enemy = enemy;
        Damage = damage;
    }
}
