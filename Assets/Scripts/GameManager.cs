using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public BoardManager board;
    public PlayerController playerController;

    private TurnManager _turnManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _turnManager = new TurnManager();
        
        board.Init();
        playerController.Spawn(board, Vector2Int.one);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
