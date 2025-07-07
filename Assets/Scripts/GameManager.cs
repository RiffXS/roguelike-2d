using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public BoardManager board;
    public PlayerController playerController;

    public TurnManager TurnManager { get; private set; }

    [Header("UI Elements")]
    [SerializeField]
    private UIDocument uiDoc;
    private Label _foodLabel;
    private int _foodAmount = 20;

    [Header("GameOver")]
    private VisualElement _gameOverPanel;
    private Label _gameOverMessage;

    private int _currentLevel;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        _foodLabel = uiDoc.rootVisualElement.Q<Label>("FoodLabel");
        
        _gameOverPanel = uiDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        _gameOverMessage = _gameOverPanel.Q<Label>("GameOverMessage");

        StartNewGame();
    }

    public void StartNewGame()
    {
        _gameOverPanel.style.visibility = Visibility.Hidden;
        
        _currentLevel = 0;
        _foodAmount = 20;
        _foodLabel.text = "Food : " + _foodAmount;
        
        NewLevel();
    }

    public void NewLevel()
    {
        board.ClearLevel();
        board.Init();
        playerController.Spawn(board, Vector2Int.one);
        
        _currentLevel++;
    }
    
    private void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    
    public void ChangeFood(int food) {
        _foodAmount += food;
        _foodLabel.text = "Food : " + _foodAmount;

        if (_foodAmount <= 0)
        {
            playerController.GameOver();
            _gameOverPanel.style.visibility = Visibility.Visible;
            _gameOverMessage.text = "Game Over!\n\n You traveled through " + _currentLevel + " levels\n\nPress Enter to restart the game";
        }
    }
}
