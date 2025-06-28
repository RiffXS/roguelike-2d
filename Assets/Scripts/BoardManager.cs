using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool IsPassable;
        public CellObject ContainedObject;
    }
    
    private CellData[,] _boardData;
    
    private Tilemap _tilemap;
    private Grid _grid;
    private List<Vector2Int> _emptyCellsList;

    public PlayerController player;
    
    public int width;
    public int height;
    [Header("Tiles")]
    public Tile[] groundTiles;
    public Tile[] wallTiles;

    [Header("Foods")]
    [SerializeField] int minimumFood;
    [SerializeField] int maximumFood;
    [SerializeField] private FoodObject[] foodPrefabs;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        _grid = GetComponentInChildren<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();
        _emptyCellsList = new List<Vector2Int>();

        _boardData = new CellData[width, height];
        
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Tile tile;
                _boardData[x, y] = new CellData();
                
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    _boardData[x, y].IsPassable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    _boardData[x, y].IsPassable = true;
                    _emptyCellsList.Add(new Vector2Int(x, y));
                }
                
                _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        
        _emptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateFood();
    }
    
    public Vector3 CellToWorld(Vector2Int cell)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cell);
    }

    public CellData GetCellData(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= width || cell.y < 0 || cell.y >= height)
        {
            return null;
        }
        
        return _boardData[cell.x, cell.y];
    }

    void GenerateFood()
    {
        int foodCount = Random.Range(minimumFood, maximumFood + 1);

        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, _emptyCellsList.Count);
            Vector2Int coord = _emptyCellsList[randomIndex];
            
            _emptyCellsList.RemoveAt(randomIndex);
            CellData data = _boardData[coord.x, coord.y];
            FoodObject newFood = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
            newFood.transform.position = CellToWorld(coord);
            data.ContainedObject = newFood;
        }
    }
}
