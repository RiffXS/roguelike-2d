using System;
using UnityEngine;

public class Enemy : CellObject
{
    [Header("Health")]
    private int _currentHealth;
    [SerializeField]
    private int maxHealth = 3;

    public void Awake()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }

    public void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        
        _currentHealth = maxHealth;
    }
    
    public override bool PlayerWantsToEnter()
    {
        GameManager.Instance.playerController.Attack();
        _currentHealth -= 1;

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        return false;
    }

    private bool MoveTo(Vector2Int newEnemyPosition)
    {
        var board = GameManager.Instance.board;
        var targetCell = board.GetCellData(newEnemyPosition);

        if (targetCell is not { IsPassable: true } || targetCell.ContainedObject != null)
        {
            return false;
        }

        var currentCell = board.GetCellData(Cell);
        currentCell.ContainedObject = null;
        
        targetCell.ContainedObject = this;
        Cell = newEnemyPosition;
        transform.position = board.CellToWorld(newEnemyPosition);

        return true;
    }
    
    private void TurnHappened()
    { 
        var playerCell = GameManager.Instance.playerController.playerPosition;

        int xDist = playerCell.x - Cell.x;
        int yDist = playerCell.y - Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if ((xDist == 0 && absYDist == 1)
            || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instance.ChangeFood(-3);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    //if our move was not successful (so no move and not attack)
                    //we try to move along Y
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }
    }

    private bool TryMoveInX(int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(Cell + Vector2Int.right);
        }
        
        return MoveTo(Cell + Vector2Int.left);
    }

    private bool TryMoveInY(int yDist)
    {
        if (yDist > 0)
        {
            return MoveTo(Cell + Vector2Int.up);
        }

        return MoveTo(Cell + Vector2Int.down);
    }
}
