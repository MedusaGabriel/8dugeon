using System.Linq;
using UnityEngine;

public interface IHeroNarrationRepository
{
    string GetLine(string key, NarrationEvent evt);
}

public class HeroNarrationRepository : IHeroNarrationRepository
{
    private readonly HeroNarrationsConfig _cfg;
    private readonly NarrationPicker _picker;

    public HeroNarrationRepository(HeroNarrationsConfig cfg, NarrationPicker picker)
    {
        _cfg = cfg;
        _picker = picker;
    }

    public string GetLine(string key, NarrationEvent evt)
    {
        if (_cfg == null || _cfg.sets == null || _cfg.sets.Count == 0)
            return Fallback(evt);

        var set = FindSet(key);
        if (set != null)
        {
            var chosen = _picker.Pick(set.key, evt, set.GetLines(evt));
            if (!string.IsNullOrWhiteSpace(chosen)) return chosen;
        }
        else
        {
            Debug.LogWarning($"[NarrationRepo] Set '{key}' não encontrado. Tentando default.");
        }

        var def = FindSet(_cfg.defaultKey);
        if (def != null)
        {
            var chosen = _picker.Pick(def.key, evt, def.GetLines(evt));
            if (!string.IsNullOrWhiteSpace(chosen)) return chosen;
        }

        return Fallback(evt);
    }

    private HeroNarrationSet FindSet(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;
        string norm = key.Trim().ToLowerInvariant();

        return _cfg.sets.FirstOrDefault(s =>
            s != null && !string.IsNullOrWhiteSpace(s.key) &&
            s.key.Trim().ToLowerInvariant() == norm
        );
    }

    private string Fallback(NarrationEvent evt)
    {
        switch (evt)
        {
            case NarrationEvent.EncounterStart: return "Algo se aproxima...";
            case NarrationEvent.PlayerAttack:   return "Você ataca.";
            case NarrationEvent.PlayerAnalyze:  return "Você observa com atenção.";
            case NarrationEvent.PlayerDefend:   return "Você se prepara para defender.";
            default: return "...";
        }
    }
}
