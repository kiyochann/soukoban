using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase wallTile;
    public TileBase floorTile;

    public int width = 20;
    public int height = 20;
    [Range(0, 1)] public float wallDensity = 0.3f; // 壁ができる確率

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // 1. 2次元配列を作成
        int[,] mapData = new int[width, height];

        // 2. 配列にランダムな値を代入
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Random.value(0.0~1.0) が密度より小さければ壁(1)、大きければ床(0)
                mapData[x, y] = (Random.value < wallDensity) ? 1 : 0;
            }
        }

        // 3. 配列を元にタイルを描画
        DrawMap(mapData);
    }

    void DrawMap(int[,] mapData)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase selectedTile = (mapData[x, y] == 1) ? wallTile : floorTile;
                tilemap.SetTile(new Vector3Int(x, y, 0), selectedTile);
            }
        }
    }
}