using System;
using System.Collections.Generic;

public static class EnemyClassConfig
{
    
    public static readonly Dictionary<int, EnemyClassStats> Classes =
        new Dictionary<int, EnemyClassStats>
        
        {
            [1] = new EnemyClassStats
            {
                Nome = "Slime",
                Descricao = "Um monstro gelatinoso e pegajoso. Cuidado com seus ataques!",
                Hp = 50,
                Ataque = 5,
                Defesa = 2,
                Alcance = 1,
                ClasseId = 1,
            },
            [2] = new EnemyClassStats
            {
                Nome = "Goblin",
                Descricao = "Um pequeno monstro rapido e astuto",
                Hp = 80,
                Ataque = 10,
                Defesa = 5,
                Alcance = 1,
                ClasseId = 2,
            },
            [3] = new EnemyClassStats
            {
                Nome = "Orc",
                Descricao = "Um monstro forte e resistente.",
                Hp = 120,
                Ataque = 15,
                Defesa = 8,
                Alcance = 1,
                ClasseId = 3,
            }
        };

}