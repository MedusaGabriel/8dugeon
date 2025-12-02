using System;
using System.Collections.Generic;

public static class HeroClassConfig
{
    
    public static readonly Dictionary<string, HeroClassStats> Classes = new Dictionary<string, HeroClassStats>(StringComparer.OrdinalIgnoreCase)
    {
        ["guerreiro"] = new HeroClassStats { Hp = 150, Ataque = 15, Defesa = 10, UsaMana = false, Alcance = 1 },
        ["mago"] = new HeroClassStats { Hp = 100, Ataque = 20, Defesa = 3, UsaMana = true, Alcance = 5 },
        ["arqueiro"] = new HeroClassStats { Hp = 100, Ataque = 22, Defesa = 4, UsaMana = false, Alcance = 4 },
    };
    public static readonly HeroClassStats DefaultClassStats = new HeroClassStats
    {
        Hp = 50,
        Ataque = 15,
        Defesa = 5,
        UsaMana = false,
        Alcance = 1
    };
}