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

        EnemyNarrationConfig cfg = JsonUtility.FromJson<EnemyNarrationConfig>(text.text);
        if (cfg == null)
        {
            Debug.LogWarning("[EnemyNarrationsLoader] Falha ao converter JSON para EnemyNarrationConfig.");
            return null;
        }

        return cfg;
    }
}
