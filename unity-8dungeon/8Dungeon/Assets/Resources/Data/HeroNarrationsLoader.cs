using UnityEngine;

public static class HeroNarrationsLoader
{
    public static HeroNarrationsConfig Load()
    {
        TextAsset text = Resources.Load<TextAsset>("Data/hero_narrations");
        if (text == null)
        {
            Debug.LogError("[HeroNarrationsLoader] Não encontrou Data/hero_narrations em Resources.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(text.text))
        {
            Debug.LogError("[HeroNarrationsLoader] hero_narrations.json está vazio.");
            return null;
        }

        var cfg = JsonUtility.FromJson<HeroNarrationsConfig>(text.text);
        if (cfg == null)
        {
            Debug.LogError("[HeroNarrationsLoader] Parse retornou null.");
            return null;
        }

        Debug.Log($"[HeroNarrationsLoader] OK. sets={cfg.sets?.Count ?? 0} defaultKey={cfg.defaultKey}");
        return cfg;
    }
}
