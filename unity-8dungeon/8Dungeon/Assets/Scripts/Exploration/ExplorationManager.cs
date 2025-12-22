using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager
{
    private readonly float _encounterChancePerStep;
    private readonly HashSet<Vector2Int> _activeEnemies = new HashSet<Vector2Int>();

    public Vector2Int PlayerPosition { get; private set; } = Vector2Int.zero;
    public IReadOnlyCollection<Vector2Int> ActiveEnemies => _activeEnemies;

    public ExplorationManager(float encounterChancePerStep)
    {
        _encounterChancePerStep = Mathf.Clamp01(encounterChancePerStep);
    }

    public ExplorationMoveResult MoveForward(int requestedSteps)
    {
        int stepsToTake = Mathf.Clamp(requestedSteps, 1, 5);
        int stepsTaken = 0;
        Vector2Int? encounterPosition = null;

        for (int i = 0; i < stepsToTake; i++)
        {
            PlayerPosition += Vector2Int.up;
            stepsTaken++;

            if (UnityEngine.Random.value <= _encounterChancePerStep)
            {
                encounterPosition = PlayerPosition;
                _activeEnemies.Add(PlayerPosition);
                break;
            }
        }

        return new ExplorationMoveResult(
            stepsTaken,
            encounterPosition.HasValue,
            PlayerPosition,
            encounterPosition,
            _activeEnemies
        );
    }

    public void RemoveEnemy(Vector2Int position)
    {
        _activeEnemies.Remove(position);
    }

    public void Reset()
    {
        PlayerPosition = Vector2Int.zero;
        _activeEnemies.Clear();
    }
}

public readonly struct ExplorationMoveResult
{
    public int StepsTaken { get; }
    public bool EncounteredEnemy { get; }
    public Vector2Int PlayerPosition { get; }
    public Vector2Int? EncounterPosition { get; }
    public IReadOnlyCollection<Vector2Int> ActiveEnemies { get; }

    public ExplorationMoveResult(
        int stepsTaken,
        bool encounteredEnemy,
        Vector2Int playerPosition,
        Vector2Int? encounterPosition,
        IReadOnlyCollection<Vector2Int> activeEnemies)
    {
        StepsTaken = stepsTaken;
        EncounteredEnemy = encounteredEnemy;
        PlayerPosition = playerPosition;
        EncounterPosition = encounterPosition;
        ActiveEnemies = activeEnemies;
    }
}
