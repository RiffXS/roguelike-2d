using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap _mTilemap;

    public int width;
    public int height;
    [Header("Tiles")]
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mTilemap = GetComponentInChildren<Tilemap>();

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Tile tile;
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                }
                
                _mTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
