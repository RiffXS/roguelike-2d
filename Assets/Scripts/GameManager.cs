using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public BoardManager board;
    public PlayerController playerController;

    public TurnManager TurnManager { get; private set; }

    [SerializeField] UIDocument uiDoc;
    private Label _foodLabel;
    private int _foodAmount = 100;
    
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
    void Start()
    {
        _foodLabel = uiDoc.rootVisualElement.Q<Label>("FoodLabel");
        _foodLabel.text = "Food : " + _foodAmount;
        
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;
        
        board.Init();
        playerController.Spawn(board, Vector2Int.one);
    }

    void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    
    public void ChangeFood(int food) {
        _foodAmount += food;
        _foodLabel.text = "Food : " + _foodAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
