using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoardManager _mBoardManager;
    private Vector2Int _mPlayerPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Spawns the Character
    public void Spawn(BoardManager boardManager, Vector2Int playerPosition)
    {
        _mBoardManager = boardManager;
        _mPlayerPosition = playerPosition;
        
        transform.position = _mBoardManager.CellToWorld(_mPlayerPosition);
    }
}
