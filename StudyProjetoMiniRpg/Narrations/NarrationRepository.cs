using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class NarrationRepository
{
    private static readonly Dictionary<int, EnemyNarrationSet> _enemyNarrations = new();
    private static readonly Random _random = new();

    public static void LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Arquivo de narração não encontrado: {filePath}");

        string json = File.ReadAllText(filePath);

        var dict = JsonSerializer.Deserialize<Dictionary<string, EnemyNarrationSet>>(json);

        if (dict == null)
            throw new Exception("Falha ao desserializar o arquivo de narração.");

        _enemyNarrations.Clear();

        foreach (var kvp in dict)
        {
            if (int.TryParse(kvp.Key, out int classeId))
            {
                _enemyNarrations[classeId] = kvp.Value;
            }
        }
    }

    // ---------- Appear ----------
    public static string? GetRandomAppearText(int classeId, bool revelado)
    {
        if (!_enemyNarrations.TryGetValue(classeId, out var set))
            return null;

        var lista = revelado ? set.OnAppear.Revel : set.OnAppear.NoRevel;

        if (lista == null || lista.Count == 0)
        {
            lista = revelado ? set.OnAppear.NoRevel : set.OnAppear.Revel;
        }

        if (lista == null || lista.Count == 0)
            return null;

        int index = _random.Next(lista.Count);
        return lista[index];
    }

    // ---------- Attack ----------
    public static string? GetRandomAttackText(int classeId, bool revelado)
    {
        if (!_enemyNarrations.TryGetValue(classeId, out var set))
            return null;

        var lista = revelado ? set.OnAttack.Revel : set.OnAttack.NoRevel;

        if (lista == null || lista.Count == 0)
        {
            lista = revelado ? set.OnAttack.NoRevel : set.OnAttack.Revel;
        }

        if (lista == null || lista.Count == 0)
            return null;

        int index = _random.Next(lista.Count);
        return lista[index];
    }

    // ---------- Hurt ----------
    public static string? GetRandomHurtText(int classeId)
    {
        if (!_enemyNarrations.TryGetValue(classeId, out var set))
            return null;

        if (set.OnHurt == null || set.OnHurt.Count == 0)
            return null;

        int index = _random.Next(set.OnHurt.Count);
        return set.OnHurt[index];
    }

    // ---------- Death ----------
    public static string? GetRandomDeathText(int classeId)
    {
        if (!_enemyNarrations.TryGetValue(classeId, out var set))
            return null;

        if (set.OnDeath == null || set.OnDeath.Count == 0)
            return null;

        int index = _random.Next(set.OnDeath.Count);
        return set.OnDeath[index];
    }
}
