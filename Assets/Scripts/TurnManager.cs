using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;
    
    private int _turnCount;

    public TurnManager()
    {
        _turnCount = 1;
    }

    public void Tick()
    {
        _turnCount++;
        Debug.Log("Current turn count : " + _turnCount);
        OnTick?.Invoke();
    }
}
