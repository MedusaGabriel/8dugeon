using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyNarrationRepository
{
    string GetLine(EnemyArchetype archetype, bool revealed, EnemyNarrationEvent evt);
}

public class EnemyNarrationRepository : IEnemyNarrationRepository
{
    private readonly Dictionary<EnemyArchetype, EnemyNarrationSet> _sets;
    private readonly EnemyNarrationStage _defaultNoReveal;
    private readonly EnemyNarrationStage _defaultReveal;
    private readonly System.Random _rng = new System.Random();

    public EnemyNarrationRepository(EnemyNarrationConfig config)
    {
        if (config == null)
        {
            _sets = new Dictionary<EnemyArchetype, EnemyNarrationSet>();
            _defaultNoReveal = new EnemyNarrationStage();
            _defaultReveal = new EnemyNarrationStage();
            return;
        }

        _sets = new Dictionary<EnemyArchetype, EnemyNarrationSet>();
        if (config.sets != null)
        {
            foreach (EnemyNarrationSet set in config.sets)
            {
                if (set == null) continue;
                _sets[set.archetype] = set;
                Debug.Log($"[EnemyNarrationRepository] Registrado set {set.archetype} (noReveal={CountLines(set.noReveal)}, reveal={CountLines(set.reveal)})");
                LogStageLines(set.archetype, "noReveal", set.noReveal);
                LogStageLines(set.archetype, "reveal", set.reveal);
            }
        }

        _defaultNoReveal = config.defaultNoReveal ?? new EnemyNarrationStage();
        _defaultReveal = config.defaultReveal ?? new EnemyNarrationStage();
        Debug.Log($"[EnemyNarrationRepository] Default noReveal={CountLines(_defaultNoReveal)}, reveal={CountLines(_defaultReveal)}");
    }

    public string GetLine(EnemyArchetype archetype, bool revealed, EnemyNarrationEvent evt)
    {
        EnemyNarrationStage stage = null;

        if (_sets.TryGetValue(archetype, out EnemyNarrationSet set) && set != null)
        {
            stage = revealed ? set.reveal : set.noReveal;
        }

        if (stage == null)
        {
            Debug.LogWarning($"[EnemyNarrationRepository] Nenhum set específico para {archetype}, usando default {(revealed ? "reveal" : "noReveal")}");
            stage = revealed ? _defaultReveal : _defaultNoReveal;
        }

        if (stage == null)
        {
            return null;
        }

        List<string> lines = stage.GetLines(evt);
        if (lines == null || lines.Count == 0)
        {
            Debug.LogWarning($"[EnemyNarrationRepository] Sem linhas para {archetype} em {(revealed ? "reveal" : "noReveal")} evento {evt}");
            return null;
        }

            int idx = _rng.Next(lines.Count);
            string chosen = lines[idx];
            Debug.Log($"[EnemyNarrationRepository] Usando {(revealed ? "reveal" : "noReveal")}->{evt} para {archetype}: '{chosen}'");
            return chosen;
    }

    private static int CountLines(EnemyNarrationStage stage)
    {
        if (stage == null)
        {
            return 0;
        }

        int total = 0;
        total += stage.appear?.Count ?? 0;
        total += stage.reveal?.Count ?? 0;
        total += stage.attack?.Count ?? 0;
        total += stage.defend?.Count ?? 0;
        total += stage.hurt?.Count ?? 0;
        total += stage.death?.Count ?? 0;
        return total;
    }

    private static void LogStageLines(EnemyArchetype archetype, string label, EnemyNarrationStage stage)
    {
        if (stage == null)
        {
            Debug.LogWarning($"[EnemyNarrationRepository] {archetype}.{label} sem dados");
            return;
        }

        LogLines(archetype, label, "appear", stage.appear);
        LogLines(archetype, label, "reveal", stage.reveal);
        LogLines(archetype, label, "attack", stage.attack);
        LogLines(archetype, label, "defend", stage.defend);
        LogLines(archetype, label, "hurt", stage.hurt);
        LogLines(archetype, label, "death", stage.death);
    }

    private static void LogLines(EnemyArchetype archetype, string stageLabel, string eventLabel, List<string> lines)
    {
        if (lines == null)
        {
            Debug.LogWarning($"[EnemyNarrationRepository] {archetype}.{stageLabel}.{eventLabel} nulo");
            return;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            Debug.Log($"[EnemyNarrationRepository] {archetype}.{stageLabel}.{eventLabel}[{i}] = '{lines[i]}'");
        }
    }
}
