using System;
using System.Collections.Generic;
using System.Linq;

public static class EnemyFactory
{
    private static readonly Random _rng = new Random();

    public static Enemy CriarInimigoAleatorio()
    {
        if (EnemyClassConfig.Classes == null || EnemyClassConfig.Classes.Count == 0)
        {
            throw new InvalidOperationException(
                "EnemyClassConfig.Classes está vazio. Verifique se o JSON foi carregado corretamente."
            );
        }

        List<EnemyClassStats> listaClasses = EnemyClassConfig.Classes.Values.ToList();

        int index = _rng.Next(listaClasses.Count);

        EnemyClassStats stats = listaClasses[index];

        return new Enemy(stats);
    }

    public static Enemy CriarPorClasseId(int classeId)
    {
        if (!EnemyClassConfig.Classes.TryGetValue(classeId, out var stats))
        {
            throw new ArgumentException(
                $"Classe de inimigo com ID {classeId} não encontrada em EnemyClassConfig."
            );
        }

        return new Enemy(stats);
    }
}
