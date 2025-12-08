using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class HeroClassConfig
{
    public static IReadOnlyDictionary<string, HeroClassStats> Classes { get; private set; }
        = new Dictionary<string, HeroClassStats>(StringComparer.OrdinalIgnoreCase);

    public static readonly HeroClassStats DefaultClassStats = new HeroClassStats
    {
        Hp = 100,
        Ataque = 10,
        Defesa = 10,
        UsaMana = false,
        Alcance = 1
    };

    private const string CaminhoJson = "data/hero_classes.json";

    static HeroClassConfig()
    {
        CarregarClasses();
    }

    private static void CarregarClasses()
    {
        try
        {
            if (!File.Exists(CaminhoJson))
            {
                Console.WriteLine($"Arquivo não encontrado: {CaminhoJson}");
                return;
            }

            string json = File.ReadAllText(CaminhoJson);

            var dictDesserializado =
                JsonSerializer.Deserialize<Dictionary<string, HeroClassStats>>(json);

            if (dictDesserializado is null || dictDesserializado.Count == 0)
            {
                Console.WriteLine("hero_classes.json está vazio ou inválido.");
                return;
            }

            Classes = new Dictionary<string, HeroClassStats>(
                dictDesserializado,
                StringComparer.OrdinalIgnoreCase
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar hero_classes.json: {ex.Message}");
        }
    }

    public static HeroClassStats ObterOuPadrao(string classe)
    {
        if (string.IsNullOrWhiteSpace(classe))
            return DefaultClassStats;

        return Classes.TryGetValue(classe, out var stats)
            ? stats
            : DefaultClassStats;
    }
}


// using System;
// using System.Collections.Generic;

// public static class HeroClassConfig
// {

//     public static readonly Dictionary<string, HeroClassStats> Classes = new Dictionary<string, HeroClassStats>(StringComparer.OrdinalIgnoreCase)
//     {
//         ["guerreiro"] = new HeroClassStats { Hp = 150, Ataque = 15, Defesa = 10, UsaMana = false, Alcance = 1 },
//         ["mago"] = new HeroClassStats { Hp = 100, Ataque = 20, Defesa = 3, UsaMana = true, Alcance = 5 },
//         ["arqueiro"] = new HeroClassStats { Hp = 100, Ataque = 22, Defesa = 4, UsaMana = false, Alcance = 4 },
//     };
//     public static readonly HeroClassStats DefaultClassStats = new HeroClassStats
//     {
//         Hp = 50,
//         Ataque = 15,
//         Defesa = 5,
//         UsaMana = false,
//         Alcance = 1
//     };
// }