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

    public void RaiseEvent(NarrationEvent evt, Action<string> outputCallback)
    {
        if (!_templatesByKey.TryGetValue(evt.Key, out var templates) || templates.Count == 0)
        {
            outputCallback?.Invoke($"[Sem narração configurada para {evt.Key}]");
            return;
        }

        int idx = _rng.Next(templates.Count);
        string raw = templates[idx];

        string resolved = ResolvePlaceholders(raw, evt);

        outputCallback?.Invoke(resolved);
    }

    private string ResolvePlaceholders(string template, NarrationEvent evt)
    {
        string result = template;

        if (evt.Hero != null)
        {
            result = result.Replace("{heroName}", evt.Hero.Name);
        }

        if (evt.Enemy != null)
        {
            result = result.Replace("{enemyName}", evt.Enemy.Name);
        }

        if (evt.Damage > 0)
        {
            result = result.Replace("{damage}", evt.Damage.ToString());
        }

        return result;
    }

}
