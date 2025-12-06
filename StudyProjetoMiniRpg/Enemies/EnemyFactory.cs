using System;
using System.Collections.Generic;

public static class EnemyFactory
{
    private static readonly Random _random = new Random();

    public static Enemy CriarInimigo(int classeId)
    {
        if (!EnemyClassConfig.Classes.TryGetValue(classeId, out var stats))
        {
            throw new ArgumentException($"Classe de inimigo com ID {classeId} não encontrada.");
        }
        return new Enemy(stats);
    }

    public static Enemy CriarInimigoAleatorio()
    {
         var todasClasses = new List<EnemyClassStats>(EnemyClassConfig.Classes.Values);
        int index = _random.Next(todasClasses.Count);
        var statsSorteado = todasClasses[index];

        return new Enemy(statsSorteado); 
    }
}