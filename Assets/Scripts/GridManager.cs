using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 15;
    public float cellSize = 1f;
    public GameObject stonePrefab;
    public Transform gridParent;
    
    [Header("Line Settings")]
    public LineRenderer lineRendererPrefab;
    public Color lineColor = Color.black;
    public float lineWidth = 0.05f;
    
    private Vector3[,] gridPositions;
    private GameObject[,] stones;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        InitializeGrid();
        DrawGridLines();
    }
    
    void InitializeGrid()
    {
        gridPositions = new Vector3[gridSize, gridSize];
        stones = new GameObject[gridSize, gridSize];
        
        Vector3 startPos = new Vector3(-(gridSize - 1) * cellSize / 2f, -(gridSize - 1) * cellSize / 2f, 0);
        
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridPositions[x, y] = startPos + new Vector3(x * cellSize, y * cellSize, 0);
            }
        }
    }
    
    void DrawGridLines()
    {
        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineMaterial.color = lineColor;
        
        for (int i = 0; i < gridSize; i++)
        {
            LineRenderer horizontalLine = Instantiate(lineRendererPrefab, gridParent);
            horizontalLine.material = lineMaterial;
            horizontalLine.startWidth = lineWidth;
            horizontalLine.endWidth = lineWidth;
            horizontalLine.positionCount = 2;
            horizontalLine.SetPosition(0, gridPositions[0, i]);
            horizontalLine.SetPosition(1, gridPositions[gridSize - 1, i]);
            
            LineRenderer verticalLine = Instantiate(lineRendererPrefab, gridParent);
            verticalLine.material = lineMaterial;
            verticalLine.startWidth = lineWidth;
            verticalLine.endWidth = lineWidth;
            verticalLine.positionCount = 2;
            verticalLine.SetPosition(0, gridPositions[i, 0]);
            verticalLine.SetPosition(1, gridPositions[i, gridSize - 1]);
        }
    }
    
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        Vector3 localPos = worldPosition;
        Vector3 startPos = new Vector3(-(gridSize - 1) * cellSize / 2f, -(gridSize - 1) * cellSize / 2f, 0);
        
        int x = Mathf.RoundToInt((localPos.x - startPos.x) / cellSize);
        int y = Mathf.RoundToInt((localPos.y - startPos.y) / cellSize);
        
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return new Vector2Int(x, y);
        }
        
        return new Vector2Int(-1, -1);
    }
    
    public Vector3 GetWorldPosition(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return gridPositions[x, y];
        }
        return Vector3.zero;
    }
    
    public bool CanPlaceStone(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize && stones[x, y] == null;
    }
    
    public GameObject PlaceStone(int x, int y, bool isBlack)
    {
        if (!CanPlaceStone(x, y)) return null;
        
        GameObject stone = Instantiate(stonePrefab, GetWorldPosition(x, y), Quaternion.identity, gridParent);
        Stone stoneComponent = stone.GetComponent<Stone>();
        if (stoneComponent != null)
        {
            stoneComponent.SetStone(isBlack);
        }
        
        stones[x, y] = stone;
        return stone;
    }
    
    public GameObject GetStone(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return stones[x, y];
        }
        return null;
    }
    
    public void ClearGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (stones[x, y] != null)
                {
                    DestroyImmediate(stones[x, y]);
                    stones[x, y] = null;
                }
            }
        }
    }
}