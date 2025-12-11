using System.Linq;
using UnityEngine;

public static class EnemyFactory
{
    private static EnemyTypeData[] _cachedEnemies;

    private static void EnsureLoaded()
    {
        if (_cachedEnemies == null || _cachedEnemies.Length == 0)
        {
            _cachedEnemies = Resources.LoadAll<EnemyTypeData>("Enemies");

            if (_cachedEnemies == null || _cachedEnemies.Length == 0)
            {
                Debug.LogError("EnemyFactory: Nenhum EnemyTypeData encontrado em Resources/Enemies.");
            }
        }
    }

    public static EnemyStats CreateRandomEnemy()
    {
        EnsureLoaded();

        if (_cachedEnemies == null || _cachedEnemies.Length == 0)
        {
            // fallback de segurança
            return new EnemyStats(EnemyArchetype.Default);
        }

        var data = _cachedEnemies[Random.Range(0, _cachedEnemies.Length)];

        return new EnemyStats(
            data.archetype,
            data.enemyName,
            data.maxHp,
            data.attack,
            data.defense,
            data.dodgeChance,
            data.counterChance
        );
    }
}
