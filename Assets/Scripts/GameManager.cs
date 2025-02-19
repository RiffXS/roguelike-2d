using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public BoardManager board;
    public PlayerController playerController;

    public TurnManager TurnManager { get; private set; }

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
        TurnManager = new TurnManager();
        
        board.Init();
        playerController.Spawn(board, Vector2Int.one);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
