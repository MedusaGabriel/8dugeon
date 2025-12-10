using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class HeroInputSynonymsRepository
{
    private readonly Dictionary<string, HeroInputSynonymsDefinition> _porClasse;
    private readonly HeroInputSynonymsDefinition _global;

    private const string DefaultJsonPath = "Data/hero_input_synonyms.json";

    public HeroInputSynonymsRepository(string? jsonPath = null)
    {
        jsonPath ??= DefaultJsonPath;

        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException(
                $"Arquivo de sinônimos de input não encontrado em '{jsonPath}'. " +
                "Verifique o caminho e se o arquivo foi copiado para o diretório de saída."
            );
        }

        string json = File.ReadAllText(jsonPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dict =
            JsonSerializer.Deserialize<Dictionary<string, HeroInputSynonymsDefinition>>(json, options)
            ?? throw new InvalidOperationException(
                "Não foi possível desserializar o arquivo hero_input_synonyms.json."
            );

        _porClasse = new Dictionary<string, HeroInputSynonymsDefinition>(StringComparer.OrdinalIgnoreCase);

        HeroInputSynonymsDefinition globalTemp = new HeroInputSynonymsDefinition();

        foreach (var kvp in dict)
        {
            string key = kvp.Key.Trim();

            if (key.Equals("global", StringComparison.OrdinalIgnoreCase))
            {
                globalTemp = kvp.Value ?? new HeroInputSynonymsDefinition();
            }
            else
            {
                _porClasse[key.ToLowerInvariant()] = kvp.Value ?? new HeroInputSynonymsDefinition();
            }
        }

        _global = globalTemp;
    }

    public HeroInputSynonymsDefinition GetMergedForClass(string classe)
    {
        string key = (classe ?? "").Trim().ToLowerInvariant();

        _porClasse.TryGetValue(key, out var defsClasse);

        return Merge(_global, defsClasse);
    }

    private static HeroInputSynonymsDefinition Merge(
        HeroInputSynonymsDefinition global,
        HeroInputSynonymsDefinition? classe)
    {
        return new HeroInputSynonymsDefinition
        {
            Atacar = MergeList(global.Atacar, classe?.Atacar),
            AtaqueForte = MergeList(global.AtaqueForte, classe?.AtaqueForte),
            Defender = MergeList(global.Defender, classe?.Defender),
            Analisar = MergeList(global.Analisar, classe?.Analisar),
            Fugir = MergeList(global.Fugir, classe?.Fugir)
        };
    }

    private static List<string> MergeList(List<string>? a, List<string>? b)
    {
        var result = new List<string>();

        if (a != null)
            result.AddRange(a);

        if (b != null)
            result.AddRange(b);

        return result;
    }
}
