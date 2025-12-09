using System;

public class PositionSystem
{
    // Posição RELATIVA do inimigo em relação ao herói,
    // considerando que o herói está sempre em (0,0).
    //
    // Eixos:
    // x < 0 = esquerda, x > 0 = direita
    // y > 0 = "para frente" (direção em que o herói está olhando)
    // y < 0 = "atrás" do herói
    public int EnemyX { get; private set; }
    public int EnemyY { get; private set; }

    // enemyStartY = 3 significa: inimigo começa 3 casas à frente do herói.
    public PositionSystem(int enemyStartX = 0, int enemyStartY = 3)
    {
        EnemyX = enemyStartX;
        EnemyY = enemyStartY;
    }

    // Distância de Chebyshev (tipo movimento do rei no xadrez):
    // dist = max(|dx|, |dy|)
    // dist = 1 -> adjacente (3x3 ao redor)
    // dist = 2 -> dentro de um quadrado 5x5, etc.
    private int DistanciaChebyshev()
    {
        int dx = Math.Abs(EnemyX);
        int dy = Math.Abs(EnemyY);
        return Math.Max(dx, dy);
    }

    // --- ALCANCE DO HERÓI / INIMIGO ---

    public bool HeroiPodeAtacar(Hero heroi)
    {
        int dist = DistanciaChebyshev();
        // alcance do herói = número de casas até onde ele pode atacar
        return dist <= heroi.alcance;
    }

    public bool InimigoPodeAtacar(Enemy inimigo)
    {
        int dist = DistanciaChebyshev();
        // alcance do inimigo = número de casas até onde ele pode atacar
        return dist <= inimigo.Alcance;
    }

    // --- MOVIMENTO DO INIMIGO ---

    // Usado pela IA do inimigo quando ele não consegue atacar ainda.
    public void InimigoAproxima()
    {
        // Move 1 casa em direção ao herói (0,0),
        // reduzindo a distância Chebyshev em 1, quando possível.
        int dx = EnemyX;
        int dy = EnemyY;

        if (dx > 0)      EnemyX--;  
        else if (dx < 0) EnemyX++;   

        if (dy > 0)      EnemyY--;   
        else if (dy < 0) EnemyY++;   

        BattleUI.Print("A criatura se aproxima, encurtando a distância entre vocês.");
    }

    // --- AÇÕES DO HERÓI (futuro: Aproximar / Recuar) ---

    // Do ponto de vista do sistema, o herói está sempre em (0,0).
    // Aproximar o herói do inimigo é equivalente a aproximar o inimigo do herói,
    // então podemos simplesmente reutilizar InimigoAproxima().
    public void Aproximar()
    {
        InimigoAproxima();
    }

    // Recuar = aumentar a distância entre herói e inimigo.
    public void Recuar()
    {
        int dx = EnemyX;
        int dy = EnemyY;

        if (dx > 0)      EnemyX++;   // empurra o inimigo mais para a direita
        else if (dx < 0) EnemyX--;   // empurra mais para a esquerda

        if (dy > 0)      EnemyY++;   // empurra mais "para frente"
        else if (dy < 0) EnemyY--;   // empurra mais "para trás"

        BattleUI.Print("Você recua, aumentando a distância em relação ao inimigo.");
    }
}
