using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int Cell;

    public virtual void Init(Vector2Int cell)
    {
        Cell = cell;
    }

    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }
    
    public virtual void PlayerEntered() { }
}
