using System.Collections.Generic;
using System.Text.Json.Serialization;

// ===== ENUM =====
public enum EnemyAttackStyle
{
    Rapido,
    Fraco,
    Forte
}

// ===== NARRATION DTOs =====

public class EnemyAppearNarration
{
    [JsonPropertyName("NoRevel")]
    public List<string>? NoRevel { get; set; }

    [JsonPropertyName("Revel")]
    public List<string>? Revel { get; set; }
}

public class EnemyAttackVariantNarration
{
    [JsonPropertyName("NoRevel")]
    public List<string>? NoRevel { get; set; }

    [JsonPropertyName("Revel")]
    public List<string>? Revel { get; set; }
}

// LEMBRAR GABRIEL O ESSE METODO AQUI, nao esta sendo utilizado
// pois no json do enemy_narrations nao tem nada para
// uma versao revel e nao revel, mas 
//para o bem da sua saude mental vamos manter assim

public class EnemyHurtNarration
{
    public List<string>? NoRevel { get; set; }
    public List<string>? Revel { get; set; }
}

public class EnemyAttackNarrationGroup
{
    [JsonPropertyName("Rapido")]
    public EnemyAttackVariantNarration? Rapido { get; set; }

    [JsonPropertyName("Fraco")]
    public EnemyAttackVariantNarration? Fraco { get; set; }

    [JsonPropertyName("Forte")]
    public EnemyAttackVariantNarration? Forte { get; set; }

    // Opcional: fallback genérico
    [JsonPropertyName("Default")]
    public EnemyAttackVariantNarration? Default { get; set; }

    public EnemyAttackVariantNarration? GetForStyle(EnemyAttackStyle estilo)
    {
        return estilo switch
        {
            EnemyAttackStyle.Rapido => Rapido ?? Default,
            EnemyAttackStyle.Fraco  => Fraco  ?? Default,
            EnemyAttackStyle.Forte  => Forte  ?? Default,
            _ => Default
        };
    }
}


//aqui tambem 
public class EnemyPhaseNarration
{
    [JsonPropertyName("NoRevel")]
    public List<string>? NoRevel { get; set; }

    [JsonPropertyName("Revel")]
    public List<string>? Revel { get; set; }
}

public class EnemyNarrationDefinition
{
    [JsonPropertyName("OnAppear")]
    public EnemyAppearNarration? OnAppear { get; set; }

    [JsonPropertyName("OnAttack")]
    public EnemyAttackNarrationGroup? OnAttack { get; set; }

    [JsonPropertyName("OnHurt")]
    public List<string>? OnHurt { get; set; }

    [JsonPropertyName("OnDeath")]
    public List<string>? OnDeath { get; set; }
}
