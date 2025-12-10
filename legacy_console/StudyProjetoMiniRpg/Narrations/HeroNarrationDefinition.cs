using System.Collections.Generic;
using System.Text.Json.Serialization;


public class HeroNarrationDefinition
{

    [JsonPropertyName("tomarDano")]
    public List<string>? TomarDano{get; set;}

    [JsonPropertyName("causarDano")]
    public List<string>? CausarDano{get; set;}

    [JsonPropertyName("defender")]
    public List<string>? Defender{get; set;}
    [JsonPropertyName("fugir")]
    public List<string>? Fugir{get; set;}

    public List<string>? GetList(HeroNarrationEvent evento)
    {
        return evento switch
        {
            HeroNarrationEvent.TomarDano => TomarDano,
            HeroNarrationEvent.CausarDano => CausarDano,
            HeroNarrationEvent.Defender   => Defender,
            HeroNarrationEvent.Fugir      => Fugir,
            _ => null
        };
    }
}