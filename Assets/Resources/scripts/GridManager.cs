using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [Header("Data References")]
    public DataLevel datalevel;  
    public ColorData colorData; 

    public GameObject tilePrefab; 
    public Transform gridParent;  
    private Dictionary<Vector2Int, TileController> gridTiles = new Dictionary<Vector2Int, TileController>(); 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        List<DataLevel.Level> levelData = datalevel.Conten.lv3;
        if (levelData == null || levelData.Count == 0)
        {
            Debug.Log("ko tim thay data");
            return;
        }

        for (int y = 0; y < levelData.Count; y++)
        {
            List<string> row = levelData[y].GetAllColums();
            for (int x = 0; x < row.Count; x++)
            {
                int colorID;
                if (int.TryParse(row[x], out colorID))
                {
                    Color? tileColor = GetColorFromID(colorID); // Lấy màu từ ID
                    SpawnTile(x, y, tileColor.Value);
                }
                else
                {
                    Debug.LogError($" Invalid color ID at ({x}, {y}): {row[x]}");
                }
            }
        }
    }
    void SpawnTile(int x, int y, Color tileColor)
    {
        GameObject newTile = SmartPool.Instance.Spawn(tilePrefab, gridParent.transform.position,gridParent.transform.rotation);
        newTile.transform.SetParent(gridParent.transform, false);   
        TileController tileController = newTile.GetComponent<TileController>();
        if (tileController != null)
        {
            gridTiles[new Vector2Int(x, y)] = tileController;
            tileController.SetPosition(x, y);
            tileController.SetColor(tileColor);
        }
    }
    Color? GetColorFromID(int id)
    {
        foreach (var colorEntry in colorData.ContentContent.Hex_Color)
        {
            if (colorEntry.ID == id) // Tìm màu có ID tương ứng
            {
                if (ColorUtility.TryParseHtmlString(colorEntry.HEX, out Color color))
                {
                    return color; // Trả về màu đúng từ HEX
                }
                else
                {
                    Debug.LogError($"⚠️ Invalid HEX code for ID {id}: {colorEntry.HEX}");
                }
            }
        }
        return null; // Nếu không tìm thấy, trả về màu đen
    }
    public void ChangeConnectedTiles(int startX, int startY, Color originalColor, Color newColor)
    {
        StartCoroutine(SpreadColor(startX, startY, originalColor, newColor));
    }

    IEnumerator SpreadColor(int startX, int startY, Color originalColor, Color newColor)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(new Vector2Int(startX, startY));
        visited.Add(new Vector2Int(startX, startY));
        float delay = 1f; // Mỗi lần lan có độ trễ 0.1s
        while (queue.Count > 0)
        {
            int waveSize = queue.Count; // Số ô hiện tại trong đợt lan này

            for (int i = 0; i < waveSize; i++)
            {
                Vector2Int pos = queue.Dequeue();
                if (gridTiles.ContainsKey(pos))
                {
                    TileController tile = gridTiles[pos];
                    Image tileImage = tile.GetComponent<Image>();

                    if (tileImage.color == originalColor)
                    {
                        tileImage.color = newColor;
                    }
                }
                Vector2Int[] directions = {
                new Vector2Int(-1, 0), 
                new Vector2Int(1, 0),  
                new Vector2Int(0, -1),
                new Vector2Int(0, 1)   
            };

                foreach (var dir in directions)
                {
                    Vector2Int neighbor = pos + dir;
                    if (!visited.Contains(neighbor) && gridTiles.ContainsKey(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            yield return new WaitForSeconds(delay); // Chờ trước khi lan sang ô tiếp theo
        }
    }
}
