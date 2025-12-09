using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class NarrationRepository
{
    private static Dictionary<int, EnemyNarrationDefinition> _enemyNarrations =
        new Dictionary<int, EnemyNarrationDefinition>();

    public static void LoadFromJson(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Arquivo de narrações de inimigos não encontrado em '{path}'.");
        }

        string json = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dict =
            JsonSerializer.Deserialize<Dictionary<string, EnemyNarrationDefinition>>(json, options)
            ?? throw new InvalidOperationException(
                "Não foi possível desserializar o arquivo enemy_narrations.json."
            );

        _enemyNarrations.Clear();

        foreach (var kvp in dict)
        {
            if (int.TryParse(kvp.Key, out int id))
            {
                _enemyNarrations[id] = kvp.Value;
            }
        }
    }

    private static EnemyNarrationDefinition? GetDef(int classeId)
    {
        _enemyNarrations.TryGetValue(classeId, out var def);
        return def;
    }

    private static string? PickRandom(List<string>? lista)
    {
        if (lista == null || lista.Count == 0)
            return null;

        var rng = new Random();
        int index = rng.Next(lista.Count);
        return lista[index];
    }

    public static string? GetRandomAppearText(int classeId, bool revelado)
    {
        var def = GetDef(classeId);
        if (def?.OnAppear == null) return null;

        return revelado
            ? PickRandom(def.OnAppear.Revel)
            : PickRandom(def.OnAppear.NoRevel);
    }

    public static string? GetRandomHurtText(int classeId)
    {
        var def = GetDef(classeId);
        return PickRandom(def?.OnHurt);
    }

    public static string? GetRandomDeathText(int classeId)
    {
        var def = GetDef(classeId);
        return PickRandom(def?.OnDeath);
    }

    public static string? GetRandomAttackText(int classeId, bool revelado, EnemyAttackStyle estilo)
    {
        var def = GetDef(classeId);
        if (def?.OnAttack == null) return null;

        var variante = def.OnAttack.GetForStyle(estilo);
        if (variante == null) return null;

        return revelado
            ? PickRandom(variante.Revel)
            : PickRandom(variante.NoRevel);
    }

    public static string? GetRandomAttackText(int classeId, bool revelado)
    {
        return GetRandomAttackText(classeId, revelado, EnemyAttackStyle.Fraco);
    }
}
