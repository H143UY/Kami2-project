using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
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
        // Duyệt qua tất cả các Field trong class listLevel
        FieldInfo[] fields = typeof(listInfor).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            // Kiểm tra nếu field đó là List<Level> thì thêm vào danh sách
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
    public int ID;
    public string HEX;
    
}

[Serializable]
public class listInfor
{
    [SpreadsheetPage("Color")]
    public List<InforData> Hex_Color;
}
