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
    public Dictionary<Vector2Int, TileController> gridTiles = new Dictionary<Vector2Int, TileController>();
    public int currentLevelIndex;
    private List<List<DataLevel.Level>> levels; //
    private int tilesProcessing = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        this.RegisterListener(EventID.addScore, (sender, param) =>
        {
            NextLevel();
        });
    }
    void Start()
    {
        levels = new List<List<DataLevel.Level>>()
        {
            datalevel.Conten.lv1,
            datalevel.Conten.lv2,
            datalevel.Conten.lv3,
            datalevel.Conten.lv4,
            datalevel.Conten.lv5,
            datalevel.Conten.lv6,
            datalevel.Conten.lv7,
            datalevel.Conten.lv8,
            datalevel.Conten.lv9,
            datalevel.Conten.lv10,
            datalevel.Conten.lv11,
        };
        currentLevelIndex = GameManager.instance.selectedLevel;
        LoadGame();
    }
    void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveLevel"))
        {
            currentLevelIndex = PlayerPrefs.GetInt("SavedLevel");
            GenerateGrid();

            foreach (var tile in gridTiles)
            {
                Vector2Int pos = tile.Key;
                string tileKey = $"Tile_{pos.x}_{pos.y}";

                if (PlayerPrefs.HasKey(tileKey))
                {
                    string savedColor = PlayerPrefs.GetString(tileKey);
                    Color tileColor;
                    if (ColorUtility.TryParseHtmlString("#" + savedColor, out tileColor))
                    {
                        tile.Value.SetColor(tileColor);
                    }
                }
            }
            Debug.Log("Game đã được tải lại!");
        }
        else
        {
            GenerateGrid();
        }
    }
    public void GenerateGrid()
    {
        List<DataLevel.Level> levelData = levels[currentLevelIndex];
        if (levelData == null || levelData.Count == 0)
        {
            Debug.Log("Không tìm thấy data");
            return;
        }

        for (int y = 0; y < levelData.Count; y++)
        {
            List<string> row = levelData[y].GetAllColums();
            for (int x = 0; x < row.Count; x++)
            {
                string hexColor = ExtraColor(row[x]);
                Color tileColor;

                if (ColorUtility.TryParseHtmlString(hexColor, out tileColor))
                {
                    SpawnTile(x, y, tileColor);
                }
                else
                {
                    Debug.LogError($" Mã màu không hợp lệ tại ({x}, {y}): {row[x]}");
                }
            }
        }
    }
    string ExtraColor(string HexColor)
    {
        string[] parts = HexColor.Split('_');
        return (parts.Length > 1) ? parts[1] : HexColor;
    }
    void SpawnTile(int x, int y, Color tileColor)
    {
        GameObject newTile = SmartPool.Instance.Spawn(tilePrefab, gridParent.transform.position, gridParent.transform.rotation);
        newTile.transform.SetParent(gridParent.transform, false);
        TileController tileController = newTile.GetComponent<TileController>();
        if (tileController != null)
        {
            gridTiles[new Vector2Int(x, y)] = tileController;
            tileController.SetPosition(x, y);
            tileController.SetColor(tileColor);
        }
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

        tilesProcessing = 0; // Reset bộ đếm
        float TimeDelay = 0.5f;

        while (queue.Count > 0)
        {
            int waveSize = queue.Count;
            tilesProcessing += waveSize; // Ghi nhận số ô sẽ đổi màu

            for (int i = 0; i < waveSize; i++)
            {
                Vector2Int pos = queue.Dequeue();
                if (!gridTiles.ContainsKey(pos)) continue;

                TileController tile = gridTiles[pos];
                Image tileImage = tile.GetComponent<Image>();

                if (tileImage.color == originalColor)
                {
                    tileImage.color = newColor;
                }
                else
                {
                    tilesProcessing--; // Nếu không đổi màu, giảm bộ đếm
                    continue;
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
                    if (visited.Contains(neighbor)) continue;
                    if (gridTiles.ContainsKey(neighbor))
                    {
                        Image neighborImage = gridTiles[neighbor].GetComponent<Image>();
                        if (neighborImage.color == originalColor)
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }

                tilesProcessing--; // Đánh dấu ô này đã xử lý xong
            }

            yield return new WaitForSeconds(TimeDelay);
        }

        // Đợi cho đến khi toàn bộ ô đã được xử lý
        while (tilesProcessing > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Chờ UI cập nhật hoàn toàn
        ColorManager.Instance.CheckWin(); // Chỉ gọi khi đã lan màu xong
    }
    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count)
        {
            ClearGrid();
            GenerateGrid();
            if (ColorManager.Instance != null)
            {
                ColorManager.Instance.LoadColors();
                ColorManager.Instance.AsignColor();
            }
            Debug.Log($"chuyển cấp {currentLevelIndex + 1}");
        }
        else
        {
            Debug.Log("Đã hoàn thành tất cả cấp độ!");
            this.PostEvent(EventID.WinGame);
        }
    }
    public void ClearGrid()
    {
        foreach (var tile in gridTiles.Values)
        {
            Destroy(tile.gameObject);
        }
        gridTiles.Clear();
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("SaveLevel", currentLevelIndex);
        Debug.Log(currentLevelIndex + "a");
        foreach (var tile in gridTiles)
        {
            Vector2Int pos = tile.Key;
            TileController tileController = tile.Value;
            string tileKey = $"Tile_{pos.x}_{pos.y}";
            string tileColor = ColorUtility.ToHtmlStringRGB(tileController.GetColor());
            PlayerPrefs.SetString(tileKey, tileColor);
        }
        PlayerPrefs.Save();
        Debug.Log("Game đã được lưu!");
    }

}