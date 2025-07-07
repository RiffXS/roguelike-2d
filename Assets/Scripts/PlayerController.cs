using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2Int playerPosition;
    
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private BoardManager _boardManager;

    private bool _gameOver;
    
    private float _time;
    private bool _canMove = true;
    private InputAction _enterAction;
    private InputAction _moveAction;
    private PlayerActionsScript _playerActions;
    
    public float movementSpeed = 5.0f;
    
    [Header("Movement")]
    private bool _isMoving;
    private Vector3 _moveTarget;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _playerActions = new PlayerActionsScript();
        
        _moveAction = _playerActions.Player.Move;
        _enterAction = _playerActions.Player.Enter;

        _moveAction.Enable();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gameOver)
        {
            if (_enterAction.IsPressed())
            {
                GameManager.Instance.StartNewGame();
                _gameOver = false;
                _enterAction.Disable();
            }
            return;
        }

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, movementSpeed * Time.deltaTime);

            if (transform.position == _moveTarget)
            {
                _isMoving = false;
                _animator.SetBool(Moving, false);
                var cellData = _boardManager.GetCellData(playerPosition);
                cellData.ContainedObject?.PlayerEntered();
            }
        }
        
        bool hasMoved = false;
        
        if (_canMove)
        {
            Vector2Int newPlayerPosition = playerPosition;
            
            Vector2Int valueToAdd = new Vector2Int((int)_moveAction.ReadValue<Vector2>().x, (int)_moveAction.ReadValue<Vector2>().y);

            if (valueToAdd != Vector2Int.zero)
            {
                hasMoved = true;
            }

            if (hasMoved)
            {
                newPlayerPosition += valueToAdd;
                
                BoardManager.CellData cellData = _boardManager.GetCellData(newPlayerPosition);

                if (cellData is { IsPassable: true })
                {
                    GameManager.Instance.TurnManager.Tick();

                    _canMove = false;
                    if (!cellData.ContainedObject)
                    {
                        MoveTo(newPlayerPosition);
                    }
                    else if (cellData.ContainedObject.PlayerWantsToEnter())
                    {
                        MoveTo(newPlayerPosition);
                    }
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

    public void GameOver()
    {
        _gameOver = true;
        _enterAction.Enable();
    }
    
    // Spawns the Character
    public void Spawn(BoardManager boardManager, Vector2Int position)
    {
        _boardManager = boardManager;
        
        MoveTo(position, true);
    }

    private void MoveTo(Vector2Int newPlayerPosition, bool immediate = false)
    {
        playerPosition = newPlayerPosition;

        if (immediate)
        {
            _isMoving = false;
            transform.position = _boardManager.CellToWorld(playerPosition);
        }
        else
        {
            _isMoving = true;
            _moveTarget = _boardManager.CellToWorld(playerPosition);
            _animator.SetBool(Moving, true);
        }
        
    }

    public void Attack()
    {
        _animator.SetTrigger(Attack1);
    }
}
