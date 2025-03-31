using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static DataLevel;
[CreateAssetMenu(menuName = "DataColor", fileName = "DataColor")]
public class ColorData : SpreadsheetsContainerBase
{
    [SpreadsheetContent]
    [SerializeField] listInfor content;
    public listInfor ContentContent => content;

    public List<List<InforData>> GetAllColors()
    {
        List<List<InforData>> allColors = new List<List<InforData>>();
        FieldInfo[] fields = typeof(listInfor).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(List<InforData>))
            {
                List<InforData> levelList = (List<InforData>)field.GetValue(content);
                if (levelList != null)
                {
                    allColors.Add(levelList);
                }
            }
        }
        return allColors;
    }
}
[Serializable]

public class InforData
{
    public string colorCode1;
    public string colorCode2;
    public string colorCode3;
    public string colorCode4;
    public string finalColor;
    public int moveLimit;
}

[Serializable]
public class listInfor
{
    [SpreadsheetPage("Level_1")]
    public List<InforData> lv1;
    [SpreadsheetPage("Level_2")]
    public List<InforData> lv2;
    [SpreadsheetPage("Level_3")]
    public List<InforData> lv3;
    [SpreadsheetPage("Level_4")]
    public List<InforData> lv4;
    [SpreadsheetPage("Level_5")]
    public List<InforData> lv5;
    [SpreadsheetPage("Level_6")]
    public List<InforData> lv6;
    [SpreadsheetPage("Level_7")]
    public List<InforData> lv7;
    [SpreadsheetPage("Level_8")]
    public List<InforData> lv8;
    [SpreadsheetPage("Level_9")]
    public List<InforData> lv9;
    [SpreadsheetPage("Level_10")]
    public List<InforData> lv10;
    [SpreadsheetPage("Level_11")]
    public List<InforData> lv11;
}
