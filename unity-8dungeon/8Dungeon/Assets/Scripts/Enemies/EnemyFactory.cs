using System.Linq;
using UnityEngine;

public static class EnemyFactory
{
    private static EnemyTypeData[] _cachedEnemies;

    private static void EnsureLoaded()
    {
        if (_cachedEnemies != null && _cachedEnemies.Length > 0)
            return;

        var all = Resources.LoadAll<EnemyTypeData>("Enemies");

        if (all == null || all.Length == 0)
        {
            Debug.LogError("EnemyFactory: Nenhum EnemyTypeData encontrado em Resources/Enemies.");
            _cachedEnemies = new EnemyTypeData[0];
            return;
        }

        // FILTRA: remove os que são archetype Default do sorteio
        _cachedEnemies = all
            .Where(e => e.archetype != EnemyArchetype.Default)
            .ToArray();

        Debug.Log($"[EnemyFactory] Encontrados {all.Length} inimigos, filtrados {_cachedEnemies.Length} (sem Default).");

        if (_cachedEnemies.Length == 0)
        {
            Debug.LogWarning("[EnemyFactory] Só foram encontrados inimigos Default. Sorteio usará fallback Default.");
        }
    }

    public static EnemyStats CreateRandomEnemy()
    {
        EnsureLoaded();

        if (_cachedEnemies == null || _cachedEnemies.Length == 0)
        {
            // fallback de segurança -> só aqui entra o Default
            Debug.LogWarning("[EnemyFactory] Nenhum inimigo válido encontrado, usando Default.");
            return new EnemyStats(EnemyArchetype.Default);
        }

        var data = _cachedEnemies[Random.Range(0, _cachedEnemies.Length)];

        Debug.Log(
            $"[EnemyFactory] Sorteado inimigo: nome='{data.enemyName}', archetype={data.archetype}, HP={data.maxHp}, ATK={data.attack}, DEF={data.defense}, Dodge={data.dodgeChance}, Counter={data.counterChance}"
        );

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
