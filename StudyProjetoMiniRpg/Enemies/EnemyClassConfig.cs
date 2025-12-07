using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class EnemyClassConfig
{
    public static IReadOnlyDictionary<int, EnemyClassStats> Classes { get; private set; }
        = new Dictionary<int, EnemyClassStats>();

    private static readonly string JsonPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/enemy_classes.json");

    static EnemyClassConfig()
    {
        LoadFromJson();
    }

    private static void LoadFromJson()
    {
        if (!File.Exists(JsonPath))
        {
            throw new FileNotFoundException(
                $"Arquivo de configuração de inimigos não encontrado: {JsonPath}"
            );
        }

        string json = File.ReadAllText(JsonPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dados =
            JsonSerializer.Deserialize<Dictionary<string, EnemyClassStats>>(json, options)
            ?? throw new InvalidOperationException(
                "Falha ao desserializar enemy_classes.json: o resultado veio nulo."
            );

        var classesConvertidas = dados.ToDictionary(
            kvp => int.Parse(kvp.Key),
            kvp => kvp.Value
        );

        Classes = classesConvertidas;
    }
}

