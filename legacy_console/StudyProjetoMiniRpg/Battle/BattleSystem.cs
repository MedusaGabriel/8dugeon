using System;

public static class BattleSystem
{
    public static void Executar(Hero heroi, Enemy inimigo, string heroClassKey)
    {
        var orchestrator = new BattleOrchestrator();
        orchestrator.Executar(heroi, inimigo, heroClassKey);
    }
}
