using System.Collections.Generic;
using System.Text.Json.Serialization;

public class HeroInputSynonymsDefinition
{
    [JsonPropertyName("atacar")]
    public List<string>? Atacar { get; set; }

    [JsonPropertyName("ataqueForte")]
    public List<string>? AtaqueForte { get; set; }

    [JsonPropertyName("defender")]
    public List<string>? Defender { get; set; }

    [JsonPropertyName("analisar")]
    public List<string>? Analisar { get; set; }

    [JsonPropertyName("fugir")]
    public List<string>? Fugir { get; set; }
}
