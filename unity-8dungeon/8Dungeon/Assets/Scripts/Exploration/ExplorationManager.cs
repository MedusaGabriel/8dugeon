using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager
{
    private readonly float _encounterChancePerStep;
    private readonly HashSet<Vector2Int> _activeEnemies = new HashSet<Vector2Int>();
    private readonly HashSet<Vector2Int> _obstacles = new HashSet<Vector2Int>();

    public Vector2Int PlayerPosition { get; private set; } = Vector2Int.zero;
    public IReadOnlyCollection<Vector2Int> ActiveEnemies => _activeEnemies;
    public IReadOnlyCollection<Vector2Int> Obstacles => _obstacles;

    private const int MaxStepsPerMove = 5;
    private const int BattleProximityRange = 1; // 3x3 ao redor do player

    public ExplorationManager(float encounterChancePerStep)
    {
        _encounterChancePerStep = Mathf.Clamp01(encounterChancePerStep);
    }

    public void SetObstacles(IEnumerable<Vector2Int> positions)
    {
        _obstacles.Clear();

        if (positions == null)
        {
            return;
        }

        foreach (Vector2Int pos in positions)
        {
            _obstacles.Add(pos);
        }
    }

    public ExplorationMoveResult Move(Vector2Int direction, int requestedSteps)
    {
        Vector2Int clampedDirection = new Vector2Int(Mathf.Clamp(direction.x, -1, 1), Mathf.Clamp(direction.y, -1, 1));
        if (clampedDirection == Vector2Int.zero)
        {
            clampedDirection = Vector2Int.up;
        }

        int stepsToTake = Mathf.Clamp(requestedSteps, 1, MaxStepsPerMove);
        int stepsTaken = 0;
        bool blockedByObstacle = false;
        Vector2Int? encounterPosition = null;

        for (int i = 0; i < stepsToTake; i++)
        {
            Vector2Int next = PlayerPosition + clampedDirection;
            if (_obstacles.Contains(next))
            {
                blockedByObstacle = true;
                break;
            }

            PlayerPosition = next;
            stepsTaken++;

            if (UnityEngine.Random.value <= _encounterChancePerStep)
            {
                encounterPosition = PlayerPosition;
                _activeEnemies.Add(PlayerPosition);
                break;
            }
        }

        bool proximityTriggered = false;
        Vector2Int? battlePosition = encounterPosition;

        if (!encounterPosition.HasValue)
        {
            MoveEnemiesTowardPlayer();
            if (TryFindEnemyInRange(out Vector2Int enemyInRange))
            {
                proximityTriggered = true;
                battlePosition = enemyInRange;
            }
        }
        else
        {
            proximityTriggered = true; // já está no mesmo tile
        }

        return new ExplorationMoveResult(
            stepsTaken,
            blockedByObstacle,
            encounterPosition.HasValue,
            proximityTriggered,
            PlayerPosition,
            battlePosition,
            _activeEnemies,
            _obstacles
        );
    }

    private void MoveEnemiesTowardPlayer()
    {
        if (_activeEnemies.Count == 0)
        {
            return;
        }

        HashSet<Vector2Int> updated = new HashSet<Vector2Int>();

        foreach (Vector2Int enemy in _activeEnemies)
        {
            Vector2Int target = ChooseEnemyStep(enemy);

            if (_obstacles.Contains(target) || updated.Contains(target))
            {
                target = enemy;
            }

            updated.Add(target);
        }

        _activeEnemies.Clear();
        foreach (Vector2Int enemy in updated)
        {
            _activeEnemies.Add(enemy);
        }
    }

    private Vector2Int ChooseEnemyStep(Vector2Int enemy)
    {
        Vector2Int delta = PlayerPosition - enemy;
        int stepX = delta.x == 0 ? 0 : (delta.x > 0 ? 1 : -1);
        int stepY = delta.y == 0 ? 0 : (delta.y > 0 ? 1 : -1);

        // prefere mover no eixo de maior distância; se igual, move diagonalmente
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return enemy + new Vector2Int(stepX, 0);
        }
        if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
        {
            return enemy + new Vector2Int(0, stepY);
        }

        return enemy + new Vector2Int(stepX, stepY);
    }

    private bool TryFindEnemyInRange(out Vector2Int enemyPosition)
    {
        foreach (Vector2Int enemy in _activeEnemies)
        {
            int dx = Mathf.Abs(enemy.x - PlayerPosition.x);
            int dy = Mathf.Abs(enemy.y - PlayerPosition.y);
            if (dx <= BattleProximityRange && dy <= BattleProximityRange)
            {
                enemyPosition = enemy;
                return true;
            }
        }

        enemyPosition = default;
        return false;
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
    public bool BlockedByObstacle { get; }
    public bool EncounteredEnemy { get; }
    public bool ProximityTriggered { get; }
    public Vector2Int PlayerPosition { get; }
    public Vector2Int? BattlePosition { get; }
    public IReadOnlyCollection<Vector2Int> ActiveEnemies { get; }
    public IReadOnlyCollection<Vector2Int> Obstacles { get; }
    public bool ShouldStartBattle => BattlePosition.HasValue;

    public ExplorationMoveResult(
        int stepsTaken,
        bool blockedByObstacle,
        bool encounteredEnemy,
        bool proximityTriggered,
        Vector2Int playerPosition,
        Vector2Int? battlePosition,
        IReadOnlyCollection<Vector2Int> activeEnemies,
        IReadOnlyCollection<Vector2Int> obstacles)
    {
        StepsTaken = stepsTaken;
        BlockedByObstacle = blockedByObstacle;
        EncounteredEnemy = encounteredEnemy;
        ProximityTriggered = proximityTriggered;
        PlayerPosition = playerPosition;
        BattlePosition = battlePosition;
        ActiveEnemies = activeEnemies;
        Obstacles = obstacles;
    }
}
