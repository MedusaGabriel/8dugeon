using System;
using System.Collections.Generic;

public interface IEnemyNarrationRepository
{
    string GetLine(EnemyArchetype archetype, bool revealed, EnemyNarrationEvent evt);
}

public class EnemyNarrationRepository : IEnemyNarrationRepository
{
    private readonly Dictionary<EnemyArchetype, EnemyNarrationSet> _sets;
    private readonly EnemyNarrationStage _defaultNoReveal;
    private readonly EnemyNarrationStage _defaultReveal;
    private readonly Random _rng = new Random();

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
            }
        }

        _defaultNoReveal = config.defaultNoReveal ?? new EnemyNarrationStage();
        _defaultReveal = config.defaultReveal ?? new EnemyNarrationStage();
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
            stage = revealed ? _defaultReveal : _defaultNoReveal;
        }

        if (stage == null)
        {
            return null;
        }

        List<string> lines = stage.GetLines(evt);
        if (lines == null || lines.Count == 0)
        {
            return null;
        }

        int idx = _rng.Next(lines.Count);
        return lines[idx];
    }
}
