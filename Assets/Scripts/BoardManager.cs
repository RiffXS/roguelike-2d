using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool IsPassable;
    }
    
    private CellData[,] _mBoardData;
    
    private Tilemap _mTilemap;
    private Grid _mGrid;

    public PlayerController player;

    public int width;
    public int height;
    [Header("Tiles")]
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mGrid = GetComponentInChildren<Grid>();
        _mTilemap = GetComponentInChildren<Tilemap>();

        _mBoardData = new CellData[width, height];
        
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Tile tile;
                _mBoardData[x, y] = new CellData();
                
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    _mBoardData[x, y].IsPassable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    _mBoardData[x, y].IsPassable = true;
                }
                
                _mTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        
        player.Spawn(this, new Vector2Int(1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return _mGrid.GetCellCenterWorld((Vector3Int)cell);
    }
}
