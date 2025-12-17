using UnityEngine;

[CreateAssetMenu(fileName = "HeroClassData", menuName = "8Dungeon/Hero Class")]
public class HeroClassData : ScriptableObject
{
    public HeroClass heroClass;

    [Header("Atributos Base")]
    public int maxHp;
    public int attack;
    public int defense;

    [Header("Descrição")]
    [TextArea]
    public string classDescription;
}
