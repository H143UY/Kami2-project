using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
[CreateAssetMenu(fileName = "DataLevel", menuName = "DataLevel")]
public class DataLevel : SpreadsheetsContainerBase
{
    [SpreadsheetContent]
    [SerializeField] listLevel content;
    public listLevel Conten => content;
    public List<List<Level>> GetAllLevels()
    {
        List<List<Level>> alllevels = new List<List<Level>>();
        FieldInfo[] fields = typeof(List<Level>).GetFields(BindingFlags.Public | BindingFlags.Static);  
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(List<Level>))
            {
                List<Level> levelList = (List<Level>)field.GetValue(this);
                if (levelList != null)
                {
                    alllevels.Add(levelList);
                }
            }
        }
        return alllevels;
    }
    [Serializable]
    public class Level
    {
        public string Colum_1;
        public string Colum_2;
        public string Colum_3;
        public string Colum_4;
        public string Colum_5;
        public string Colum_6;
        public string Colum_7;
        public string Colum_8;
        public List<string> GetAllColums()
        {
            return new List<string>
            {
                Colum_1,Colum_2, Colum_3, Colum_4,Colum_5, Colum_6, Colum_7, Colum_8
            };
        }
    }
    [Serializable]
    public class listLevel
    {
        [SpreadsheetPage ("Level_1")]
        public List<Level> lv1;
        [SpreadsheetPage("Level_2")]
        public List<Level> lv2;
        [SpreadsheetPage("Level_3")]
        public List<Level> lv3;
        [SpreadsheetPage("Level_4")]
        public List<Level> lv4;
        [SpreadsheetPage("Level_5")]
        public List<Level> lv5;
        [SpreadsheetPage("Level_6")]
        public List<Level> lv6;
        [SpreadsheetPage("Level_7")]
        public List<Level> lv7;
        [SpreadsheetPage("Level_8")]
        public List<Level> lv8;
        [SpreadsheetPage("Level_9")]
        public List<Level> lv9;
        [SpreadsheetPage("Level_10")]
        public List<Level> lv10;
        [SpreadsheetPage("Level_11")]
        public List<Level> lv11;

    }
}