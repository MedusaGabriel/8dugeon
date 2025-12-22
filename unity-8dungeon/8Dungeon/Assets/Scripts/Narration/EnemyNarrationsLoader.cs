using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyNarrationsLoader
{
    private const string ResourcePath = "Data/enemy_narrations";

    public static EnemyNarrationConfig Load()
    {
        TextAsset text = Resources.Load<TextAsset>(ResourcePath);
        if (text == null)
        {
            Debug.LogWarning("[EnemyNarrationsLoader] Não encontrou Data/enemy_narrations em Resources.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(text.text))
        {
            Debug.LogWarning("[EnemyNarrationsLoader] enemy_narrations.json está vazio.");
            return null;
        }

        EnemyNarrationConfigSerialized raw = JsonUtility.FromJson<EnemyNarrationConfigSerialized>(text.text);
        if (raw == null)
        {
            Debug.LogWarning("[EnemyNarrationsLoader] Falha ao converter JSON para EnemyNarrationConfig.");
            return null;
        }

        return Convert(raw);
    }

    private static EnemyNarrationConfig Convert(EnemyNarrationConfigSerialized raw)
    {
        var config = new EnemyNarrationConfig
        {
            defaultNoReveal = raw.defaultNoReveal ?? new EnemyNarrationStage(),
            defaultReveal = raw.defaultReveal ?? new EnemyNarrationStage(),
            sets = new List<EnemyNarrationSet>()
        };

        if (raw.sets != null)
        {
            foreach (EnemyNarrationSetSerialized set in raw.sets)
            {
                if (set == null)
                {
                    continue;
                }

                EnemyArchetype archetype;
                if (!Enum.TryParse(set.archetype, true, out archetype))
                {
                    Debug.LogWarning($"[EnemyNarrationsLoader] Archetype inválido '{set.archetype}' no JSON. Ignorando set.");
                    continue;
                }

                var converted = new EnemyNarrationSet
                {
                    archetype = archetype,
                    noReveal = set.noReveal ?? new EnemyNarrationStage(),
                    reveal = set.reveal ?? new EnemyNarrationStage()
                };

                config.sets.Add(converted);
            }
        }

        return config;
    }

    [Serializable]
    private class EnemyNarrationConfigSerialized
    {
        public EnemyNarrationStage defaultNoReveal = new EnemyNarrationStage();
        public EnemyNarrationStage defaultReveal = new EnemyNarrationStage();
        public List<EnemyNarrationSetSerialized> sets = new List<EnemyNarrationSetSerialized>();
    }

    [Serializable]
    private class EnemyNarrationSetSerialized
    {
        public string archetype = EnemyArchetype.Default.ToString();
        public EnemyNarrationStage noReveal = new EnemyNarrationStage();
        public EnemyNarrationStage reveal = new EnemyNarrationStage();
    }
}
