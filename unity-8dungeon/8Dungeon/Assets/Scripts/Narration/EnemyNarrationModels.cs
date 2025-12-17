using System;
using System.Collections.Generic;

[Serializable]
public class EnemyNarrationConfig
{
    public EnemyNarrationStage defaultNoReveal = new EnemyNarrationStage();
    public EnemyNarrationStage defaultReveal = new EnemyNarrationStage();
    public List<EnemyNarrationSet> sets = new List<EnemyNarrationSet>();
}

[Serializable]
public class EnemyNarrationSet
{
    public EnemyArchetype archetype = EnemyArchetype.Default;
    public EnemyNarrationStage noReveal = new EnemyNarrationStage();
    public EnemyNarrationStage reveal = new EnemyNarrationStage();
}

[Serializable]
public class EnemyNarrationStage
{
    public List<string> appear = new List<string>();
    public List<string> reveal = new List<string>();
    public List<string> attack = new List<string>();
    public List<string> defend = new List<string>();
    public List<string> hurt = new List<string>();
    public List<string> death = new List<string>();

    public List<string> GetLines(EnemyNarrationEvent evt)
    {
        switch (evt)
        {
            case EnemyNarrationEvent.Appear: return appear;
            case EnemyNarrationEvent.Reveal: return reveal;
            case EnemyNarrationEvent.Attack: return attack;
            case EnemyNarrationEvent.Defend: return defend;
            case EnemyNarrationEvent.Hurt: return hurt;
            case EnemyNarrationEvent.Death: return death;
            default: return s_empty;
        }
    }

    private static readonly List<string> s_empty = new List<string>();
}
