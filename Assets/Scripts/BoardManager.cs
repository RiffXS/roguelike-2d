using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool IsPassable;
    }
    
    private CellData[,] _boardData;
    
    private Tilemap _tilemap;
    private Grid _grid;

    public PlayerController player;

    public int width;
    public int height;
    [Header("Tiles")]
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        _grid = GetComponentInChildren<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();

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
                }
                
                _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
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
}
