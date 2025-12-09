using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class HeroNarrationRepository
{
    private readonly Dictionary<string, HeroNarrationDefinition> _narrations;
    private static readonly Random _rng = new Random();

    private const string DefaultJsonPath = "Data/hero_narrations.json";
    public HeroNarrationRepository(string? jsonPath = null)
    {
        jsonPath ??= DefaultJsonPath;

        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException(
                $"Arquivo de narrações não encontrado em '{jsonPath}'. " +
                "Verifique o caminho e se a ação 'Copy to Output Directory' está configurada."
            );
        }

        string json = File.ReadAllText(jsonPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _narrations =
            JsonSerializer.Deserialize<Dictionary<string, HeroNarrationDefinition>>(json, options)
            ?? throw new InvalidOperationException(
                "Não foi possível desserializar o arquivo hero_narrations.json."
            );
    }


    public string GetRandomNarration(string heroClassKey, HeroNarrationEvent evento)
    {
        if (string.IsNullOrWhiteSpace(heroClassKey))
        {
            return GetFallbackNarration(evento);
        }

        if (!_narrations.TryGetValue(heroClassKey, out var def))
        {
            string lower = heroClassKey.ToLowerInvariant();
            _narrations.TryGetValue(lower, out def);
        }

        if (def is null)
        {
            return GetFallbackNarration(evento);
        }

        var lista = def.GetList(evento);

        if (lista == null || lista.Count == 0)
        {
            return GetFallbackNarration(evento);
        }

        int index = _rng.Next(lista.Count);
        return lista[index];
    }

    private static string GetFallbackNarration(HeroNarrationEvent evento)
    {
        return evento switch
        {
            HeroNarrationEvent.TomarDano =>
                "O herói sente o impacto do golpe, mas se mantém firme.",

            HeroNarrationEvent.CausarDano =>
                "O herói avança e ataca com determinação.",

            HeroNarrationEvent.Defender =>
                "O herói se prepara para o próximo ataque, assumindo uma postura defensiva.",

            HeroNarrationEvent.Fugir =>
                "O herói recua, priorizando a sobrevivência para lutar outro dia.",

            _ => "O herói reage, mas as palavras se perdem no caos da batalha."
        };
    }
}
