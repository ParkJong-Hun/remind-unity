using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text turnText;
    public Text winText;
    public Button restartButton;
    public GameObject winPanel;
    
    [Header("Text Settings")]
    public string blackTurnText = "흑돌 차례";
    public string whiteTurnText = "백돌 차례";
    public string blackWinText = "흑돌 승리!";
    public string whiteWinText = "백돌 승리!";
    
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }
    
    public void UpdateTurnText(bool isBlackTurn)
    {
        if (turnText != null)
        {
            turnText.text = isBlackTurn ? blackTurnText : whiteTurnText;
        }
    }
    
    public void ShowWinMessage(bool blackWon)
    {
        if (winText != null)
        {
            winText.text = blackWon ? blackWinText : whiteWinText;
        }
        
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
    
    public void HideWinMessage()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }
    
    public void RestartGame()
    {
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
    }
}