using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;
    private Vector2Int _playerPosition;

    private bool _canMove = true;
    private float _time = 0f;
    private InputAction _movementAction;
    private PlayerActionsScript _playerActions;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerActions = new PlayerActionsScript();

        _movementAction = _playerActions.Player.Move;

        _movementAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        bool hasMoved = false;
        
        if (_canMove)
        {
            Vector2Int newPlayerPosition = _playerPosition;
            
            Vector2Int valueToAdd = new Vector2Int((int)_movementAction.ReadValue<Vector2>().x, (int)_movementAction.ReadValue<Vector2>().y);

            if (valueToAdd != Vector2Int.zero)
            {
                hasMoved = true;
            }

            if (hasMoved)
            {
                newPlayerPosition += valueToAdd;
                
                BoardManager.CellData cellData = _boardManager.GetCellData(newPlayerPosition);

                if (cellData != null)
                {
                    GameManager.Instance.TurnManager.Tick();
                    MoveTo(newPlayerPosition);
                }
            }
        }
        else
        {
            _time += Time.deltaTime;

            if (_time > 0.3f)
            {
                _canMove = true;
                _time = 0f;
            }
        }
    }
    
    // Spawns the Character
    public void Spawn(BoardManager boardManager, Vector2Int playerPosition)
    {
        _boardManager = boardManager;
        
        MoveTo(playerPosition);
    }

    private void MoveTo(Vector2Int newPlayerPosition)
    {
        _canMove = false;
        _playerPosition = newPlayerPosition;
        transform.position = _boardManager.CellToWorld(_playerPosition);
    }
}
