using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField]
    private WallObject[] wallPrefabs;

    [Header("Foods")]
    [SerializeField]
    private int minimumFood;
    [SerializeField]
    private int maximumFood;
    [SerializeField]
    private FoodObject[] foodPrefabs;
    
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
        GenerateWall();
        GenerateFood();
    }
    
    public Vector3 CellToWorld(Vector2Int cell)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cell);
    }

    public Tile GetCellTile(Vector2Int cell)
    {
        return _tilemap.GetTile<Tile>(new Vector3Int(cell.x, cell.y, 0));
    }
    
    public void SetCellTile(Vector2Int cell, Tile tile)
    {
        _tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
    }
    
    public CellData GetCellData(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= width || cell.y < 0 || cell.y >= height)
        {
            return null;
        }
        
        return _boardData[cell.x, cell.y];
    }

    private void AddObject(CellObject obj, Vector2Int coord)
    {
        var data = _boardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }
    
    private void GenerateFood()
    {
        var foodCount = Random.Range(minimumFood, maximumFood + 1);

        for (var i = 0; i < foodCount; ++i)
        {
            var randomIndex = Random.Range(0, _emptyCellsList.Count);
            var coord = _emptyCellsList[randomIndex];
            
            _emptyCellsList.RemoveAt(randomIndex);
            var newFood = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
            AddObject(newFood, coord);
        }
    }

    private void GenerateWall()
    {
        var wallCount = Random.Range(6, 10);
        
        for (var i = 0; i < wallCount; ++i)
        {
            var randomIndex = Random.Range(0, _emptyCellsList.Count);
            var coord = _emptyCellsList[randomIndex];
            
            _emptyCellsList.RemoveAt(randomIndex);
            var newWall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]);
            AddObject(newWall, coord);
        }
    }
}
