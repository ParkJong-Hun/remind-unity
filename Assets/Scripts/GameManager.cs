using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public GridManager gridManager;
    public UIManager uiManager;
    
    [Header("Game State")]
    public bool isBlackTurn = true;
    public bool gameOver = false;
    
    private Camera mainCamera;
    private int[,] board;
    private int gridSize;
    
    void Start()
    {
        mainCamera = Camera.main;
        gridSize = gridManager.gridSize;
        board = new int[gridSize, gridSize];
        
        if (uiManager != null)
        {
            uiManager.UpdateTurnText(isBlackTurn);
        }
    }
    
    void Update()
    {
        if (!gameOver && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleMouseClick();
        }
    }
    
    void HandleMouseClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        
        Vector2Int gridPos = gridManager.GetGridPosition(worldPos);
        
        if (gridPos.x >= 0 && gridPos.y >= 0 && gridManager.CanPlaceStone(gridPos.x, gridPos.y))
        {
            PlaceStone(gridPos.x, gridPos.y);
        }
    }
    
    void PlaceStone(int x, int y)
    {
        GameObject stone = gridManager.PlaceStone(x, y, isBlackTurn);
        if (stone != null)
        {
            board[x, y] = isBlackTurn ? 1 : 2;
            
            if (CheckWin(x, y))
            {
                gameOver = true;
                if (uiManager != null)
                {
                    uiManager.ShowWinMessage(isBlackTurn);
                }
            }
            else
            {
                isBlackTurn = !isBlackTurn;
                if (uiManager != null)
                {
                    uiManager.UpdateTurnText(isBlackTurn);
                }
            }
        }
    }
    
    bool CheckWin(int x, int y)
    {
        int player = board[x, y];
        
        int[] dx = { 1, 0, 1, 1 };
        int[] dy = { 0, 1, 1, -1 };
        
        for (int dir = 0; dir < 4; dir++)
        {
            int count = 1;
            
            for (int i = 1; i < 5; i++)
            {
                int nx = x + dx[dir] * i;
                int ny = y + dy[dir] * i;
                
                if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridSize && board[nx, ny] == player)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            
            for (int i = 1; i < 5; i++)
            {
                int nx = x - dx[dir] * i;
                int ny = y - dy[dir] * i;
                
                if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridSize && board[nx, ny] == player)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            
            if (count >= 5)
            {
                return true;
            }
        }
        
        return false;
    }
    
    public void RestartGame()
    {
        gameOver = false;
        isBlackTurn = true;
        
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                board[x, y] = 0;
            }
        }
        
        gridManager.ClearGrid();
        
        if (uiManager != null)
        {
            uiManager.UpdateTurnText(isBlackTurn);
            uiManager.HideWinMessage();
        }
    }
}