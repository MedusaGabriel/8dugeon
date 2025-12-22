using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI grid that paints a base color for all cells and highlights
/// player/enemy positions on demand. Intended to sit under Panel_Grid.
/// </summary>
[AddComponentMenu("8Dungeon/UI/Grid View Controller")]
public class GridViewController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private Image cellPrefab;

    [Header("Layout")]
    [SerializeField, Min(1)] private int columns = 5;
    [SerializeField, Min(1)] private int rows = 5;
    [SerializeField] private Vector2 cellSize = new Vector2(32f, 32f);
    [SerializeField] private Vector2 cellSpacing = new Vector2(4f, 4f);

    [Header("Colors")]
    [SerializeField] private Color baseColor = Color.black;
    [SerializeField] private Color playerColor = new Color(0.6f, 0f, 0.8f); // Roxo
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color wallColor = Color.gray;

    private Image[,] _cells;
    private Vector2Int _playerPosition;
    private readonly HashSet<Vector2Int> _enemyPositions = new HashSet<Vector2Int>();
    private readonly HashSet<Vector2Int> _obstacles = new HashSet<Vector2Int>();

    private void Awake()
    {
        BuildGrid();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        columns = Mathf.Max(1, columns);
        rows = Mathf.Max(1, rows);

        if (gridContainer == null)
        {
            gridContainer = GetComponent<RectTransform>();
        }
    }
#endif

    /// <summary>
    /// Regenerates the entire grid. Safe to call after tweaking inspector values.
    /// </summary>
    public void BuildGrid()
    {
        if (gridContainer == null || cellPrefab == null)
        {
            Debug.LogError("[GridViewController] Missing gridContainer or cellPrefab.", this);
            return;
        }

        ClearContainerChildren();
        ConfigureLayoutComponent();

        _cells = new Image[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Image cell = Instantiate(cellPrefab, gridContainer);
                cell.name = $"Cell_{x}_{y}";
                cell.color = baseColor;
                _cells[x, y] = cell;
            }
        }

        _enemyPositions.Clear();
    }

    /// <summary>
    /// Highlights the given positions. Player takes priority over enemies.
    /// </summary>
    public void Render(Vector2Int playerPosition, IEnumerable<Vector2Int> enemies)
    {
        if (_cells == null || _cells.Length == 0)
        {
            Debug.LogWarning("[GridViewController] Grid not built yet. Building now.", this);
            BuildGrid();
        }

        _playerPosition = playerPosition;
        _enemyPositions.Clear();

        if (enemies != null)
        {
            foreach (Vector2Int enemyPos in enemies)
            {
                _enemyPositions.Add(enemyPos);
            }
        }

        PaintGrid();
    }

    /// <summary>
    /// Updates only the player position. Useful when enemies stay put.
    /// </summary>
    public void SetPlayerPosition(Vector2Int playerPosition)
    {
        if (_cells == null || _cells.Length == 0)
        {
            Debug.LogWarning("[GridViewController] Grid not built yet. Building now.", this);
            BuildGrid();
        }

        _playerPosition = playerPosition;
        PaintGrid();
    }

    /// <summary>
    /// Replace all enemies currently displayed.
    /// </summary>
    public void SetEnemyPositions(IEnumerable<Vector2Int> enemies)
    {
        if (_cells == null || _cells.Length == 0)
        {
            Debug.LogWarning("[GridViewController] Grid not built yet. Building now.", this);
            BuildGrid();
        }

        _enemyPositions.Clear();

        if (enemies != null)
        {
            foreach (Vector2Int enemyPos in enemies)
            {
                _enemyPositions.Add(enemyPos);
            }
        }

        PaintGrid();
    }

    public void SetObstacles(IEnumerable<Vector2Int> obstacles)
    {
        _obstacles.Clear();

        if (obstacles != null)
        {
            foreach (Vector2Int obstacle in obstacles)
            {
                _obstacles.Add(obstacle);
            }
        }

        PaintGrid();
    }

#if UNITY_EDITOR
    [ContextMenu("Render Sample (Editor)")]
    private void RenderSample()
    {
        BuildGrid();
        var sampleEnemies = new List<Vector2Int>
        {
            new Vector2Int(2, 0),
            new Vector2Int(-1, -2)
        };
            var sampleObstacles = new List<Vector2Int>
            {
                new Vector2Int(1, 0),
                new Vector2Int(0, -1)
            };

            SetObstacles(sampleObstacles);
        Render(new Vector2Int(0, 0), sampleEnemies);
    }
#endif

    private void PaintGrid()
    {
        if (_cells == null)
        {
            return;
        }

        int centerX = columns / 2;
        int centerY = rows / 2;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2Int worldPos = new Vector2Int(
                    _playerPosition.x + (x - centerX),
                    _playerPosition.y + (y - centerY));

                Color color = baseColor;

                if (worldPos == _playerPosition)
                {
                    color = playerColor;
                }
                else if (_enemyPositions.Contains(worldPos))
                {
                    color = enemyColor;
                }
                else if (_obstacles.Contains(worldPos))
                {
                    color = wallColor;
                }

                _cells[x, y].color = color;
            }
        }
    }

    private void ConfigureLayoutComponent()
    {
        GridLayoutGroup layout = gridContainer.GetComponent<GridLayoutGroup>();
        if (layout == null)
        {
            layout = gridContainer.gameObject.AddComponent<GridLayoutGroup>();
        }

        layout.cellSize = cellSize;
        layout.spacing = cellSpacing;
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = columns;
        layout.startAxis = GridLayoutGroup.Axis.Horizontal;
        layout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layout.childAlignment = TextAnchor.MiddleCenter;
    }

    private void ClearContainerChildren()
    {
        for (int i = gridContainer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gridContainer.GetChild(i).gameObject);
        }
    }
}
