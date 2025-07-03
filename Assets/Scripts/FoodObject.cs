using UnityEngine;

public class FoodObject : CellObject
{
    public int amountGranted = 10;
    
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        
        GameManager.Instance.ChangeFood(amountGranted);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
