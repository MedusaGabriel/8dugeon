using System;
using System.Collections.Generic;
using UnityEngine;

public class NarrationService
{
    private static NarrationService _instance;
    public static NarrationService Instance => _instance ??= new NarrationService();

    private readonly System.Random _rng = new System.Random();
    private Dictionary<NarrationKey, List<string>> _templatesByKey;

    private NarrationService()
    {
        LoadDatabase();
    }

    private void LoadDatabase()
    {
        // Arquivo JSON em Resources/Narrations/battle_narrations.json (sem .json aqui)
        TextAsset jsonAsset = Resources.Load<TextAsset>("Narrations/battle_narrations");
        if (jsonAsset == null)
        {
            Debug.LogError("NarrationService: battle_narrations.json não encontrado em Resources/Narrations.");
            _templatesByKey = new Dictionary<NarrationKey, List<string>>();
            return;
        }

        var data = JsonUtility.FromJson<NarrationDatabaseData>(jsonAsset.text);
        _templatesByKey = new Dictionary<NarrationKey, List<string>>();

        foreach (var entry in data.entries)
        {
            if (Enum.TryParse(entry.key, out NarrationKey key))
            {
                _templatesByKey[key] = entry.templates;
            }
            else
            {
                Debug.LogWarning($"NarrationService: chave de narração desconhecida: {entry.key}");
            }
        }
    }

    public void RaiseEvent(NarrationContext ctx, Action<string> outputCallback)
    {
        if (!_templatesByKey.TryGetValue(ctx.Key, out var templates) || templates.Count == 0)
        {
            outputCallback?.Invoke($"[Sem narração configurada para {ctx.Key}]");
            return;
        }

        int idx = _rng.Next(templates.Count);
        string raw = templates[idx];

        string resolved = ResolvePlaceholders(raw, ctx);
        outputCallback?.Invoke(resolved);
    }

    private string ResolvePlaceholders(string template, NarrationContext ctx)
    {
        string result = template;

        if (ctx.Hero != null)
            result = result.Replace("{heroName}", ctx.Hero.Name);

        if (ctx.Enemy != null)
            result = result.Replace("{enemyName}", ctx.Enemy.Name);

        if (ctx.Damage > 0)
            result = result.Replace("{damage}", ctx.Damage.ToString());

        return result;
    }

}
