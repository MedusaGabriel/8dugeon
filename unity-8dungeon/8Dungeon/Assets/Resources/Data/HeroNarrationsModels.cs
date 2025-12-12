using System;
using System.Collections.Generic;

[Serializable]
public class HeroNarrationsConfig
{
    public int schemaVersion = 1;
    public string defaultKey = "default";
    public List<HeroNarrationSet> sets = new List<HeroNarrationSet>();
}

[Serializable]
public class HeroNarrationSet
{
    public string key;

    public List<string> onEncounter = new List<string>();
    public List<string> onAttack = new List<string>();
    public List<string> onAnalyze = new List<string>();

    public List<string> GetLines(NarrationEvent evt)
    {
        switch (evt)
        {
            case NarrationEvent.EncounterStart: return onEncounter;
            case NarrationEvent.PlayerAttack: return onAttack;
            case NarrationEvent.PlayerAnalyze: return onAnalyze;
            default: return s_empty;
        }
    }

    private static readonly List<string> s_empty = new List<string>();
}
