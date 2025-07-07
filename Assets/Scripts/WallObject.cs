using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    [Header("Tile")]
    private Tile _originalTile;
    [SerializeField]
    private Tile obstacleTile;
    [SerializeField]
    private Tile lowHeathTile;

    [Header("Health")]
    private int _currentHealth;
    [SerializeField]
    private int maxHealth = 3;
    
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        
        _currentHealth = maxHealth;
        
        _originalTile = GameManager.Instance.board.GetCellTile(cell);
        GameManager.Instance.board.SetCellTile(cell, obstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        GameManager.Instance.playerController.Attack();
        _currentHealth -= 1;

        if (_currentHealth > 0)
        {
            if (_currentHealth == 1)
            {
                GameManager.Instance.board.SetCellTile(Cell, lowHeathTile);
            }
            
            return false;
        }
        
        GameManager.Instance.board.SetCellTile(Cell, _originalTile);
        Destroy(gameObject);
        return true;
    }
}
