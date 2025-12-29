using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager
{
    private readonly float _encounterChancePerStep;
    private readonly float _wallSpawnChance;
    private readonly float _ambientEnemySpawnChance;
    private readonly int _ambientEnemySpawnRadius;
    private readonly int _maxAmbientEnemies;
    private readonly int _randomSpawnRange;
    private readonly int _visionRadiusX;
    private readonly int _visionRadiusY;

    private readonly HashSet<Vector2Int> _activeEnemies = new HashSet<Vector2Int>();
    private readonly HashSet<Vector2Int> _obstacles = new HashSet<Vector2Int>();
    private readonly Dictionary<Vector2Int, TileData> _tiles = new Dictionary<Vector2Int, TileData>();

    private int _dungeonSeed;

    private struct TileData
    {
        public bool Generated;
        public bool IsWall;
    }

    public Vector2Int PlayerPosition { get; private set; } = Vector2Int.zero;
    public IReadOnlyCollection<Vector2Int> ActiveEnemies => _activeEnemies;
    public IReadOnlyCollection<Vector2Int> Obstacles => _obstacles;

    private const int MaxStepsPerMove = 5;
    private const int BattleProximityRange = 1; // 3x3 ao redor do player
    private const int MaxAmbientSpawnAttempts = 12;

    public ExplorationManager(
        float encounterChancePerStep,
        float wallSpawnChance,
        float ambientEnemySpawnChance,
        int ambientEnemySpawnRadius,
        int maxAmbientEnemies,
        int randomSpawnRange,
        int visionRadiusX,
        int visionRadiusY)
    {
        _encounterChancePerStep = Mathf.Clamp01(encounterChancePerStep);
        _wallSpawnChance = Mathf.Clamp01(wallSpawnChance);
        _ambientEnemySpawnChance = Mathf.Clamp01(ambientEnemySpawnChance);
        _ambientEnemySpawnRadius = Mathf.Max(1, ambientEnemySpawnRadius);
        _maxAmbientEnemies = Mathf.Max(1, maxAmbientEnemies);
        _randomSpawnRange = Mathf.Max(1, randomSpawnRange);
        _visionRadiusX = Mathf.Max(1, visionRadiusX);
        _visionRadiusY = Mathf.Max(1, visionRadiusY);
    }

    public void Reset(bool randomizeStartPosition)
    {
        _activeEnemies.Clear();
        _obstacles.Clear();
        _tiles.Clear();

        _dungeonSeed = UnityEngine.Random.Range(0, int.MaxValue);

        PlayerPosition = randomizeStartPosition ? GenerateRandomStartPosition() : Vector2Int.zero;

        PrefillAround(PlayerPosition, Vector2Int.zero, allowEnemySpawn: false);
    }

    public void Reset()
    {
        Reset(randomizeStartPosition: false);
    }

    public void EnsureCurrentTileGenerated()
    {
        PrefillAround(PlayerPosition, Vector2Int.zero, allowEnemySpawn: false);
    }

    public void SetObstacles(IEnumerable<Vector2Int> positions)
    {
        List<Vector2Int> existingKeys = new List<Vector2Int>(_tiles.Keys);

        foreach (Vector2Int key in existingKeys)
        {
            TileData data = _tiles[key];
            if (data.IsWall)
            {
                data.IsWall = false;
                _tiles[key] = data;
            }
        }

        _obstacles.Clear();

        if (positions == null)
        {
            return;
        }

        foreach (Vector2Int pos in positions)
        {
            if (pos == PlayerPosition)
            {
                continue;
            }

            TileData data = _tiles.ContainsKey(pos) ? _tiles[pos] : new TileData { Generated = true };
            data.Generated = true;
            data.IsWall = true;
            _tiles[pos] = data;
            _obstacles.Add(pos);
        }
    }

    public ExplorationMoveResult Move(Vector2Int direction, int requestedSteps)
    {
        Vector2Int clampedDirection = ClampDirection(direction, fallbackUp: true);
        PrefillAround(PlayerPosition, clampedDirection);

        int stepsToTake = Mathf.Clamp(requestedSteps, 1, MaxStepsPerMove);
        int stepsTaken = 0;
        bool blockedByObstacle = false;
        Vector2Int? encounterPosition = null;

        for (int i = 0; i < stepsToTake; i++)
        {
            Vector2Int next = PlayerPosition + clampedDirection;

            GenerateTile(next, forceEmpty: false);

            if (_obstacles.Contains(next))
            {
                blockedByObstacle = true;
                break;
            }

            PlayerPosition = next;
            stepsTaken++;

            PrefillAround(PlayerPosition, clampedDirection);

            if (UnityEngine.Random.value <= _encounterChancePerStep)
            {
                encounterPosition = PlayerPosition;
                _activeEnemies.Add(PlayerPosition);
                break;
            }
        }

        bool proximityTriggered = false;
        Vector2Int? battlePosition = encounterPosition;

        MoveEnemiesTowardPlayer();

        if (!encounterPosition.HasValue)
        {
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

            GenerateTile(target, forceEmpty: false);

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

    private void PrefillAround(Vector2Int center, Vector2Int forwardDir, bool allowEnemySpawn = true)
    {
        Vector2Int normalizedForward = ClampDirection(forwardDir, fallbackUp: false);

        GenerateTile(center, forceEmpty: true);

        for (int dx = -_visionRadiusX; dx <= _visionRadiusX; dx++)
        {
            for (int dy = -_visionRadiusY; dy <= _visionRadiusY; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                Vector2Int offset = new Vector2Int(dx, dy);
                Vector2Int candidate = center + offset;

                bool immediateNeighbor = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy)) == 1;
                bool keepOpen = immediateNeighbor && normalizedForward != Vector2Int.zero &&
                                (offset == normalizedForward || offset == -normalizedForward);
                bool existingWall = false;
                if (_tiles.TryGetValue(candidate, out TileData existingData))
                {
                    existingWall = existingData.IsWall;
                }

                bool forceEmpty = keepOpen && immediateNeighbor && !existingWall;

                GenerateTile(candidate, forceEmpty: forceEmpty);
            }
        }

        if (allowEnemySpawn)
        {
            TrySpawnAmbientEnemyNear(center);
        }
    }

    private void TrySpawnAmbientEnemyNear(Vector2Int center)
    {
        if (_activeEnemies.Count >= _maxAmbientEnemies)
        {
            return;
        }

        if (UnityEngine.Random.value > _ambientEnemySpawnChance)
        {
            return;
        }

        for (int attempt = 0; attempt < MaxAmbientSpawnAttempts; attempt++)
        {
            int offsetX = UnityEngine.Random.Range(-_ambientEnemySpawnRadius, _ambientEnemySpawnRadius + 1);
            int offsetY = UnityEngine.Random.Range(-_ambientEnemySpawnRadius, _ambientEnemySpawnRadius + 1);

            if (offsetX == 0 && offsetY == 0)
            {
                continue;
            }

            Vector2Int candidate = center + new Vector2Int(offsetX, offsetY);

            GenerateTile(candidate, forceEmpty: false);

            if (_obstacles.Contains(candidate) || _activeEnemies.Contains(candidate) || candidate == PlayerPosition)
            {
                continue;
            }

            if (Mathf.Abs(candidate.x - PlayerPosition.x) <= 1 && Mathf.Abs(candidate.y - PlayerPosition.y) <= 1)
            {
                continue; // evita spawn imediato ao lado do jogador
            }

            _activeEnemies.Add(candidate);
            break;
        }
    }

    private TileData GenerateTile(Vector2Int position, bool forceEmpty)
    {
        TileData data;

        if (_tiles.TryGetValue(position, out data))
        {
            if (forceEmpty && data.IsWall)
            {
                data.IsWall = false;
            }
        }
        else
        {
            data = new TileData
            {
                Generated = true,
                IsWall = !forceEmpty && ShouldPlaceWall(position)
            };
        }

        data.Generated = true;
        _tiles[position] = data;

        if (data.IsWall)
        {
            _obstacles.Add(position);
        }
        else
        {
            _obstacles.Remove(position);
        }

        return data;
    }

    private bool ShouldPlaceWall(Vector2Int position)
    {
        if (position == PlayerPosition)
        {
            return false;
        }

        float sample = Deterministic01(position);
        return sample < _wallSpawnChance;
    }

    private float Deterministic01(Vector2Int position)
    {
        int hash = position.x * 374761393 + position.y * 668265263 + _dungeonSeed * 700001;
        hash = (hash ^ (hash >> 13)) * 1274126177;
        hash ^= hash >> 16;

        int masked = hash & 0x7FFFFFFF;
        return masked / (float)int.MaxValue;
    }

    private Vector2Int ClampDirection(Vector2Int direction, bool fallbackUp)
    {
        int x = Mathf.Clamp(direction.x, -1, 1);
        int y = Mathf.Clamp(direction.y, -1, 1);

        if (x == 0 && y == 0)
        {
            return fallbackUp ? Vector2Int.up : Vector2Int.zero;
        }

        return new Vector2Int(x, y);
    }

    private Vector2Int GenerateRandomStartPosition()
    {
        int x = UnityEngine.Random.Range(-_randomSpawnRange, _randomSpawnRange + 1);
        int y = UnityEngine.Random.Range(-_randomSpawnRange, _randomSpawnRange + 1);
        return new Vector2Int(x, y);
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
