using UnityEngine;

public enum EnemyArchetype
{
    Slime,
    Esqueleto,
    Default
}

[CreateAssetMenu(fileName = "EnemyTypeData", menuName = "8Dungeon/Enemy Type")]
public class EnemyTypeData : ScriptableObject
{
    public EnemyArchetype archetype;

    [Header("Atributos Base")]
    public string enemyName;
    public int maxHp;
    public int attack;
    public int defense;

    [Header("Chances")]
    [Range(0f, 1f)] public float dodgeChance;
    [Range(0f, 1f)] public float counterChance;
}
