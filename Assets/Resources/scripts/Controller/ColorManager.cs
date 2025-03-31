using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;
    public ColorData colorData;
    public Image ImageColorWin;
    public Image Color1;
    public Image Color2;
    public Image Color3;
    public Image Color4;
    public int Level;
    public int MoveLimit;
    public TextMeshProUGUI TextMoveLimit;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        this.RegisterListener(EventID.SubMoveLimit, (sedner, param) =>
        {
            MoveLimit--;
        });
    }
    void Start()
    {
        if (colorData == null)
        {
            Debug.LogError("ColorData chưa được gán vào ColorManager!");
            return;
        }
        LoadColors();
        AsignColor();
    }
    private void Update()
    {
        TextMoveLimit.text = MoveLimit.ToString();
    }
    public void LoadColors()
    {
        Level = GridManager.Instance.currentLevelIndex;
        List<List<InforData>> allColors = colorData.GetAllColors();
        List<InforData> levelData = allColors[Level];
        if (levelData.Count > 0)
        {
            string hexColor = levelData[0].finalColor;
            if (ColorUtility.TryParseHtmlString(hexColor, out Color finalColor))
            {
                ImageColorWin.color = finalColor;
            }
            else
            {
                Debug.LogError($" Không thể chuyển {hexColor} thành màu!");
            }
        }
        else
        {
            Debug.LogError($" Level {Level} không có dữ liệu màu!");
        }
    }
    public void AsignColor()
    {
        Level = GridManager.Instance.currentLevelIndex;
        List<List<InforData>> allColors = colorData.GetAllColors();
        List<InforData> levelData = allColors[Level];
        if (levelData.Count > 0)
        {
            if (ColorUtility.TryParseHtmlString(levelData[0].colorCode1, out Color parsedColor1))
            {
                Color1.color = parsedColor1;
            }
            if (ColorUtility.TryParseHtmlString(levelData[0].colorCode2, out Color parsedColor2))
            {
                Color2.color = parsedColor2;
            }

            if (ColorUtility.TryParseHtmlString(levelData[0].colorCode3, out Color parsedColor3))
            {
                Color3.color = parsedColor3;
            }

            if (ColorUtility.TryParseHtmlString(levelData[0].colorCode4, out Color parsedColor4))
            {
                Color4.color = parsedColor4;
            }
            MoveLimit = levelData[0].moveLimit;
        }
    }
    public void CheckWin()
    {
        if (GridManager.Instance == null || GridManager.Instance.gridTiles == null || GridManager.Instance.gridTiles.Count == 0)
        {
            return;
        }

        bool allMatch = true;

        foreach (var tile in GridManager.Instance.gridTiles.Values)
        {
            if (tile.GetComponent<Image>().color != ImageColorWin.color)
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch)
        {
            this.PostEvent(EventID.addScore);
        }
        else if (MoveLimit == 0 && !allMatch)
        {
            this.PostEvent(EventID.LoseGame);
        }
    }
}
