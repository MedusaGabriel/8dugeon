using System.Linq;
using UnityEngine;

public static class HeroFactory
{
    private static HeroClassData[] _cachedClasses;

    private static void EnsureLoaded()
    {
        if (_cachedClasses == null || _cachedClasses.Length == 0)
        {
            _cachedClasses = Resources.LoadAll<HeroClassData>("HeroClasses");
            if (_cachedClasses == null || _cachedClasses.Length == 0)
            {
                Debug.LogError("HeroFactory: Nenhum HeroClassData foi encontrado em Resources/HeroClasses.");
            }
        }
    }

    public static HeroStats CreateHero(string name, HeroClass heroClass)
    {
        EnsureLoaded();

        HeroClassData data = null;

        if (_cachedClasses != null && _cachedClasses.Length > 0)
        {
            data = _cachedClasses.FirstOrDefault(c => c.heroClass == heroClass);

            if (data == null)
            {
                data = _cachedClasses.FirstOrDefault(c => c.heroClass == HeroClass.Default);
            }
        }

        if (data == null)
        {
            Debug.LogWarning("HeroFactory: Usando valores padrão hardcoded porque nenhum HeroClassData foi encontrado.");
            data = ScriptableObject.CreateInstance<HeroClassData>();
            data.heroClass = heroClass;
            data.maxHp = 100;
            data.attack = 10;
            data.defense = 5;
            data.classDescription = "Heroi versatil sem vantagens claras.";
        }
        

        var hero = new HeroStats
        {
            Name = name,
            Class = data.heroClass,
            MaxHp = data.maxHp,
            CurrentHp = data.maxHp,
            Attack = data.attack,
            Defense = data.defense,
            ClassDescription = data.classDescription
        };

        return hero;
    }
}
